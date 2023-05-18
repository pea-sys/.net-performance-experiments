
namespace ch5.Listing9
{
    public interface IEmailGateway
    {
        void SendReceipt(string email, string productName, int quantity);
    }
}
