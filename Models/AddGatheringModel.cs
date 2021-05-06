using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prezentomat.Models
{
    public class AddGatheringModel
    {
        // specjalny model stworzony do tworzenia zbiorek (do metody moze byc przekazany tylko jeden model, on sie potem dzieli na dwa)
        [Key]
        public int user_of_gathering_id { get; set; }
        [Required]
        public int user_id { get; set; }
        [Required]
        public DateTime joining_date { get; set; }
        [Required]
        public int current_amount { get; set; }
        [Required]
        public int target_amount { get; set; }
        [Required]
        public DateTime finish_date { get; set; }
        [Required]
        public string gathering_name { get; set; }
        [Required]
        public int creator_id { get; set; }

    }
}