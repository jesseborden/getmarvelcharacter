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
                LblError.Text = "Can't set marvel character.";
            }
        }
        private void GetMarvelCharacter()
        {

            var backgroundtimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            backgroundtimer.Tick += async (o, e) =>
            {
                //Try to get Iron Man
                MarvelAPI.RootObject mycharacter = await MarvelAPI.GetMarvelCharacter.GetCharacter(1009610);
                //Make sure you recieved good data. Class returns null if something is broken.
                if (mycharacter != null)
                {
                    int resultindex = (mycharacter.data.results.Count) - 1;
                    Random random = new Random();
                    int randomindex = random.Next(0, resultindex);
                    string badimage = "image_not_available";
                    string imageurl = mycharacter.data.results[randomindex].thumbnail.path + "." + mycharacter.data.results[randomindex].thumbnail.extension;
                    //If the image is not available, change background to wonderwoman image. You need to add your own image, I am only providing a blank image.
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
