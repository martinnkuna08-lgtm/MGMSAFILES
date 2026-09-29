using Dapper;
using Mandiluve.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Mandiluve.Controllers
{
	public class UserController : Controller
	{
		private IDbConnection _context;

		public UserController(IDbConnection context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Login()
		{

			return View();

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(string email, string password)
		{


			var rg = await _context.QueryFirstOrDefaultAsync<User>("sp_LoginUser", new { Email = email, Password = password }, commandType: CommandType.StoredProcedure);


			var a = this.HttpContext.Session.GetString("UserID");
			HttpContext.Session.SetString("UserID", rg.UserID.ToString());

			var b = this.HttpContext.Session.GetString("Email");
			HttpContext.Session.SetString("Email", rg.Email.ToString());

			var c = this.HttpContext.Session.GetString("FirstName");
			HttpContext.Session.SetString("FirstName", rg.FirstName.ToString());

			var d = this.HttpContext.Session.GetString("LastName");
			HttpContext.Session.SetString("LastName", rg.LastName.ToString());

			var e = this.HttpContext.Session.GetString("UserType");
			HttpContext.Session.SetString("UserType", rg.UserType.ToString());

			var f = this.HttpContext.Session.GetString("Title");
			HttpContext.Session.SetString("Title", rg.Title.ToString());

			if (rg.UserType.ToString() == "Admin" & rg.Active.ToString() == "T")
				return RedirectToAction("Index", "Admin", new { userId = rg.UserID });
			else if (rg.UserType.ToString() == "Nurse" & rg.Active.ToString() == "T")
				return RedirectToAction("Index", "Nurse", new { userId = rg.UserID });
			else if (rg.UserType.ToString() == "Doctor" & rg.Active.ToString() == "T")
				return RedirectToAction("Index", "Doctor", new { userId = rg.UserID });
			else if (rg.UserType.ToString() == "Surgeon" & rg.Active.ToString() == "T")
				return RedirectToAction("Index", "Surgeon", new { userId = rg.UserID });
			else if (rg.UserType.ToString() == "Pharmacist" & rg.Active.ToString() == "T")
				return RedirectToAction("Index", "Pharmacist", new { userId = rg.UserID });

			else if ((rg.UserType.ToString() == "Admin" || rg.UserType.ToString() == "Nurse" || rg.UserType.ToString() == "Doctor" || rg.UserType.ToString() == "Warden" || rg.UserType.ToString() == "ScriptManager") & rg.Active.ToString() == "F")
				TempData["ErrorMessage"] = "User Account Locked,Contact Admin For Further Assistance";


			return View();

		}
	}
}
