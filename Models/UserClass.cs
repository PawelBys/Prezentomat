
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
    [Table("user", Schema = "public")]
    public class UserClass
    {
        [Key]
        public int user_id { get; set; }

        [Required] 
        [EmailAddress]
        public string email { get; set; }
        
        [Required] 
        [MinLength(6)] 
        public string password { get; set; }

        public string firstname { get; set; }

        [Required] 
        [MaxLength(15)]
        public string lastname { get; set; }

        [Required]
        public string birthdate { get; set; }
        public char gender { get; set; }
        public int wallet { get; set; }

        [Required]
        public string repeat_password { get; set; }
    }
}