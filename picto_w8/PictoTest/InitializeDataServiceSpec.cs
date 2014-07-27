using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PictoUI.Model;
using PictoUI.Services;
using SQLite;

namespace PictoTest
{
    [TestClass]
    public class InitializeDataServiceSpec
    {
        private InitialDataService initialDataService;
        private string _baseUrl;
        private string[] _pictos;
        private string _folderName;

        [TestInitialize]
        public void Setup()
        {
            initialDataService = new InitialDataService(new Pictos());
        }

        [TestMethod]
        public void load_initial_data_from_scratch_should_copy_pictos_database()
        {
            initialDataService.LoadInitialData().Wait();
            StorageFile storageFile = ApplicationData.Current.LocalFolder.GetFileAsync("pictos.db").AsTask().Result;
            Assert.IsTrue(storageFile!=null);
        }

        [TestMethod]
        public void load_initial_data_from_early_version_should_copy_and_fill_database()
        {
            arrange_previous_version().Wait();

            initialDataService.LoadInitialData().Wait();
            
            //copy
            StorageFile storageFile = ApplicationData.Current.LocalFolder.GetFileAsync("pictos.db").AsTask().Result;
            Assert.IsTrue(storageFile != null);
            
            //delete files and folders
            Assert.IsFalse(ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized"));
            Assert.ThrowsException<AggregateException>(()=>ApplicationData.Current.LocalFolder.GetFolderAsync("Emotions").AsTask().Result);

            var pictos = new Pictos();
            Assert.AreEqual(5, pictos.GetCategories().Result.Single(c=>c.Text=="Emotions").Children.Count);
        } 

        private async Task arrange_previous_version()
        {
            if (CultureInfo.CurrentCulture.Parent.Name == "es")
            {
                _baseUrl = "https://googledrive.com/host/0B2j1eTLysQsUVXI0YVpLN2VsajA/es-es/";
                _pictos = new[] { "asustado", "confundido", "curioso", "emocionado", "espantado" };
                _folderName = "Emociones";
            }
            else
            {
                _baseUrl = "https://googledrive.com/host/0B2j1eTLysQsUVXI0YVpLN2VsajA/en-us/";
                _pictos = new[] { "afraid", "confused", "curious", "excited", "scared" };
                _folderName = "Emotions";
            }

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized"))
            {
                var folder =
                    await
                    ApplicationData.Current.LocalFolder.CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);

                await CreateFile(folder, _folderName, "png", _baseUrl + _folderName + "/");

                foreach (var picto in _pictos)
                {
                    await CreatePicto(folder, picto);
                }

                ApplicationData.Current.LocalSettings.Values.Add("Initialized", true);

            }
        }

        private async Task CreatePicto(StorageFolder categoryFolder, string picto)
        {
            var subfolder = await categoryFolder.CreateFolderAsync(picto, CreationCollisionOption.OpenIfExists);


            CreateFile(subfolder, picto, "png", _baseUrl + categoryFolder.Name + "/");
            //CreateFile(subfolder, picto, "mp3", _baseUrl + categoryFolder.Name + "/" + subfolder.Name + "/");
        }

        private async Task CreateFile(StorageFolder folder, string picto, string extension, string url)
        {
            var file = await folder.CreateFileAsync(picto + "." + extension, CreationCollisionOption.ReplaceExisting);
            var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            var requestPic = WebRequest.Create(url + picto + "." + extension);

            using (var responsePic = await requestPic.GetResponseAsync())
            using (var outStream = stream.GetOutputStreamAt(0))
            {
                var remoteFile = ReadFully(responsePic.GetResponseStream());
                await outStream.AsStreamForWrite().WriteAsync(remoteFile, 0, remoteFile.Length);
                await outStream.FlushAsync();
            }
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[512];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
