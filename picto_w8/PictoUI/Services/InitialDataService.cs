using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using PictoUI.Common;
using PictoUI.Model;

namespace PictoUI.Services
{
    public class InitialDataService
    {
        private readonly IPictos _pictos;

        public InitialDataService(IPictos pictos)
        {
            _pictos = pictos;
        }

        public async Task LoadInitialData()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("pictos.db");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("assets\\pictos.db");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
            await _pictos.Initialize();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized"))
            {
                var categories = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

                foreach (var category in categories)
                {
                    var categoryObject=await _pictos.SavePicto(null, new Picto{
                            Text =category.Name, 
                            Image = await Base64Converter.FromStorageFile(await category.GetFileAsync(category.Name + ".png")),
                            Sound = ""});
                    var pictos = await category.GetFoldersAsync();
                    foreach (var picto in pictos)
                    {
                        string sound="";
                        try
                        {
                            sound = await Base64Converter.FromStorageFile(await picto.GetFileAsync(picto.Name + ".mp3"));
                        }
                        catch{}
                        await _pictos.SavePicto(categoryObject, new Picto{
                            Text = picto.Name, 
                            Image = await Base64Converter.FromStorageFile(await picto.GetFileAsync(picto.Name + ".png")), 
                            Sound = sound});
                    }
                    await category.DeleteAsync();
                }
                ApplicationData.Current.LocalSettings.Values.Remove("Initialized");
            }
        }
    }
}
