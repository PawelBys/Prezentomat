using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{

    [Table("gathering", Schema = "public")]
    public class GatheringClass
    {
        [Key] // to wskazuje na to, która zmienna jest kluczem głównym
        public int gathering_id { get; set; }
        public int current_amount { get; set; }
        public int target_amount { get; set; }
        public DateTime finish_date { get; set; }
    }
}