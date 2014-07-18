using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace PictoTest
{
    public class StorageFileConverter
    {
        public static async Task<string> ConvertToBase64(StorageFile file)
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
    }
}