using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneToMany.Models
{
    public class SubCatagory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int cat_id { get; set; }
    }
}
