using System.Data;
using System.Linq;
using System.Web.Mvc;
using QuanLyHocSinhCap3.DAL;
using QuanLyHocSinhCap3.Models;
using CryptoHelper;
using QuanLyHocSinhCap3.Helpers;

namespace QuanLyHocSinhCap3.Controllers
{
    public class AccountController : Controller
    {
        private HighSchoolContext db = new HighSchoolContext();
        private SessionHelper sessionHelper = new SessionHelper();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            if (sessionHelper.IsLoggedIn())
            {
                return View("Hello");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login([Bind(Include = "UserName,Password")] Account account)
        {
            // get account user name
            Account accountDetails = db.Accounts.Where(x => x.UserName == account.UserName).FirstOrDefault();
            if (accountDetails == null)
            {
                // no account found
                ViewBag.LoginErrorMessage = "User name or password is incorrect";
                return View("Login", account);
            }
            else
            {
                if (passwordVerified(accountDetails, account))
                {
                    createLoginSession(accountDetails);
                    return View("Hello");
                }
                else
                {
                    ViewBag.LoginErrorMessage = "User name or password is incorrect";
                    return View("Login");
                }
            }
        }

        // Password verify
        [NonAction]
        private bool passwordVerified(Account accountDetails, Account account)
        {
            // get password from db
            var hashedPassword = accountDetails.Password;

            if (Crypto.VerifyHashedPassword(hashedPassword, account.Password))
            {
                return true;
            }
            return false;
        }

        // Create logged in session
        [NonAction]
        private void createLoginSession(Account accountDetails)
        {
            Session["userId"] = accountDetails.UserID;
            Session["userName"] = accountDetails.UserName;
        }

        public ActionResult Logout()
        {
            Session["userId"] = null;
            Session["userName"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }

        // GET: Account/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
