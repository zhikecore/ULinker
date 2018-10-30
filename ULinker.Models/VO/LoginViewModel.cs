using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ULinker.Models.VO
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("账号")]
        public string Account { get; set; }

        [Required]
        [DisplayName("手机验证码")]
        public string PhoneValideCode { get; set; }
    }
}
