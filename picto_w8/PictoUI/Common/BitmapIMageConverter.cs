using System;
using System.Threading.Tasks;
using Windows.Storage;
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

            return new BitmapImage(new Uri(value as string));
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
