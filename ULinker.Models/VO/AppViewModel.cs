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
    public class AppViewModel:App
    {
        public string Manager { get; set; }
        public string Creator { get; set; }
    }
}
