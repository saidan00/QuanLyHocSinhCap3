using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class AccountRole
    {
        public int AccountRoleID { get; set; }
        public string RoleType { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}