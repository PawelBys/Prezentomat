using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
    [Table("payment_history", Schema = "public")]
    public class PaymentHistoryClass
    {
        [Key] // to wskazuje na to, która zmienna jest kluczem głównym
        public int payment_history_id { get; set; }
        [Required]
        public int user_of_gathering_id { get; set; }
        [Required]
        public DateTime payment_date { get; set; }
        [Required]
        public int amount_of_payment { get; set; }
    }
}