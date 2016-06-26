using System;
using System.Linq;
using System.Web.Mvc;
using WarsawSleepTime.Algorithms.Algorithm;
using WarsawSleepTime.Entities.Context;
using WarsawSleepTime.Entities.Entities;
using WarsawSleepTime.Models.Models.FriendsModels;

namespace WarsawSleepTime.Controllers
{
    public class FriendsController : Controller
    {

        private readonly WarsawSleepTimeContext context;
        private readonly IAlgorithmService algorithmService;
        public FriendsController()
        {
            context = new WarsawSleepTimeContext();
            algorithmService = new AlgorithmService(context);
        }

        [Authorize]
        public ActionResult Friends()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            PutNameIntoViewBag();
            UserFriendsModel model;
            if (firstOrDefault != null)
            {
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
                model = new UserFriendsModel
                {
                    Friends =
                        context.Friendships.Where(x => (x.Customer.Id == firstOrDefault.Id)).ToArray()
                            .Select(
                                x =>
                                    new FriendItem
                                    {
                                        Image = x.CustomerFriend.Image,
                                        Id = x.CustomerFriend.Id,
                                        FirstName = x.CustomerFriend.FirstName,
                                        LastName = x.CustomerFriend.LastName
                                    }),
                    FriendsToAdd = algorithmService.FriendsByAssociation(firstOrDefault.Id, 100)
                };
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ShowProfile(int id)
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            PutNameIntoViewBag();
            var c = context.Customers.First(x => x.Id == id);
            var model = new FriendModel
            {
                Image = c.Image,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Id = c.Id,
                DateOfBirth = c.DateOfBirth,
                Gender = c.Gender,
                PhoneNumber = c.PhoneNumber,
                IsFriend = context.Friendships.Any(x => (x.CustomerFriend.Id == id && x.Customer.Id == firstOrDefault.Id))
            };
            return View(model);
        }

        [Authorize]
        public ActionResult AssignFriend(int id)
        {
            try
            {
                var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
                PutNameIntoViewBag();
                var c = context.Customers.First(x => x.Id == id);
                if (firstOrDefault != null && c.Id == firstOrDefault.Id)
                {
                    return RedirectToAction("Friends", "Friends");
                }
                var me = context.Customers.First(x => x.User.UserName == User.Identity.Name);
                if (context.Friendships.Any(x => (x.CustomerFriend.Id == id && x.Customer.Id == firstOrDefault.Id)))
                {
                    return RedirectToAction("Friends", "Friends");
                }
                var friendship = new Friendship { Customer = me, CustomerFriend = c, FriendshipDate = DateTime.Today };
                context.Friendships.Add(friendship);

                context.SaveChanges();
                return RedirectToAction("Friends", "Friends");
            }
            catch (Exception)
            {
                return RedirectToAction("Friends", "Friends");
            }
        }

        [Authorize]
        public ActionResult RemoveFriend(int id)
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            PutNameIntoViewBag();
            if (context.Friendships.All(x => (x.CustomerFriend.Id != id && x.Customer.Id != firstOrDefault.Id)))
            {
                return RedirectToAction("Friends", "Friends");
            }
            var friendship = context.Friendships.First(x => (x.CustomerFriend.Id == id && x.Customer.Id == firstOrDefault.Id));
            context.Friendships.Remove(friendship);
            context.SaveChanges();
            return RedirectToAction("Friends", "Friends");
        }

        #region helpers
        private void PutNameIntoViewBag()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
        }
        #endregion
    }
}
