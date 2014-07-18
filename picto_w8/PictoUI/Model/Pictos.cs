using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using PictoTest;
using SQLite;

namespace PictoUI.Model
{
    public class Pictos : IPictos
    {
        private ObservableCollection<Picto> _list;
        private SQLiteAsyncConnection _db;
        private string _culture;

      

        public Pictos()
        {
            _db = new SQLiteAsyncConnection("pictos.db");
            _list = new ObservableCollection<Picto>();
            _culture = "en";
        }

        private async Task<ObservableCollection<Picto>> GetList()
        {
            if (_list.Count == 0)
            {
                var categories =
                    await
                        _db.QueryAsync<Picto>(
                            @"select pictos.id as Id, pictos.key as Key, names.value as Text, pictos.image as Image, pictos.sound as Sound  from pictos
                            inner join names on names.key like pictos.key and names.language=? where pictos.id_parent is null",
                            _culture);

                foreach (var category in categories)
                {
                    var pictos = (await
                        _db.QueryAsync<Picto>(@"select pictos.id as Id, pictos.key as Key, names.value as Text, pictos.image as Image, pictos.sound as Sound  from pictos
                                inner join names on names.key like pictos.key and names.language=?
                                where pictos.id_parent=?", _culture, category.Id))
                        .Select(p => new Picto(p.Id,p.Key, p.Text, p.Image, p.Sound));
                    _list.Add(new Picto(category.Id, category.Key, category.Text, category.Image,
                        category.Sound, new ObservableCollection<Picto>(pictos)));
                }
            }
            return _list;
        }

        public async Task<ICollection<Picto>> GetCategories()
        {
            return await GetList();
        }

        public async Task<Picto> GetCategory(string categoryKey)
        {
            return (await GetList()).Single(c => c.Key == categoryKey);
        }

        public async Task DeleteCategory(Picto selectedCategory)
        {
            await _db.ExecuteScalarAsync<int>("DELETE FROM PICTOS WHERE id_parent=? or id=?;", selectedCategory.Id, selectedCategory.Id);
            (await GetList()).Remove(selectedCategory);
        }

        public async Task DeletePicto(Picto selectedCategory, Picto selectedPicto)
        {
            await _db.ExecuteScalarAsync<int>("DELETE FROM PICTOS WHERE id_parent=? and id=?;", selectedCategory.Id, selectedPicto.Id);
            (await GetCategory(selectedCategory.Key)).Children.Remove(selectedPicto);
        }

        public bool IsUnique(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return true;
            
            return GetList().Result.All(f => f.Text != categoryName);
        }

        public bool IsUnique(string categoryKey, string pictoName)
        {
            if (string.IsNullOrEmpty(categoryKey) || string.IsNullOrEmpty(pictoName))
                return true;
            
            return GetList().Result.Single(f => f.Key == categoryKey).Children.All(p=>p.Text!=pictoName);
        }

        public async Task<Picto> SavePicto(Picto parent, string text, StorageFile image, StorageFile sound)
        {
            var list=await GetList();
            var idParent = parent==null? (int?) null:parent.Id;
            var key = Guid.NewGuid().ToString();
            var image64 = await StorageFileConverter.ConvertToBase64(image);
            var sound64 = await StorageFileConverter.ConvertToBase64(sound);

            await _db.ExecuteScalarAsync<int>(
                        @"  insert into names(key, language, value) values(?,?,?);", key, _culture, text);
            await _db.ExecuteScalarAsync<int>(
                            @"insert into pictos (key, image, id_parent, sound) VALUES(?, ?,?,?);"
                            ,key, image64, idParent, string.IsNullOrEmpty(sound64)?null:sound64);
            var id =await _db.ExecuteScalarAsync<int>(@"select last_insert_rowid();");


            Picto newPicto = null;
            if (parent == null)
            {
                newPicto = new Picto(id, key, text, image64, sound64, new ObservableCollection<Picto>());
                (await GetList()).Add(newPicto);
            }
            else
            {
                newPicto = new Picto(id, key, text, image64, sound64);
                (await GetCategories()).Single(c=>c.Key==parent.Key).Children.Add(newPicto);
            }

            return newPicto;
        }
    }
}
