
namespace ch8.Listing2
{
    public interface IMessageBus
    {
        void SendEmailChangedMessage(int userId, string newEmail);
    }
}
