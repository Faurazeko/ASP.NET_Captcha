using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP_Captcha.Models
{
    public class Captcha
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        [DataType("datetime2")]
        public DateTime ExpirationTime { get; set; }

        public Captcha(string content, string path)
        {
            Content = content;
            Path = path;
            ExpirationTime = DateTime.Now.AddMinutes(3);
        }

        public Captcha(string content, string path, DateTime expirationTime)
        {
            Content = content;
            Path = path;
            ExpirationTime = expirationTime;
        }
    }
}
