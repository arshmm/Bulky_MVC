using BulkyBook.DataAccess.Data;

using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagement(string userId)
        {
            string roleId = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
            RoleManagementVM roleVM = new()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _db.Roles.Select(i => new SelectListItem { Text = i.Name, Value = i.Name }),
                CompanyList = _db.Companies.Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() })

            };

            roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            return View(roleVM);
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleVM)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == roleVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            if (!(roleVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleVM.ApplicationUser.Id);
                if (roleVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var item in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                item.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (item.Company == null)
                {
                    item.Company = new() { Name = "" };
                }
            }

            return Json(new { data = objUserList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while locking/unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully Unlocked" });
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfuly unlocked" });
            }



        }
        #endregion
    }
}