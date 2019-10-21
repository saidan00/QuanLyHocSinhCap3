using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HighSchoolManagerAPI.FrontEndModels
{
    public class TeacherModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}