using Microsoft.AspNetCore.Identity;
using PanOpticon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.UserRoles
{
    public class PanopticonUser : IdentityUser
    {
        public CellProvider CellProvider { get; set; } = CellProvider.None;
    }
}
