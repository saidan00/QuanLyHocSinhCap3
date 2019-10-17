using System;
using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class ViolationRules
    {
        public int ViolationRuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public virtual List<Violation> Violation { get; set; }
    }
}