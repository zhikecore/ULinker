using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Models.DO
{
    public class Api:Base
    {
        public int PlatformId { get; set; }
        public string FunctionName { get; set; }
        public string Url { get; set; }
    }
}
