using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExoWebApi.Web.Models.User
{
    public class AutentificationModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ConfidentialsModel
    {
        [Required]
        public string Email { get; set; }
    }
}