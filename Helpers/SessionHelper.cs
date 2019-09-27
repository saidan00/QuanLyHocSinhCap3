using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHocSinhCap3.Helpers
{
    public class SessionHelper
    {
        public bool IsLoggedIn()
        {
            if (HttpContext.Current.Session["userId"] == null)
            {
                return false;
            }
            return true;
        }
    }
}