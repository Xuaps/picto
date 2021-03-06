using System.Collections.ObjectModel;
using PictoUI.Model;

namespace PictoUI.Dto
{
    public class HomeDto
    {
        public string Key { get; set; }
        public string Category { get; set; }
        public ObservableCollection<Picto> Words { get; set; }
    }
}
