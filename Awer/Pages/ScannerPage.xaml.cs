using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ZXing;


namespace Awer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage: ContentPage
    {
        Database.Firebase db = new Database.Firebase();
        public ScannerPage()
        {
            InitializeComponent();
        }

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

                await Navigation.PopToRootAsync();

                if (result.Text == "None")
                {
                    await DisplayAlert("Oops", "No such chats exist.", "Try Again");
                } else
                {
                    var oauthToken = await SecureStorage.GetAsync("oauth_token");
                    var person = await db.GetPerson(oauthToken);

                    var _person = person.Key;
                    var _name = person.FullName;

                    await db.joinConversation2(result.Text, _person, _name);

                    
                }
                
            });
        }



    }
}