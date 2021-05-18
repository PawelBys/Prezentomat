using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prezentomat.Models
{
    public class AddUserOfGathering
    {
        public List<UserClass> added { get; set; }
        public List<UserClass> noadded { get; set; }
    }
}