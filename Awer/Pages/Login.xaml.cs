using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Awer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        Database.Firebase db;
        public Login()
        {
            InitializeComponent();
            db = new Database.Firebase();

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}

        public async void Handle_Login(object sender, System.EventArgs e)
        {
            await db.CheckLogin(_email.Text, _password.Text);
            
            await Navigation.PopModalAsync();
            

        }
    }
}