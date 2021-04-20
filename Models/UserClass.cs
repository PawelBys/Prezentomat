
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
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthdate { get; set; }
        public char gender { get; set; }
        public int wallet { get; set; }

        public string hashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            Console.WriteLine(Convert.ToBase64String(encrypted_bytes));
            return Convert.ToBase64String(encrypted_bytes);
        }


    }
}