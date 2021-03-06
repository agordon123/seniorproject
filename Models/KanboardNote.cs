using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public class KanboardNote
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Note { get; set; }
        public string Title { get; set; }
        public PanopticonUser PanopticonUser { get; set; }

        public Todo Todo {get;set;}

    }
}
