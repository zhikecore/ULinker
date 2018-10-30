using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULinker.WebApi.Models
{
    public class Token
    {
        /// <summary>
        /// 用户名
        /// </summary>
        //public int StaffId { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 用户名对应签名Token
        /// </summary>
        public string SignToken { get; set; }


        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}