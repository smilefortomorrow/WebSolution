using KP.GmailClient.Authentication.TokenClients;
using KP.GmailClient.Authentication.TokenStores;
using KP.GmailClient.Models;
using KP.GmailClient;
using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Cms;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KP.GmailClient.Services;
using System.Text;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

//         public async Task<bool> Send(string target, string title, string body)
//         {
//             try {
//                 var tokenClient = await TokenClient.CreateAsync("oauth_client_credentials.json");
//                 var tokenStore = new FileTokenStore("token.json");
//                 using var client = new GmailClient(tokenClient, tokenStore);
// 
// 
//                 // Send a HTML email
//                 Message htmlMessage = await client.Messages.SendAsync(target, title, body, isBodyHtml: true);
//                 return true;
//             } catch (Exception ex) {
//                 return false;
//             }
//         }

        public async Task<bool> Send(string target, string title, string body, byte[] p)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Client", _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("TinaKing", target));
            emailMessage.Subject = title;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient()) {
                try {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(emailMessage);
                    return true;
                } catch (Exception ex){
                    //log an error message or throw an exception, or both.
                    return false;
                } finally {
                    client.Disconnect(true);
                    client.Dispose();
                }
                return false;
            }

        }

        public string string_body = "<html>\n<head>\n<title>Your request of reset password was accepted</title>\n</head>\n<body style =\"background-color: #EEDDFF\">\n<style type=\"text/css\">\n\tH1, H2, H3, H4{\n\t\tcolor:#604080;\n\t}\n\tH1, H2, H3, H4, label, a{\n\t\tmargin:20px;text-align: center;\n\t}\n</style>\n<H3 style=\"margin-top:100px;margin-left: 20px;text-align: center;\">Your request was accepted in TinaKing.com.</H3>\n<label class=\"ms-text\"> To reset the password, Please click on the link below : </label><br /><br />\n<a class=\"ms-text\" href=\"{token_url}\">{token_url}</a>\n<br /><br />\n<label>Your password will be reset to \"{password}\".</label>\n<br /><br />\n<H4>Login Using New Password</H4>\n\n</body>\n</html>";
        public async Task<bool> SendTokenMail(string target, string title, string token, string pwd)
        {
            string body = string_body.Replace("{token_url}", token);
            body = body.Replace("{password}", pwd);
            return await Send(target, title, body);
        }
    }
}
