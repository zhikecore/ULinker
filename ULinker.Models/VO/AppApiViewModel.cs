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
    public class AppApiViewModel:AppApi
    {
        public string AppName { get; set; }

        public string ApiName { get; set; }
    }
}
