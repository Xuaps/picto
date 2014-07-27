using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace PictoUI.Model
{
    public interface IPictos
    {
        Task Initialize();
        Task<ICollection<Picto>> GetCategories();
        Task<Picto> SavePicto(Picto parent, string text, StorageFile image, StorageFile sound);
        Task<Picto> GetCategory(string categoryName);
        Task DeleteCategory(Picto selectedCategory);
        Task DeletePicto(Picto selectedCategory, Picto selectedPicto);
        bool IsUnique(string categoryName);
        bool IsUnique(string categoryName, string pictoName);
    }
}