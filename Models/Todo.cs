using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<KanboardNote> KanboardNotes { get; set; }
        public PanopticonUser PanopticonUser { get; set; }

    }
}
