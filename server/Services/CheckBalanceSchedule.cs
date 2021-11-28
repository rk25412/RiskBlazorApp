using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Clear.Risk.Data;
using Clear.Risk.Models.ClearConnection;
using Clear.Risk.Models;
////using Coravel.Invocable;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using Novacode;
using System.Drawing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Net;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;

namespace Clear.Risk
{
    //public class CheckBalanceSchedule : IInvocable
    public class CheckBalanceSchedule
    {
        private readonly ClearConnectionService clearService;
        public CheckBalanceSchedule(ClearConnectionService connection)
        {
            clearService = connection;
        }

        public async Task Invoke()
        {
            try
            {
                var persons = await clearService.GetPeople(new Query() { Filter = $@"i => i.COMPANYTYPE == 2" });

                foreach (var item in persons)
                {
                    //if (item.CURRENT_BALANCE < item.Applicence.DEFAULT_CREDIT)

                    if (item.CURRENT_BALANCE < item.Applicence.MIN_BALANCE)
                    {
                        await sendEmail(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task sendEmail(Person person)
        {
            Smtpsetup smtpsetup = await clearService.GetSmtpsetup();
            if (smtpsetup != null)
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                mimeMessage.To.Add(new MailboxAddress(person.BUSINESS_EMAIL));

                mimeMessage.Subject = "Important!  Clear System - Payment now due.";
                string emailbody = await EmailBody(person);
                mimeMessage.Body = new TextPart("Html")
                {
                    Text = emailbody
                };
                string SmtpServer = smtpsetup.SMTP_SERVER_STRING;
                int SmtpPortNumber = smtpsetup.SMTP_PORT;
                bool useSsl = smtpsetup.USE_SSL;
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, useSsl);
                        client.Authenticate(smtpsetup.SMTP_USER_ACCOUNT, smtpsetup.SMTP_ACCOUNT_PASSWORD);
                        await client.SendAsync(mimeMessage);
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task<string> EmailBody(Person person)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"<html>");
            builder.Append($@"<body>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Transaction usage of the Clear Work & Health Risk Assessment system has now reached a minimum use threshold. You only have a few transactions left before the system will not allow you to create or send any more Risk Assessments.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"You can add funds to your accounts quickly and easily and at any amount by using the payment services from within the system. How to do this is described in the &#8220;How To&#8221; section of the system - &#8220;Adding additional funds&#8221;.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Should your account balance reach £0.00 then you will no longer be able to send any transactions.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"If you have any questions then please contact support@Clear-whs.com or check the &#8220;How To&#8221; section of the system.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"We hope that you will continue to use the system and enjoy the benefits and security that the system brings.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Sincerely,");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Support@Clear-whs.com");
            builder.Append($@"</p>");
            builder.Append($@"</body>");
            builder.Append($@"</html>");
            return builder.ToString();
        }
    }
}
