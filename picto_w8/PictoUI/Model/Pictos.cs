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

        public async Task Initialize()
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
                AddPictoToList(
                    stmt.GetIntAt(5) > 0 ? _list.Single(p=>p.Id==stmt.GetIntAt(5)):null,
                    new Picto
                    {
                        Id=stmt.GetIntAt(0),
                        Key=stmt.GetTextAt(1),
                        Text=stmt.GetTextAt(2),
                        Image = stmt.GetTextAt(3),
                        Sound = stmt.GetTextAt(4),
                    });
            }
        }

        public async Task<Picto> SavePicto(Picto parent, Picto child)
        {
            if (_list == null) await Initialize();
            
            var idParent = parent == null ? (int?)null : parent.Id;
            if (child.Key == null)
            {
                child.Key = Guid.NewGuid().ToString();
                await InsertPicto(child, idParent);

                return AddPictoToList(parent, child);
            }
            else
            {
                await UpdatePicto(child, idParent);
                return UpdatePictoInList(parent, child);                       
            }
        }

        private async Task InsertPicto(Picto child, int? idParent)
        {
            var ins_name = await _db.PrepareStatementAsync(
                @"  insert into names(key, language, value) values(?,?,?);");
            ins_name.BindTextParameterAt(1, child.Key);
            ins_name.BindTextParameterAt(2, _culture);
            ins_name.BindTextParameterAt(3, child.Text);

            await ins_name.StepAsync().AsTask().ConfigureAwait(false);

            var ins_picto = await _db.PrepareStatementAsync(
                @"insert into pictos (key, image, id_parent, sound) VALUES(?, ?,?,?);");
            ins_picto.BindTextParameterAt(1, child.Key);
            ins_picto.BindTextParameterAt(2, child.Image);
            ins_picto.BindTextParameterAt(4, child.Sound);
            if (idParent != null)
            {
                ins_picto.BindIntParameterAt(3, idParent.Value);
            }
            else
            {
                ins_picto.BindNullParameterAt(3);
            }
            await ins_picto.StepAsync().AsTask().ConfigureAwait(false);
            child.Id = (int) _db.GetLastInsertedRowId();
        }

        private async Task UpdatePicto(Picto child, int? idParent)
        {
            var ins_name = await _db.PrepareStatementAsync(
                @"  update names SET value = ? WHERE language=? and key=?;");
            ins_name.BindTextParameterAt(3, child.Key);
            ins_name.BindTextParameterAt(2, _culture);
            ins_name.BindTextParameterAt(1, child.Text);

            await ins_name.StepAsync().AsTask().ConfigureAwait(false);

            var ins_picto = await _db.PrepareStatementAsync(
                @"update pictos set image = ?, sound = ? WHERE id_parent= ? and key = ?;");
            ins_picto.BindTextParameterAt(4, child.Key);
            ins_picto.BindTextParameterAt(1, child.Image);
            ins_picto.BindTextParameterAt(2, child.Sound);
            if (idParent != null)
            {
                ins_picto.BindIntParameterAt(3, idParent.Value);
            }
            else
            {
                ins_picto.BindNullParameterAt(3);
            }
            await ins_picto.StepAsync().AsTask().ConfigureAwait(false);
            child.Id = (int)_db.GetLastInsertedRowId();
        }

        private Picto AddPictoToList(Picto parent, Picto child)
        {
            if (parent != null)
            {
                parent.Children.Add(child);
            }
            else
            {
                child.Children=new ObservableCollection<Picto>();
                _list.Add(child);
            }

            return child;
        }

        private Picto UpdatePictoInList(Picto parent, Picto child)
        {
            if (parent != null)
            {
                parent.Children.Remove(parent.Children.Single(c=>c.Key==child.Key));
                parent.Children.Add(child);
            }
            else
            {
                _list.Remove(_list.Single(p => p.Key == child.Key));
                _list.Add(child);
            }

            return child;
        }
        private async Task<ObservableCollection<Picto>> GetList()
        {
            if (_list==null)
            {
                await Initialize();
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

        public bool IsUniqueCategory(string categoryName, string categoryKey)
        {
            if (string.IsNullOrEmpty(categoryName))
                return true;
            
            return GetList().Result.All(f => f.Text != categoryName || f.Key == categoryKey);
        }

        public bool IsUniquePicto(string categoryKey, string pictoName, string pictoKey)
        {
            if (string.IsNullOrEmpty(categoryKey) || string.IsNullOrEmpty(pictoName))
                return true;
            
            return GetList().Result.Single(f => f.Key == categoryKey).Children.All(p=>p.Text!=pictoName || p.Key==pictoKey);
        }
    }
}
