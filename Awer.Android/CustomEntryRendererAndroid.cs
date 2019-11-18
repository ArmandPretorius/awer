using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awer.Droid;
using Awer.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntryRenderer), typeof(CustomEntryRendererAndroid))]
namespace Awer.Droid
{
    public class CustomEntryRendererAndroid : EntryRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            int colors = Color.FromHex("#FEEFDF").ToAndroid();
               


            if (e.OldElement == null)
            {
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetColor(colors);
                gradientDrawable.SetStroke(0, Android.Graphics.Color.Transparent);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight, ControlUsedForAutomation.PaddingBottom);
            }
        }
    }
}