using System.Threading.Tasks;

namespace PictoUI.Messaging
{
    public interface IHub
    {
        Task Send<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
