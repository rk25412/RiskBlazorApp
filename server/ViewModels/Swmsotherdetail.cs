using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.ViewModels
{
    public class Swmsotherdetail
    {
       public IEnumerable<int> plants { get; set; }
        public IEnumerable<int> ppes { get; set; }
        public IEnumerable<int> licenes { get; set; }
        public IEnumerable<int> legislations { get; set; }
        public IEnumerable<int> materialsHazardous { get; set; }
    }
}
