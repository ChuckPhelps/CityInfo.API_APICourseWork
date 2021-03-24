namespace CityInfo.API.Services
{
    public interface IMailService //Interface. 
    {
        void Send(string subject, string message);

    }
}