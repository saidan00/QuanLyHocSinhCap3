using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHocSinhCap3.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleType { get; set; }
        public int Test { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}