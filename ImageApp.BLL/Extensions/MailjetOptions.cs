using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Extensions
{
    public class MailjetOptions
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
