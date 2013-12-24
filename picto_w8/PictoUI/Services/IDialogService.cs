using System.Threading.Tasks;

namespace PictoUI.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message);
        Task ShowResourceMessageAsync(string key);
    }
}
