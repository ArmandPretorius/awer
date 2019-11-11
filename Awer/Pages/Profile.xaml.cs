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
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }

        public async void Handle_Clicked(object sender, System.EventArgs e)
        {
            SecureStorage.Remove("oauth_token");
            await SecureStorage.SetAsync("loggedIn", "false");
            await Navigation.PopModalAsync();
        }
    }
}