using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace PictoUI.Model
{
    public interface IPictos
    {
        Task Initialize();
        Task<ICollection<Picto>> GetCategories();
        Task<Picto> SavePicto(Picto parent, Picto child);
        Task<Picto> GetCategory(string categoryName);
        Task DeleteCategory(Picto selectedCategory);
        Task DeletePicto(Picto selectedCategory, Picto selectedPicto);
        bool IsUniqueCategory(string categoryName, string categoryKey);
        bool IsUniquePicto(string categoryName, string pictoName, string pictoKey);
    }
}