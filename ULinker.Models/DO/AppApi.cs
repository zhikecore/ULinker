using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Models.DO
{
    public class AppApi : Base
    {
        public int AppId { get; set; }
        public int ApiId { get; set; }
        public bool IsUse { get; set; }
    }
}
