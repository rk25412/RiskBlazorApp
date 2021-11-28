using System.Threading.Tasks;

namespace ClearCovid
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
        Task SendSmsEkosAsync(string number, string message);
    }
}
