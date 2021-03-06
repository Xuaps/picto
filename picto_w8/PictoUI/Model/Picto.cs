using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PictoUI.Model
{
    public class Picto
    {
        public Picto()
        {
        }

        public Picto(int id, string key, string text, string image, string sound="", ICollection<Picto> children=null)
        {
            Children = children;
            Text = text;
            Sound = sound;
            Image = image;
            Key = key;
            Id = id;
        }

        public Picto(string text)
        {
            Text = text;
            Children=new ObservableCollection<Picto>();
        }

        public ICollection<Picto> Children { get; set; }

        public int Id { get; set; }

        public string Key { get; set; }
        
        public string Text { get; set; }

        public string Sound { get; set; }

        public string Image { get; set; }
    }
}
