using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using PictoUI.Common;
using SQLiteWinRT;

namespace PictoUI.Model
{
    public class Pictos : IPictos
    {
        private ObservableCollection<Picto> _list;
        private string _culture;
        private Database _db;

        public Pictos()
        {
            _db = new Database(ApplicationData.Current.LocalFolder, "pictos.db");
            _culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        public async Task<ObservableCollection<Picto>> GetList()
        {
            if (_list==null)
            {
                await _db.OpenAsync();

                _list = new ObservableCollection<Picto>();
                var stmt = await _db.PrepareStatementAsync(
                            @"select pictos.id as Id, pictos.key as Key, names.value as Text, pictos.image as Image, 
                            pictos.sound as Sound, pictos.id_parent as Parent  from pictos
                            left outer join names on names.key = pictos.key and names.language=?
                            order by pictos.id_parent");
                stmt.BindTextParameterAt(1, _culture);

                while (await stmt.StepAsync().AsTask().ConfigureAwait(false))
                {
                    var picto = new Picto(
                        stmt.GetIntAt(0),
                        stmt.GetTextAt(1),
                        stmt.GetTextAt(2),
                        stmt.GetTextAt(3),
                        stmt.GetTextAt(4),
                        stmt.GetIntAt(5)>0?null:new ObservableCollection<Picto>()
                    );

                    if (picto.Children == null)
                    {
                        _list.Single(p => p.Id == stmt.GetIntAt(5)).Children.Add(picto);
                    }
                    else
                    {
                        _list.Add(picto);
                    }
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
            (await GetList()).Remove(selectedCategory);
            
            var stm = await _db.PrepareStatementAsync("DELETE FROM PICTOS WHERE id_parent=:id or id=:id;");
            stm.BindIntParameterWithName(":id", selectedCategory.Id);
            await stm.StepAsync();
        }

        public async Task DeletePicto(Picto selectedCategory, Picto selectedPicto)
        {
            (await GetCategory(selectedCategory.Key)).Children.Remove(selectedPicto);

            var stm = await _db.PrepareStatementAsync("DELETE FROM PICTOS WHERE id_parent=? and id=?;");
            stm.BindIntParameterAt(1, selectedCategory.Id);
            stm.BindIntParameterAt(2, selectedPicto.Id);
            await stm.StepAsync();
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
            var image64 = await Base64Converter.FromStorageFile(image);
            var sound64 = await Base64Converter.FromStorageFile(sound);

            var ins_name=await _db.PrepareStatementAsync(
                        @"  insert into names(key, language, value) values(?,?,?);");
            ins_name.BindTextParameterAt(1, key);
            ins_name.BindTextParameterAt(2, _culture);
            ins_name.BindTextParameterAt(3, text);

            await ins_name.StepAsync().AsTask().ConfigureAwait(false);

            var ins_picto = await _db.PrepareStatementAsync(
                            @"insert into pictos (key, image, id_parent, sound) VALUES(?, ?,?,?);");
            ins_picto.BindTextParameterAt(1, key);
            ins_picto.BindTextParameterAt(2,image64);
            if (idParent != null)
            {
                ins_picto.BindIntParameterAt(3, idParent.Value);
            }
            else
            {
                ins_picto.BindNullParameterAt(3);
            }
            await ins_picto.StepAsync().AsTask().ConfigureAwait(false);
            var id =(int)_db.GetLastInsertedRowId();


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
