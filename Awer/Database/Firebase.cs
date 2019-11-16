using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Awer.Database
{
    public class Firebase
    {
        FirebaseClient fbClient;
        FirebaseAuthProvider authProvider;


        public Firebase()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCObcI7wNFXD-mGbvSUkYiqk7lPWh7eQWw"));

            fbClient = new FirebaseClient("https://awer-8918c.firebaseio.com/");
            
        }

        //Authentication
        public async Task CheckLogin(string email, string password)
        {
         
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password)

             //.ContinueWith(async authent =>
             //{
             //    if (authent.IsCanceled)
             //    {
             //        Debug.Write("CreateUserWithEmailAndPasswordAsync was canceled.");
             //        return;
             //    }
             //    if (authent.IsFaulted)
             //    {
             //        Debug.Write("CreateUserWithEmailAndPasswordAsync encountered an error: " + authent.Exception.GetBaseException());
             //        return;
             //    }
             //})
             ;

                var user = auth.User.LocalId;
                //try
                //{
                    await SecureStorage.SetAsync("oauth_token", user);
                    await SecureStorage.SetAsync("loggedIn", "true");

                //}
                //catch (Exception ex)
                //{
                //    // Possible that device doesn't support secure storage on device.
                //    Console.WriteLine("Error: " + ex);
                //   // await SecureStorage.SetAsync("loggedIn", "false");
                //}
            
        }

        //Persons
        //Get All Persons
        public async Task<List<Model.Person>> GetAllPersons()
        {

            return (await fbClient
              .Child("Persons")
              .OnceAsync<Model.Person>()).Select(item => new Model.Person
              {
                  FullName = item.Object.FullName,
                  Key = item.Key,
                  Role = item.Object.Role,
                  Uid = item.Object.Uid
              }).ToList();
        }

        //Add Person
        public async Task AddPerson(Model.Person p)
        {
            await fbClient
              .Child("Persons")
              .PostAsync(p);
        }

        //Get One person
        public async Task<Model.Person>GetPerson(string id)
        {
            var allPersons = await GetAllPersons();
            await fbClient
              .Child("Persons")
              .OnceAsync<Model.Person>();
            return allPersons.Where(a => a.Uid == id).FirstOrDefault();
        }

        //Update Person
        public async Task UpdatePerson(string id, string name, string role)
        {
            var toUpdatePerson = (await fbClient
              .Child("Persons")
              .OnceAsync<Model.Person>()).Where(a => a.Object.Uid == id).FirstOrDefault();

            await fbClient
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Model.Person() { FullName = name, Role = role});
        }

        //Delete person
        public async Task DeletePerson(string id)
        {
            var toDeletePerson = (await fbClient
              .Child("Persons")
              .OnceAsync<Model.Person>()).Where(a => a.Object.Uid == id).FirstOrDefault();
            await fbClient.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();
        }



        //Conversations
        //All Convos
        public async Task<List<Model.Conversation>> getConversationList()
        {

              return  (await fbClient
                .Child("Conversation")

                .OnceAsync<Model.Conversation>()
                )
                .Select((item) =>

                new Model.Conversation
                {
                    Key = item.Key,
                    Name = item.Object.Name,
                })
                .ToList();

        }

        //New Conversation
        public async Task createConversation(Model.Conversation convo)
        {
            await fbClient.Child("Conversation")
                .PostAsync(convo);
        }

        //Chats
        //Store messages
        public async Task saveMessage(Model.Chat _ch, string _convo)
        {
            await fbClient.Child("Conversation/" + _convo + "/Message")
                    .PostAsync(_ch);
        }

        //Observe messages
        public ObservableCollection<Model.Chat> subChat(string _convoKEY)
        {

            return fbClient.Child("Conversation/" + _convoKEY + "/Message")
                           .AsObservable<Model.Chat>()
                           .AsObservableCollection<Model.Chat>();
        }

        //Joining Chat

        public async Task joinConversation2(string _convo, string _person, string _personname)
        {
            await fbClient.Child("Conversation/" + _convo + "/Users")
                .PostAsync(new Model.JoinedChat { PersonId = _person, Name = _personname });
        }






    }
}