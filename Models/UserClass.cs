using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
        [Table("user", Schema = "public")]
    public class UserClass
    {
        [Key]
        public int userID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string  lastname { get; set; }
        public DateTime birthdate { get; set; }
        public char gender { get; set; }
        public int wallet { get; set; }
    }
}