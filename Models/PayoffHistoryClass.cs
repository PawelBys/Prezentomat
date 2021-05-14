using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
    [Table("payoff_history", Schema = "public")]
    public class PayoffHistoryClass
    {
        [Key] // to wskazuje na to, która zmienna jest kluczem głównym
        public int id_payoff_history { get; set; }
        [Required]
        public int id_user_of_gathering { get; set; }
        [Required]
        public DateTime payoff_date { get; set; }
        [Required]
        public int amount_of_payment { get; set; }
    }
}