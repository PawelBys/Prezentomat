using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
    [Table("user_of_gathering", Schema = "public")]
    public class UserOfGatheringClass
    {
        [Key] // to wskazuje na to, która zmienna jest kluczem głównym
        public int user_of_gathering_id { get; set; }
        public int user_id { get; set; }
        public int gathering_id { get; set; }
        public DateTime joining_date { get; set; }

    }
}