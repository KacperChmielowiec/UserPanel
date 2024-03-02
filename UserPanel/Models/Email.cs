using MimeKit;
namespace UserPanel.Models
{
    public class Email
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Email(IEnumerable<string> to, string subject, string body) { 
            
            var t = new List<MailboxAddress>();
            t.AddRange(to.Select(item => new MailboxAddress("email",item)));
            this.To = t;

            this.Subject = subject;
            this.Body = body;
        }



    }
}
