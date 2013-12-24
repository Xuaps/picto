using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PictoUI.Model
{
    public class Pictos : IPictos
    {
        private ObservableCollection<Picto> list;

        public async Task<ICollection<Picto>> GetCategories()
        {
            if (list != null)
                return list;
                
            list = new ObservableCollection<Picto>();       
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            foreach (var folder in folders)
            {
                var children = await GetPictos(folder);
                lock (list)
                {
                    list.Add(new Picto(children, folder.Name, "", folder.Path + "\\" + folder.Name + ".jpg"));                            
                }
            }
            return list;
        }

        private async Task<ICollection<Picto>> GetPictos(StorageFolder folder)
        {
            var subfolders = await folder.GetFoldersAsync();
            return new ObservableCollection<Picto>(subfolders.Select(
                    subfolder =>
                    new Picto(null, subfolder.Name, subfolder.Path + "\\" + subfolder.Name + ".mp3",
                              subfolder.Path + "\\" + subfolder.Name + ".jpg")));
        }

        public async Task<Picto> GetCategory(string categoryName)
        {
            var categories = await GetCategories();
            return categories.Single(c => c.Text == categoryName);
        }

        public async Task DeleteCategory(Picto selectedCategory)
        {
            var folder=await ApplicationData.Current.LocalFolder.GetFolderAsync(selectedCategory.Text);
            await folder.DeleteAsync();

            list.Remove(list.Single(c => c.Text == selectedCategory.Text));
        }

        public async Task DeletePicto(Picto selectedCategory, Picto selectedPicto)
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(selectedCategory.Text);
            var subfolder = await folder.GetFolderAsync(selectedPicto.Text);
            await subfolder.DeleteAsync();

            var sublist=list.Single(c => c.Text == selectedCategory.Text).Children;
            sublist.Remove(sublist.Single(p => p.Text == selectedPicto.Text));
        }

        public bool IsUnique(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return true;
            
            return list.All(f => f.Text != categoryName);
        }

        public bool IsUnique(string categoryName, string pictoName)
        {
            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(pictoName))
                return true;
            
            return list.Single(f => f.Text != categoryName).Children.All(p=>p.Text!=pictoName);
        }

        public async Task<Picto> SavePicto(Picto parent, string text, StorageFile image, StorageFile sound)
        {
            StorageFolder folder;
            string sndPath="";
            ObservableCollection<Picto> children = null;
            if(parent==null)
            {
                folder=await ApplicationData.Current.LocalFolder.CreateFolderAsync(text);
                children=new ObservableCollection<Picto>();
            }else
            {
                var parentFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(parent.Text);
                folder = await parentFolder.CreateFolderAsync(text);
            }

            var img = await image.CopyAsync(folder, text + ".jpg");
            if (sound != null)
            {
                var file = await sound.CopyAsync(folder, text + ".mp3");
                sndPath = file.Path;
            }

            var picto=new Picto(children, text, sndPath, img.Path);
            if(parent==null)
                list.Insert(0,picto);
            else
            {
                var p = list.Single(c => c.Text == parent.Text);                
                p.Children.Add(picto);
            }
            return picto;
        }
    }
}
