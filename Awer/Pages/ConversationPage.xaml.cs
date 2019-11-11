using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Firebase.Database;
using Firebase.Database.Query;


namespace Awer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationPage : ContentPage
    {
        FirebaseClient fbClient = new FirebaseClient("https://awer-8918c.firebaseio.com/");
        Database.Firebase db = new Database.Firebase();

        public ConversationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Check Logged In User
            var loggedIn = await SecureStorage.GetAsync("loggedIn");
            if (loggedIn == "true")
            {
               
                //All Conversations
                var list = await db.getConversationList();


                //Logged in User's key
                var loggedInPerson = await SecureStorage.GetAsync("oauth_token");
                var person = await db.GetPerson(loggedInPerson);
                var personKey = person.Key;
                var personAuth = person.Role;

                //Second List
                List<Model.Conversation> list2 = new List<Model.Conversation>();

                //For loop to add the logged in users conversations into the second list
                foreach(Model.Conversation conversation in list)
                {
                    var users = await fbClient.Child("Conversation").Child(conversation.Key).Child("Users").OnceAsync<Model.JoinedChat>();

                    foreach(var user in users)
                    {
                        if(user.Object.PersonId == personKey)
                        {
                            list2.Add(conversation);
                        }
                        
                    }   
                }

                //Bind list

                _lstx.BindingContext = list2;

                if (list2.Count == 0)
                {
                    NoChats.IsVisible = true;
                }
                else
                {
                    NoChats.IsVisible = false;
                 
                }
               



                //Check Role to show create Button
                if (personAuth == "Admin")
                {
                    CreateButton.IsVisible = true;
                }
                else
                {
                    CreateButton.IsVisible = false;
                }
            }
            else
            {
                await Navigation.PushModalAsync(new Pages.Login());
            }
        }

        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            var loggedInPerson = await SecureStorage.GetAsync("oauth_token");
            var person = await db.GetPerson(loggedInPerson);
            var personKey = person.Key;
            OnAppearing();
            _lstx.IsRefreshing = false;
        }


        //NAVIGATION TO PROFILE PAGE
        //public async void Info_Clicked(object sender, System.EventArgs e)
        //{
        //    var oauthToken = await SecureStorage.GetAsync("oauth_token");

        //    var Person = await db.GetPerson(oauthToken);
        //    var Name = Person.FullName;

        //    await Navigation.PushModalAsync(new Pages.Profile());
        //}

        public async void Logout_Clicked(object sender, System.EventArgs e)
        {
            SecureStorage.Remove("oauth_token");
            await SecureStorage.SetAsync("loggedIn", "false");
            await Navigation.PopToRootAsync();
            OnAppearing();

        }

        void Create_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Pops.CreateConversationPopup());

        }
        //Send the conversations information to the chats page using MessagingCenter.Send, where it will be observed.
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (_lstx.SelectedItem != null)
            {
                //Set selected conversation information
                var selectConvo = (Model.Conversation)_lstx.SelectedItem;

                //Navigate to Chatpage
                Navigation.PushAsync(new Pages.ChatPage());
     
                //Send Information
                MessagingCenter.Send<Pages.ConversationPage, Model.Conversation>(this, "ConvoProp", selectConvo);

            }
        }


        public async void Join_Convo_Click_2(object sender, System.EventArgs e)
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            var person = await db.GetPerson(oauthToken);

            var _person = person.Key;
            var _convo = "-LgwlxjN2mtkR39cnZhk";
            var _convoname = "James Bond";
            await db.joinConversation2(_convo, _person, _convoname);
        }

        private async void ScanCustomPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScannerPage());
        }

    }
}