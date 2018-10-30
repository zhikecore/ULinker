using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Models.DO
{
    public class App : Base
    {
        public int AppTypeId { get; set; }//应用程序类型 
        public int CreatorId { get; set; }
        public int ManagerId { get; set; }//负责人
    }
}
