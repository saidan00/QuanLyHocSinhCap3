using System;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Student : IAggregateRoot
    {
        public int StudentID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public DateTime EnrollDate { get; set; }

        public int? ClassID { get; set; }

        public virtual Class Class { get; set; }
        // public virtual List<Conduct> Conducts { get; set; }
        // public virtual List<Result> Results { get; set; }

        public Student()
        {
            EnrollDate = DateTime.Today;
        }
    }
}
