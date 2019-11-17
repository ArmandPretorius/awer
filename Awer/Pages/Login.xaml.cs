using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Firebase.Database;
using Firebase.Database.Query;

namespace Awer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        Database.Firebase db;
        FirebaseClient fbClient = new FirebaseClient("https://awer-8918c.firebaseio.com/");

        public Login()
        {
            InitializeComponent();
            db = new Database.Firebase();

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Check Logged In User
            var loggedIn = await SecureStorage.GetAsync("loggedIn");
            if (loggedIn == "true")
            {
                await Navigation.PushAsync(new ConversationPage());
            }
        }

        public async void Handle_Login(object sender, System.EventArgs e)
        {
            try
            {
                errorMessage.IsVisible = false;
                await db.CheckLogin(_email.Text, _password.Text);
                await Navigation.PushAsync(new ConversationPage());
            }
            catch
            {
                Console.Write("Not Correct");
                errorMessage.IsVisible = true;
                _password.Text = "";
            }
        }
    }
}