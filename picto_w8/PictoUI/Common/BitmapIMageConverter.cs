using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PictoUI.Common
{
    public class BitmapImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(targetType !=typeof(ImageSource))
                throw new ArgumentException("Invalid target type");

            var imgBytes = System.Convert.FromBase64String(value.ToString());
            var ms = new InMemoryRandomAccessStream();
            var dw = new DataWriter(ms);
            dw.WriteBytes(imgBytes);
            dw.StoreAsync();
            ms.Seek(0);

            var bm = new BitmapImage();
            bm.SetSource(ms);
            return bm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(targetType!=typeof(string))
                throw new ArgumentException("Invalid target type");
            if (value == null)
                return "";

            return (value as BitmapImage).UriSource.ToString();
        }
    }
}
