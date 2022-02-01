using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PanOpticon.Models
{
    public enum CellProvider
    {
        [Display(Name="None")]
        None = 1, 
        Tmobile,
        Verizon,
        Sprint
    }


}
