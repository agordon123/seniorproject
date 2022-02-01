using PanOpticon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class PriorityCss : Attribute
    {
        public string CustomBadgeCSS { get; set; }

 
        public PriorityCss(string c)
        {
            CustomBadgeCSS = c;
           
        }
    }


    public static class PriorityCssHelper
    {
        public static PriorityCss GetDisplayName(this TaskPriority taskPriority)
        {
            var type = taskPriority.GetType();
            var name = Enum.GetName(type, taskPriority);
            var attrib = type.GetField(name).GetCustomAttributes(false).OfType<PriorityCss>().FirstOrDefault();

            return attrib ?? new PriorityCss("");

        }


    }
}
