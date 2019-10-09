using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HighSchoolManagerAPI.Helpers
{
    public class ResponeHelper
    {
        public int status { get; set; }
        public List<object> errors { get; set; }

        public ResponeHelper()
        {
            status = 200; // 200: ok
            errors = new List<object>();
        }
    }
}