using System;
using System.Threading.Tasks;
using ClearCovid.Data;
using ClearCovid.Models.ClearConnection;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Mvc;

namespace ClearCovid
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly ClearConnectionService context;

        public AuthMessageSender(ClearConnectionService Context)
        {
            context = Context;
        }
        public async Task SendEmailAsync(string email, string subject, string message, bool isHtml = false)
        {
            try
            {
                var smtp = await context.GetSmtpsetupBySmtpSetupId(1);
                var emailmessage = new MimeMessage();
                if (client != null && !string.IsNullOrEmpty(client.SMTPHOST) && client.SMTPPORT != null && !string.IsNullOrEmpty(client.REQUESTEMAIL)
                    && !string.IsNullOrEmpty(client.REQUESTUSER) && !string.IsNullOrEmpty(client.REQUESTUSERPW))
                {

                    emailmessage.From.Add(new MailboxAddress(client.REQUESTEMAIL));
                    emailmessage.To.Add(new MailboxAddress(email));
                    emailmessage.Subject = subject;


                    var textFormat = isHtml ? TextFormat.Html : TextFormat.Plain;
                    emailmessage.Body = new TextPart(textFormat)
                    {
                        Text = message
                    };

                    using (var sclient = new SmtpClient())
                    {
                        // Accept all SSL certificates (in case the server supports STARTTLS)
                        sclient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        await sclient.ConnectAsync(client.SMTPHOST, (int)client.SMTPPORT, false);

                        // Note: since we don't have an OAuth2 token, disable
                        // the XOAUTH2 authentication mechanism.
                        sclient.AuthenticationMechanisms.Remove("XOAUTH2");

                        // Note: only needed if the SMTP server requires authentication
                        await sclient.AuthenticateAsync(client.REQUESTUSER, client.REQUESTUSERPW);

                        await sclient.SendAsync(emailmessage);
                        await sclient.DisconnectAsync(true);
                    }
                }
            }
            catch(Exception ex)
            {

            }
           
            // Plug in your email service here to send an email.
            
        }

        public async Task SendEmailAsync(string email, string subject, string message,int ClientID, bool isHtml = false)
        {
            try
            {
                var client = await _clientService.GetClientById(ClientID);
                var emailmessage = new MimeMessage();
                if (client != null && !string.IsNullOrEmpty(client.SMTPHOST) && client.SMTPPORT != null && !string.IsNullOrEmpty(client.REQUESTEMAIL)
                    && !string.IsNullOrEmpty(client.REQUESTUSER) && !string.IsNullOrEmpty(client.REQUESTUSERPW))
                {

                    emailmessage.From.Add(new MailboxAddress(client.REQUESTEMAIL));
                    emailmessage.To.Add(new MailboxAddress(email));
                    emailmessage.Subject = subject;


                    var textFormat = isHtml ? TextFormat.Html : TextFormat.Plain;
                    emailmessage.Body = new TextPart(textFormat)
                    {
                        Text = message
                    };

                    using (var sclient = new SmtpClient())
                    {
                        // Accept all SSL certificates (in case the server supports STARTTLS)
                        sclient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        await sclient.ConnectAsync(client.SMTPHOST, (int)client.SMTPPORT, false);

                        // Note: since we don't have an OAuth2 token, disable
                        // the XOAUTH2 authentication mechanism.
                        sclient.AuthenticationMechanisms.Remove("XOAUTH2");

                        // Note: only needed if the SMTP server requires authentication
                        await sclient.AuthenticateAsync(client.REQUESTUSER, client.REQUESTUSERPW);

                        await sclient.SendAsync(emailmessage);
                        await sclient.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            // Plug in your email service here to send an email.

        }
        public async Task SendEmailAsyncWithAttachment(string email, string subject, string message, bool isHtml = false, byte[] UploadFiles=null, int userid=0, string FileName=null)
        {
            try
            {
                var client = await _clientService.GetClientById(userid);
                var emailmessage = new MimeMessage();
                if (client != null && !string.IsNullOrEmpty(client.SMTPHOST) && client.SMTPPORT != null && !string.IsNullOrEmpty(client.REQUESTEMAIL)
                    && !string.IsNullOrEmpty(client.REQUESTUSER) && !string.IsNullOrEmpty(client.REQUESTUSERPW))
                {

                    emailmessage.From.Add(new MailboxAddress(client.REQUESTEMAIL.Trim(), client.REQUESTEMAIL.Trim()));
                    emailmessage.To.Add(new MailboxAddress(email, email));
                    //emailmessage.From
                    emailmessage.Subject = subject;


                    var textFormat = isHtml ? TextFormat.Html : TextFormat.Plain;
                    emailmessage.Body = new TextPart(textFormat)
                    {
                        Text = message
                    };
                    var bodyBuilder = new BodyBuilder();
                    String str = FileName;
                    FileName = str.Replace('/', '-');
                    if (UploadFiles != null)
                        bodyBuilder.Attachments.Add(FileName, UploadFiles);
                    bodyBuilder.HtmlBody = message;
                    emailmessage.Body = bodyBuilder.ToMessageBody();
                    using (var sclient = new SmtpClient())
                    {
                        // Accept all SSL certificates (in case the server supports STARTTLS)
                        sclient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        await sclient.ConnectAsync(client.SMTPHOST.Trim(), (int)client.SMTPPORT, false);

                        // Note: since we don't have an OAuth2 token, disable
                        // the XOAUTH2 authentication mechanism.
                        sclient.AuthenticationMechanisms.Remove("XOAUTH2");

                        // Note: only needed if the SMTP server requires authentication
                       await sclient.AuthenticateAsync(client.REQUESTUSER.Trim(), client.REQUESTUSERPW.Trim());

                        await sclient.SendAsync(emailmessage);
                        await sclient.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            // Plug in your email service here to send an email.

        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            String sURL = "";
            System.IO.StreamReader objReader;
            //sURL = @"http://sms.servein.in/api/sendhttp.php?authkey=203700AXzvOkcr5aacfa28&mobiles=+91" + number + "&message=" + message + "&sender=PMRAPP&route=6";
            sURL = @"https://control.msg91.com/api/sendhttp.php?authkey=203700AXzvOkcr5aacfa28&mobiles=+91" + number + "&message=" + message + "&sender=PMRAPP&route=6";
            System.Net.WebRequest wrGETURL;
            wrGETURL = System.Net.WebRequest.Create(sURL);
            try
            {
                System.IO.Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
                objReader = new System.IO.StreamReader(objStream);
                objReader.Close();
                return Task.FromResult(1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Task.FromResult(0);
        }


        public Task SendSmsEkosAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            String sURL = "";
            System.IO.StreamReader objReader;
            //sURL = @"http://sms.servein.in/api/sendhttp.php?authkey=203700AXzvOkcr5aacfa28&mobiles=+91" + number + "&message=" + message + "&sender=PMRAPP&route=6";
            sURL = @"http://sms.messageindia.in/sendSMS?username=ekos&message=" + message + "&sendername=EKOSIN" + "&smstype=TRANS&numbers=" + number + "&apikey=ff98c5e1-6db8-429d-b6c5-d1e8eb6b7d14";
            System.Net.WebRequest wrGETURL;
            wrGETURL = System.Net.WebRequest.Create(sURL);
            try
            {
                System.IO.Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
                objReader = new System.IO.StreamReader(objStream);
                objReader.Close();
                return Task.FromResult(1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Task.FromResult(0);
        }

        public string OTP()
        {
            string numbers = "1234567890";
            string characters = numbers;
            characters += numbers;
            string otp = string.Empty;
            int length = 6;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
    }
}
