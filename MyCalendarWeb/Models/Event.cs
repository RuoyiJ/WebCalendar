using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MyCalendarWeb.Areas.Identity.Data;

namespace MyCalendarWeb.Models
{
    public class Event
    {
        public int EventID { get; set; }

        [StringLength(50)]
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UserID { get; set; }
        public virtual MyCalendarUser User { get; set; }

    }
}
