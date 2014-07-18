using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using PictoTest;
using PictoUI.Model;

namespace PictoUI.Services
{
    public class InitialDataService
    {
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

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized"))
            {
                var categories = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
                var pictosCollection = new Pictos();

                foreach (var category in categories)
                {
                    var categoryObject=await pictosCollection.SavePicto(null, category.Name, await category.GetFileAsync(category.Name + ".png"), null);
                    var pictos = await category.GetFoldersAsync();
                    foreach (var picto in pictos)
                    {
                        StorageFile sound=null;
                        try
                        {
                            sound = await picto.GetFileAsync(picto.Name + ".mp3");
                        }
                        catch{}
                        await pictosCollection.SavePicto(categoryObject, picto.Name, await picto.GetFileAsync(picto.Name + ".png"), sound);
                    }
                    await category.DeleteAsync();
                }
                ApplicationData.Current.LocalSettings.Values.Remove("Initialized");
            }
        }
    }
}
