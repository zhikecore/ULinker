using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Models.DO
{
    public class UserPlatform : Base
    {
        public int CreatorId { get; set; }//创建人ID
        public int PlatformId { get; set; }//所属平台Id
        public int AppId { get; set; }//应用Id
        public string AppKey { get; set; }
        public string AppSecrect { get; set; }
        public string Token { get; set; }
        public bool IsUse { get; set; }
        public DateTime TokenExpireTime { get; set; }//Token过期时间
    }
}
