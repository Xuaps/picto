using System;
using PictoUI.Model;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PictoUI
{
    public sealed partial class WordsPanel : UserControl
    {
        private int index = 0;

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

        public void PlayFrom(int index)
        {
            if (sentence.Items.Count > index && index>-1)
            {
                player.Source = new Uri((sentence.Items[index] as Picto).Sound);
            }
        }

        void player_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            index++;
            if (index < sentence.Items.Count)
            {
                player.Source = new Uri((sentence.Items[index] as Picto).Sound);
            }
        }
    }
}
