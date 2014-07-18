using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PictoUI.Common
{
    public class Base64Converter
    {
        public static async Task<string> FromStorageFile(StorageFile file)
        {
            var base64String = "";

            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(fileStream.GetInputStreamAt(0));
                await reader.LoadAsync((uint)fileStream.Size);
                byte[] byteArray = new byte[fileStream.Size];
                reader.ReadBytes(byteArray);
                base64String = Convert.ToBase64String(byteArray);
            }

            return base64String;
        }

        public static BitmapImage ToBitmap(string value)
        {
            var bm = new BitmapImage();
            bm.SetSource(ToStream(value));
            return bm;
        }

        public static IRandomAccessStream ToStream(string value)
        {
            var imgBytes = Convert.FromBase64String(value);
            var ms = new InMemoryRandomAccessStream();
            var dw = new DataWriter(ms);
            dw.WriteBytes(imgBytes);
            dw.StoreAsync();
            ms.Seek(0);

            return ms;
        }
    }
}