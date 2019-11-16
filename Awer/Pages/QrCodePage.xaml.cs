using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodePage : ContentPage
    {
        public QrCodePage(string chatId, string chatName)
        {
            InitializeComponent();

            BarcodeId.BarcodeValue = chatId;
            BarcodeLabel.Text = chatName + " Barcode";

        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}