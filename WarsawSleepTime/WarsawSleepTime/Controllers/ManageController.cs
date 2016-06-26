using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WarsawSleepTime.Entities.Context;
using WarsawSleepTime.Entities.Entities;
using WarsawSleepTime.Models.Models;
using WarsawSleepTime.Shared.Enums;
using WarsawSleepTime.Models.Models.ManageModels;

namespace WarsawSleepTime.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;
        private readonly WarsawSleepTimeContext context;
        public ManageController()
        {
            context = new WarsawSleepTimeContext();
        }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            context = new WarsawSleepTimeContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file == null) return RedirectToAction("Update", "Manage");
            try
            {
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    var array = ms.GetBuffer();
                    var firstOrDefault =
                        context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                    if (firstOrDefault != null)
                        ViewBag.LoggedUserName = firstOrDefault.FirstName;
                    var c = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                    if (c != null) c.Image = array;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Update", "Manage");
            }
            return RedirectToAction("Update", "Manage");
        }

        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            PutNameIntoViewBag();
            return View();
        }

        public ActionResult UpdateAddress()
        {
            PutNameIntoViewBag();
            var c = context.Customers.First(x => x.User.UserName == User.Identity.Name);
            var model = new UpdateAddressModel
            {
                City = c.PlaceOfResidence.City,
                Country = c.PlaceOfResidence.Country,
                Number = c.PlaceOfResidence.Number,
                Street = c.PlaceOfResidence.Street
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAddress(UpdateAddressModel model)
        {
            PutNameIntoViewBag();
            if (ModelState.IsValid)
            {
                try
                {
                    var c = context.Customers.First(x => x.User.UserName == User.Identity.Name);
                    c.PlaceOfResidence = new Address
                    {
                        City = model.City,
                        Country = model.Country,
                        Number = model.Number,
                        Street = model.Street
                    };
                    context.Customers.AddOrUpdate(c);
                    context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbEntityValidationException e)
                {
                    PrintDbEntryValidationExceptions(e);
                }

            }
            AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Update", "Manage");
            }
            AddErrors(result);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Update()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
            var c = await context.Customers.FirstOrDefaultAsync(x => x.User.UserName == User.Identity.Name);
            var model = new UpdateUserModel
            {
                DateTime = c.DateOfBirth,
                Gender = c.Gender,
                PhoneNumber = c.PhoneNumber,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Image = c.Image
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UpdateUserModel model)
        {
            PutNameIntoViewBag();
            if (ModelState.IsValid)
            {
                try
                {
                    var c = context.Customers.First(x => x.User.UserName == User.Identity.Name);
                    c.FirstName = model.FirstName;
                    c.DateOfBirth = model.DateTime;
                    c.Gender = model.Gender;
                    c.LastName = model.LastName;
                    c.PhoneNumber = model.PhoneNumber;
                    context.Customers.AddOrUpdate(c);
                    await context.SaveChangesAsync();

                }
                catch (DbEntityValidationException e)
                {
                    PrintDbEntryValidationExceptions(e);
                }
                return RedirectToAction("Index", "Home");
            }
            AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
            return View(model);
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Update", "Manage");
            }
            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult UpdatePreferences()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;

            if (firstOrDefault == null) return View();
            {
                var model = new UpdatePreferencesViewModel
                {
                    ChosenLanguages = firstOrDefault.UserPreference.Languages.Select(x => (int)x.Language),
                    ChosenFeatures = firstOrDefault.UserPreference.AdditionalFeatures.Select(x => (int)x.AdditionalFeature),
                };
                return View(model);
            }
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePreferences(UpdatePreferencesViewModel model)
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
            if (ModelState.IsValid)
            {
                var customer = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                if (customer != null)
                {
                    customer.UserPreference.Languages.Clear();
                    customer.UserPreference.AdditionalFeatures.Clear();
                    customer.UserPreference.Languages = GetLanguages(model.ChosenLanguages);
                    customer.UserPreference.AdditionalFeatures = GetFeatures(model.ChosenFeatures);
                    context.Customers.AddOrUpdate(customer);
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
            AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
            return View(model);
        }

        private ICollection<AdditionalFeatureInfo> GetFeatures(IEnumerable<int> chosenFeatures)
        {
            var list = new List<AdditionalFeatureInfo>();
            if (chosenFeatures == null) return list;
            list.AddRange(chosenFeatures.Select(feature => new AdditionalFeatureInfo {Id = feature, AdditionalFeature = (AdditionalFeature) feature}));
            return list;
        }

        private ICollection<LanguageInfo> GetLanguages(IEnumerable<int> chosenLanguages)
        {
            var list = new List<LanguageInfo>();
            if (chosenLanguages == null) return list;
            list.AddRange(chosenLanguages.Select(language => new LanguageInfo {Id = language, Language = (Language) language}));
            return list;
        }

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private void PutNameIntoViewBag()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
        }

        private static void PrintDbEntryValidationExceptions(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine(
                    "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
        #endregion
    }
}