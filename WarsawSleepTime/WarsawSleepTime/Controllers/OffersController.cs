using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using WarsawSleepTime.Entities.Context;
using WarsawSleepTime.Entities.Entities;
using WarsawSleepTime.Helpers;
using WarsawSleepTime.Models.Models.OffersModels;
using WarsawSleepTime.Shared.Enums;
namespace WarsawSleepTime.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private readonly WarsawSleepTimeContext context;

        private const double TOLERANCE = 0.0000001;
        private const double defaultLat = 52.2296756;
        private const double defaultLng = 21.0122287;
        private const int limit = 100;

        public OffersController()
        {
            context = new WarsawSleepTimeContext();
        }

        public ActionResult AddOffer()
        {
            PutNameIntoViewBag();
            var model = new AddOfferModel();
            return View(model);
        }

        public ActionResult FileUpload(HttpPostedFileBase file, int id)
        {
            if (file == null) return RedirectToAction("YourOffers", "Offers");
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                    var firstOrDefault =
                        context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                    if (firstOrDefault != null)
                        ViewBag.LoggedUserName = firstOrDefault.FirstName;
                    context.CouchsurfingOffers.First(x => x.Id == id).Image = array;
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                PrintDbEntryValidationExceptions(e);
            }
            return RedirectToAction("YourOffers", "Offers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOffer(AddOfferModel model)
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            PutNameIntoViewBag();
            if (ModelState.IsValid)
            {
                var offer = new CouchsurfingOffer
                {
                    Date = model.Date,
                    Owner = firstOrDefault,
                    Description = model.Description,
                    District = "Masovian",
                    Latitude = model.latitude,
                    Longtitude = model.longtitude,
                    Street = model.Street,
                    Status = Status.Ready,
                    Number = model.Number
                };
                string json;
                string url = ConfigurationManager.AppSettings["googleApiPlacesAddressBase"] + model.Street+"+"+model.Number+",+Warszawa,+Polska&sensor=false";
                using (WebClient webClient = new WebClient())
                {
                    json = webClient.DownloadString(url);
                }
                try
                {                   
                    DeserializedObject result = (DeserializedObject)JsonConvert.DeserializeObject(json, typeof(DeserializedObject));
                    if(Math.Abs(result.results[0].geometry.location.lat - defaultLat) < TOLERANCE && Math.Abs(result.results[0].geometry.location.lng - defaultLng) < TOLERANCE)
                        throw new ArgumentException();
                    offer.Latitude = result.results[0].geometry.location.lat;
                    offer.Longtitude = result.results[0].geometry.location.lng;
                    context.CouchsurfingOffers.Add(offer);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
                    return View();
                }
                                
                return RedirectToAction("YourOffers", "Offers");
            }
            AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
            return RedirectToAction("YourOffers", "Offers");
        }

        [Authorize]
        public ActionResult YourOffers()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            PutNameIntoViewBag();
            var userOffers =
                context.CouchsurfingOffers.Where(x => x.Owner.Id == firstOrDefault.Id)
                    .Select(
                        x =>
                            new AddOfferModel
                            {
                                Image = x.Image,
                                Id = x.Id,
                                Description = x.Description,
                                Date = x.Date ?? DateTime.Today,
                                Number = x.Number,
                                Street = x.Street,
                                OwnerFirstName = x.Owner.FirstName,
                                OwnerLastName = x.Owner.LastName,
                                OwnerId = x.Owner.Id
                            })
                    .ToList();
            var assignedOffers =
                context.CouchsurfingOffers.Where(x => x.Client.Id == firstOrDefault.Id)
                    .Select(
                        x =>
                            new AddOfferModel
                            {
                                Image = x.Image,
                                Id = x.Id,
                                Description = x.Description,
                                Date = x.Date ?? DateTime.Today,
                                Number = x.Number,
                                Street = x.Street,
                                OwnerFirstName = x.Owner.FirstName,
                                OwnerLastName = x.Owner.LastName,
                                OwnerId = x.Owner.Id
                            })
                    .ToList();
            var model = new UserOffers
            {
                UsersOffers = userOffers,
                AssignedOffers = assignedOffers
            };
            return View(model);
        }

        [Authorize]
        public ActionResult RemoveOffer(int id)
        {
            try
            {
                var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                if (firstOrDefault != null)
                {
                    ViewBag.LoggedUserName = firstOrDefault.FirstName;
                    if (context.CouchsurfingOffers.First(x => x.Id == id).Owner.Id != firstOrDefault.Id)
                    {
                        return RedirectToAction("YourOffers", "Offers");
                    }
                }
                var offer = context.CouchsurfingOffers.First(x => x.Id == id);
                var historical = new HistoricalOffer
                {
                    Owner=offer.Owner.FirstName+" "+offer.Owner.LastName,
                    Client =offer.Client?.FirstName+" "+offer.Client?.LastName,
                    Date =offer.Date
                }; 
                context.CouchsurfingOffers.Where(x => x.Id == id).Delete();
                context.HistoricalOffers.Add(historical);
                context.SaveChanges();
                return RedirectToAction("YourOffers", "Offers");
            }
            catch (DbEntityValidationException e)
            {
                PrintDbEntryValidationExceptions(e);
                AddErrors(new IdentityResult("Incorrect values. Correct them and try again."));
                return RedirectToAction("YourOffers", "Offers");
            }
        }

        [Authorize]
        public ActionResult AssignOffer(int id)
        {
            var firstOrDefault = context.Customers.First(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
            var offer = context.CouchsurfingOffers.First(x => x.Id == id);
            if (firstOrDefault == null || offer.Owner.Id==firstOrDefault.Id)
            {
                return RedirectToAction("YourOffers", "Offers");
            }
            var owner = offer.Owner;
            if (offer.Client == null)
            {
                offer.Client = firstOrDefault;
                offer.Owner = owner;
                offer.Status = Status.Accepted;
            }
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                PrintDbEntryValidationExceptions(e);
            }
            return RedirectToAction("YourOffers", "Offers");
        }

        [Authorize]
        public ActionResult Search()
        {
            PutNameIntoViewBag();
            return View();
        }

        [AuthorizeWithAjax]
        public ActionResult Offer(int id)
        {
            var firstOrDefault = context.Customers.First(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault == null) return RedirectToAction("Index", "Home");
            ViewBag.LoggedUserName = firstOrDefault.FirstName;
            var offer = context.CouchsurfingOffers.First(x => x.Id == id);
            var model = new OfferModelExtended
            {
                IsAssigned = context.CouchsurfingOffers.First(x => x.Id == id).Client?.Id == firstOrDefault.Id,
                IsOwning = context.CouchsurfingOffers.First(x=>x.Id==id).Owner.Id==firstOrDefault.Id,
                Image=offer.Image,
                Description=offer.Description,
                Latitude=offer.Latitude,
                Longtitude=offer.Longtitude,
                Number=offer.Number,
                Date=offer.Date,
                Steet=offer.Street,
                ownerId=offer.Owner.Id,
                ownerName=offer.Owner.FirstName+" "+offer.Owner.LastName,
                Id=offer.Id
                
            };
            if (offer.Client == null) return View(model);
            model.clientId = offer.Client.Id;
            model.ClientName = offer.Client.FirstName + " " + offer.Client.LastName;
            return View(model);
        }

        [AuthorizeWithAjax]
        [HttpPost]
        public JsonResult SearchigResults(SearchingCriteria criteria)
        {
            var model = new OfferSearchingResult();
            if (criteria.Paid)
            {
                FindHotels(criteria, model);
            }
            if (criteria.Dormitories)
            {
                FindDormitories(criteria, model);
            }
            if (criteria.Hostels)
            {
                FindHostels(criteria, model);
            }

            if (criteria.Free)
            {
                FindFreeOffers(criteria, model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void FindFreeOffers(SearchingCriteria criteria, OfferSearchingResult model)
        {
            var me = context.Customers.First(x => x.User.UserName == User.Identity.Name);
            var data = context.CouchsurfingOffers.Where(
                x => x.Owner.Id != me.Id && x.Client == null).Where(t =>
                t.Longtitude > criteria.southWestX && t.Longtitude < criteria.northEastX && t.Latitude > criteria.northEastY && t.Latitude < criteria.southWestY);
            if (!criteria.Street.IsNullOrWhiteSpace())
            {
                data = data.Where(x => x.Street == criteria.Street);
            }
            if (!criteria.Number.IsNullOrWhiteSpace())
            {
                data = data.Where(x => x.Number == criteria.Number);
            }
            if (criteria.MatchPreferences)
            {
                data =
                    data.Where(x =>
                            (x.Owner.UserPreference.AdditionalFeatures.Intersect(me.UserPreference.AdditionalFeatures).Any() && x.Owner.UserPreference.Languages.Intersect(me.UserPreference.Languages).Any()));
            }
            if (criteria.DateSpecified)
            {
                data = data.Where(x => DbFunctions.TruncateTime(x.Date.Value) == DbFunctions.TruncateTime(criteria.Date));
            }
            data = data.Take(limit);
            model.FreeOffers.AddRange(data.Select(result => new OfferModel
            {
                Id = result.Id,
                Owner = result.Owner.FirstName + " " + result.Owner.LastName,
                Longtitude = result.Longtitude,
                Latitude = result.Latitude
            }));
        }

        private static void FindHostels(SearchingCriteria criteria, OfferSearchingResult model)
        {
            string json;
            string url = ConfigurationManager.AppSettings["googleApiTextSearchPlacesBase"]+(criteria.southWestY + criteria.northEastY)/2 + "," +(criteria.northEastX + criteria.southWestX)/2 + "&radius=" + GetStepFromZoom(criteria.zoom);
            string query = "&query=hostel+Warsaw";
            if (!criteria.Street.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Street;
            }
            if (!criteria.Number.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Number;
            }
            url += query;
            string key = ConfigurationManager.AppSettings["googleApiKey"];
            url += key;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                json = webClient.DownloadString(url);
            }
            try
            {
                DeserializedObjectMapPlace result =(DeserializedObjectMapPlace)JsonConvert.DeserializeObject(json, typeof (DeserializedObjectMapPlace));

                model.Hostels.AddRange(result.results
                    .Where(
                res => res.geometry.location.lng > criteria.southWestX &&
                       res.geometry.location.lng < criteria.northEastX &&
                       res.geometry.location.lat > criteria.northEastY &&
                       res.geometry.location.lat < criteria.southWestY&& (criteria.Street.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Street.ToLower())) && (criteria.Number.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Number.ToLower())))
                    .Select(res => new OfferModel
                    {
                        Latitude = res.geometry.location.lat,
                        Longtitude = res.geometry.location.lng,
                        Owner = res.name,
                        Url = res.formatted_address
                    }));

                
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void FindDormitories(SearchingCriteria criteria, OfferSearchingResult model)
        {
            string json;
            string url = ConfigurationManager.AppSettings["googleApiTextSearchPlacesBase"]+ (criteria.southWestY + criteria.northEastY)/2 + "," + (criteria.northEastX + criteria.southWestX)/2 + "&radius=" + GetStepFromZoom(criteria.zoom);
            string query = "&query=dormitory+Warsaw";
            if (!criteria.Street.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Street;
            }
            if (!criteria.Number.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Number;
            }
            url += query;
            string key = ConfigurationManager.AppSettings["googleApiKey"];
            url += key;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                json = webClient.DownloadString(url);
            }
            try
            {
                DeserializedObjectMapPlace result =(DeserializedObjectMapPlace)JsonConvert.DeserializeObject(json, typeof (DeserializedObjectMapPlace));

                model.Dormitories.AddRange(result.results
                                    .Where(
                res => res.geometry.location.lng > criteria.southWestX &&
                       res.geometry.location.lng < criteria.northEastX &&
                       res.geometry.location.lat > criteria.northEastY &&
                       res.geometry.location.lat < criteria.southWestY && (criteria.Street.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Street.ToLower())) && (criteria.Number.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Number.ToLower())))
                    .Select(res => new OfferModel
                           {
                               Latitude = res.geometry.location.lat,
                               Longtitude = res.geometry.location.lng,
                               Owner = res.name,
                               Url = res.formatted_address
                           }));

            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void FindHotels(SearchingCriteria criteria, OfferSearchingResult model)
        {
            string json;
            string url = ConfigurationManager.AppSettings["googleApiTextSearchPlacesBase"] + (criteria.southWestY + criteria.northEastY)/2 + "," +
                         (criteria.northEastX + criteria.southWestX)/2 + "&radius=" + GetStepFromZoom(criteria.zoom);
            string query = "&query=hotel+Warsaw";
            if (!criteria.Street.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Street;
            }
            if (!criteria.Number.IsNullOrWhiteSpace())
            {
                query += "+" + criteria.Number;
            }
            url += query;
            string key = ConfigurationManager.AppSettings["googleApiKey"];
            url += key;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                json = webClient.DownloadString(url);
            }
            try
            {
                DeserializedObjectMapPlace result =(DeserializedObjectMapPlace)JsonConvert.DeserializeObject(json, typeof (DeserializedObjectMapPlace));

                model.PaidOffers.AddRange(result.results
                    .Where(res => res.geometry.location.lng > criteria.southWestX &&
                                                                         res.geometry.location.lng < criteria.northEastX &&
                                                                         res.geometry.location.lat > criteria.northEastY &&
                                                                         res.geometry.location.lat < criteria.southWestY && (criteria.Street.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Street.ToLower())) && (criteria.Number.IsNullOrWhiteSpace() || res.formatted_address.ToLower().Contains(criteria.Number.ToLower())))
                    .Select(res => new OfferModel
                    {
                        Latitude = res.geometry.location.lat,
                        Longtitude = res.geometry.location.lng,
                        Owner = res.name,
                        Url = res.formatted_address
                    }));  
                
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #region Helpers

        private static int GetStepFromZoom(int zoom)
        {
            switch (zoom)
            {
                case 11:
                    return 30000;
                case 12:
                    return 20000;
                case 13:
                    return 8000;
                case 14:
                    return 4000;
                case 15:
                    return 2000;
                case 16:
                    return 500;
                case 17:
                    return 250;
                case 18:
                    return 100;
            }
            return zoom < 11 ? 35000 : 100;
        }

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
