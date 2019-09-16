using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POSMVCWebAPIClient.Models
{
    public class Associate
    {
        [Display(Name = "Associate ID")]
        [Required(ErrorMessage = "please enter Associate Id")]
        public int AssociateId { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "please enter password")]
        public string AssociatePwd { get; set; }

        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
    }
}