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
    public partial class ChatPage : ContentPage
    {
        Database.Firebase db = new Database.Firebase();
        Model.Conversation rm = new Model.Conversation();

        public string ConvoKey;

        public ChatPage()
        {
            InitializeComponent();
            
            MessagingCenter.Subscribe<Pages.ConversationPage, Model.Conversation>(this, "ConvoProp", (page, data) =>
            {
                rm = data;

                _lstChat.BindingContext = db.subChat(data.Key);
                Title = data.Name;
                ConvoKey = data.Key;

                MessagingCenter.Unsubscribe<Pages.ConversationPage, Model.Conversation>(this, "ConvoProp");
            });

            
        }

        public async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            var person = await db.GetPerson(oauthToken);
            var chatOBJ = new Model.Chat { UserMessage = _etMessage.Text, UserId = person.FullName };
 
            await db.saveMessage(chatOBJ, rm.Key);
            _etMessage.Text = string.Empty;
        }

        private async void GenerateBarcode(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodePage(ConvoKey, Title));
        }
    }
}