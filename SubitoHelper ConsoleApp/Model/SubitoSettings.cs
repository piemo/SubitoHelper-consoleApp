using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubitoHelper_ConsoleApp.Model
{
    class SubitoSettings
    {
        public string SqlConnectionString {get;set;}
        public string username { get; set; }
        public string password { get; set; }
        public string chatToken { get; set; }
        public string botToken { get; set; }
        public string idPastebin { get; set; }

    }
}
