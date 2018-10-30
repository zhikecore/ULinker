using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;

namespace ULinker.Models.VO
{
    public class UserPlatformViewModel:UserPlatform
    {
        public string Developer { get; set; }
        public string Platform { get; set; }
        public string App { get; set; }
    }
}
