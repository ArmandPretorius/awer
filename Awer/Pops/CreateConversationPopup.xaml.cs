using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Awer.Pops
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateConversationPopup : ContentPage
    {
        FirebaseClient fbClient = new FirebaseClient("https://awer-8918c.firebaseio.com/");

        public string iconSelected = "";
        public CreateConversationPopup()
        {
            InitializeComponent();
        }

        public async void Handle_Clicked(object sender, System.EventArgs e)
        {

            if(_rootName.Text == "" || iconSelected == "")
            {
                await DisplayAlert("Oops", "You need to choose a Name and an Icon", "Try Again");
            } else
            {
                var db = new Database.Firebase();
                Console.WriteLine(iconSelected);
                await db.createConversation(new Model.Conversation() { Name = _rootName.Text, Icon = iconSelected });

                var oauthToken = await SecureStorage.GetAsync("oauth_token");
                var person = await db.GetPerson(oauthToken);

                var _person = person.Key;
                var _name = person.FullName;

                //All Conversations
                var list = await db.getConversationList();


                //For loop to add the logged in users conversations into the second list
                foreach (Model.Conversation conversation in list)
                {
                   if(conversation.Name == _rootName.Text)
                    {
                        await db.joinConversation2(conversation.Key, _person, _name);
                        await Navigation.PopAsync();
                    }
                    
                }
            }

            
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void IconButton_Clicked(object sender, EventArgs e)
        {
            //get icon pressed
            var button = (ImageButton)sender;
            var icon = button.CommandParameter;
            //Console.WriteLine(icon);

            //change icon background
            oneIcon.Source = oneIcon.CommandParameter.ToString();
            twoIcon.Source = twoIcon.CommandParameter.ToString();
            threeIcon.Source = threeIcon.CommandParameter.ToString();
            fourIcon.Source = fourIcon.CommandParameter.ToString();
            fiveIcon.Source = fiveIcon.CommandParameter.ToString();
            sixIcon.Source = sixIcon.CommandParameter.ToString();

            button.Source = icon + "Selected";
            iconSelected = icon.ToString();
        }





        //CREATE USER
        //private async void BtnAdd_Clicked(object sender, EventArgs e)
        //{
        //    var db = new Database.Firebase();

        //    await db.AddPerson(new Model.Person() { FullName = txtName.Text, Role = txtRole.Text});
        //    txtId.Text = string.Empty;
        //    txtName.Text = string.Empty;
        //    txtRole.Text = string.Empty;
        //    await DisplayAlert("Success", "Person Added Successfully", "OK");
        //    var allPersons = await db.GetAllPersons();
        //    lstPersons.ItemsSource = allPersons;
        //}
    }
}