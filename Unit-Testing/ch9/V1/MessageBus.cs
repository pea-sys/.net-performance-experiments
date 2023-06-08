
namespace ch9.V1
{
    public class MessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public void SendEmailChangedMessage(int userId, string newEmail)
        {
            _bus.Send("Type: USER EMAIL CHANGED; " +
                $"Id: {userId}; " +
                $"NewEmail: {newEmail}");
        }
    }
}
