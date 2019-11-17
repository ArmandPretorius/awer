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

                try
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Welcome!", $"You have successfully joined the group", "Thanks");

                   
                    var oauthToken = await SecureStorage.GetAsync("oauth_token");
                    var person = await db.GetPerson(oauthToken);

                    var _person = person.Key;
                    var _name = person.FullName;

                    //All Conversations
                    
                   
                    await db.joinConversation2(result.Text, _person, _name);
                    


                    //foreach (var group in list)
                    //{
                    //    if(result.Text != group.Name)
                    //    {
                    //        await DisplayAlert("Oops", "We couldn't find a group", "Try Again");
                    //    } else
                    //    {
                    //        await db.joinConversation2(result.Text, _person, _name);
                    //        await Navigation.PopAsync(true);
                    //    }
                    //}
                }
                catch
                {
                    await DisplayAlert("Oops", "We couldn't find a group", "Try Again");
                }   
            });
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }



    }
}