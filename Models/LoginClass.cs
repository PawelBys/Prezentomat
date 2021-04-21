using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Prezentomat.Models
{
    [Table("user", Schema = "public")]
    public class LoginClass
    {
            [Key]
            public int user_id { get; set; }

            [Required]
            [EmailAddress]
            public string email { get; set; }

            [Required]
            [MinLength(6)]
            public string password { get; set; }

    }
}