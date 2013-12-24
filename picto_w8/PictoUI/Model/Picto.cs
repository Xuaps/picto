using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PictoUI.Model
{
    public class Picto
    {
        private ICollection<Picto> _children;

        public Picto(ICollection<Picto> children, string text, string sound, string image)
        {
            _children = children;
            Text = text;
            Sound = sound;
            Image = image;
        }

        public Picto(string text)
        {
            Text = text;
            _children=new ObservableCollection<Picto>();
        }

        public ICollection<Picto> Children
        {
            get { return _children; }
        }

        public string Text { get; private set; }

        public string Sound { get; set; }

        public string Image { get; set; }
    }
}
