using System.Threading.Tasks;

namespace ClearCovid
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false);
        Task SendEmailAsync(string email, string subject, string message,int ClientID, bool isHtml = false);
        Task SendEmailAsyncWithAttachment(string email, string subject, string message, bool isHtml = false,byte[] UploadFiles=null,int userid=0,string FileName=null);
    }
}
