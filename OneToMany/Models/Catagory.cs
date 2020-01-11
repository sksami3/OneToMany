using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneToMany.Models
{
    public class Catagory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCatagory> SubCatagories { get; set; }
    }
}
