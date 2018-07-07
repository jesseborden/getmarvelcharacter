using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GetMarvelCharacter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            try
            {
                GetMarvelCharacter();
            }
            catch
            {
                LblError.Text = "can't set marvel character. Is the network connection down?";
            }
        }
        private void GetMarvelCharacter()
        {

            var backgroundtimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            backgroundtimer.Tick += async (o, e) =>
            {
                MarvelAPI.RootObject mycharacter = await MarvelAPI.GetMarvelCharacter.GetCharacter(1009610);

                //Change Background to Iron Man
                if (mycharacter != null)
                {
                    int resultindex = (mycharacter.data.results.Count) - 1;
                    Random random = new Random();
                    int randomindex = random.Next(0, resultindex);
                    string badimage = "image_not_available";
                    string imageurl = mycharacter.data.results[randomindex].thumbnail.path + "." + mycharacter.data.results[randomindex].thumbnail.extension;
                    if (imageurl.Contains(badimage))
                    {
                        imgBackground.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Backgrounds/wonderwoman.png"));
                        LblCharacter.Text = "Wonder Woman :)";
                    }
                    if (!imageurl.Contains(badimage))
                    {
                        imgBackground.Source = new BitmapImage(new Uri(imageurl, UriKind.Absolute));
                        LblCharacter.Text = mycharacter.data.results[randomindex].name;
                    }
                }
                else
                {
                    imgBackground.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Backgrounds/wonderwoman.png"));
                    LblCharacter.Text = "Wonder Woman :)";
                }
                };
                backgroundtimer.Start();
    }
    }
}
