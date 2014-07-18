using System;
using PictoUI.Common;
using PictoUI.Model;
using Windows.UI.Xaml.Controls;
using  Windows.Media.SpeechSynthesis;
using Windows.Storage;
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PictoUI
{
    public sealed partial class WordsPanel : UserControl
    {
        private int index = 0;
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        public WordsPanel()
        {
            this.InitializeComponent();
            Player.AutoPlay = true;
            player.Source = null;
        }
        
        public ItemCollection Items { get { return sentence.Items; }}
        public int SelectedIndex { 
            get { return sentence.SelectedIndex; }
            set { sentence.SelectedIndex = value; }
        }
        public MediaElement Player { get { return player; } }

        public void PlayLast()
        {
            index = sentence.Items.Count - 1;
            PlayFrom(sentence.Items.Count-1);
        }

        public void PlayAll()
        {
            index = 0;
            PlayFrom(0);
        }

        public async void PlayFrom(int index)
        {
            if (sentence.Items.Count > index && index>-1)
            {
                var picto = (sentence.Items[index] as Picto);
                
                if(String.IsNullOrEmpty(picto.Sound)){
                    SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(picto.Text);
                    player.SetSource(stream, stream.ContentType);
                }else
                {
                    player.SetSource(Base64Converter.ToStream(picto.Sound),"");
                }
            }
        }

        void player_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            index++;
            PlayFrom(index);
        }
    }
}
