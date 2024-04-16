using System.Net.Mail;

namespace USIMentorshipWebApp.Data
{
    public class EmailService
    {
        private string Message { get; set; } = "";

        public async Task SendMail(string emailAddress, string subject, string messageContent)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("bigb60690@gmail.com"); //from address
                    mail.To.Add(emailAddress); //where its going to go to
                    mail.Subject = subject; //subject line
                    mail.Body = messageContent; // body of mail
                    mail.IsBodyHtml = true; // can turn on and off

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) // this is the smtp of the mail address along with the port
                                                                                    //(port is the same for gmail and outlook I believe)
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("bigb60690@gmail.com", "kzmb dsgq aidx szfp");  //from address and the 2 factor code for using another app
                                                                                                                            // pw for the email is rainman1234
                        smtp.EnableSsl = true; // enabling of Ssl
                        await smtp.SendMailAsync(mail); // sending in smtp asynchronously
                        Message = "Mail Sent"; // button text
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message; // catching of error message
            }
        }
}
}
