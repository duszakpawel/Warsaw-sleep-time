using System.Collections.Generic;
using System.Linq;
using WarsawSleepTime.Entities.Context;
using WarsawSleepTime.Entities.Entities;
using WarsawSleepTime.Models.Models.FriendsModels;

namespace WarsawSleepTime.Algorithms.Algorithm
{
    public class AlgorithmService : IAlgorithmService
    {
        /// <summary>
        /// Data context
        /// </summary>
        private readonly WarsawSleepTimeContext context;
        public AlgorithmService() { }
        public AlgorithmService(WarsawSleepTimeContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Returns friends suggestions for specified user.
        /// </summary>
        /// <param name="customerId">specified user</param>
        /// <param name="resultCount">specified results count</param>
        /// <returns></returns>
        public IEnumerable<FriendItem> FriendsByAssociation(int customerId, int resultCount)
        {
            var customer = context.Customers.FirstOrDefault(x => x.Id == customerId);
            var count = 0;
            var result = new List<FriendItem>();
            var leavesQueue = new Queue<Customer>();
            foreach (var offer in context.CouchsurfingOffers.Where(x => x.Client.Id == customerId || x.Owner.Id==customerId).ToArray())
            {
                if (AttachCustomer(customerId, offer, result)) continue;
                count++;
                if (count == resultCount)
                {
                    return result;
                }
            }
            foreach (var offer in context.CouchsurfingOffers.Where(x => x.Owner.Id == customerId && x.Client!=null).ToArray())
            {
                if (AttachOwner(customerId, offer, result)) continue;
                count++;
                if (count == resultCount)
                {
                    return result;
                }
            }
            var treesQueue = new Queue<Customer>(context.Friendships.Where(x => x.Customer.Id == customer.Id).OrderBy(x => x.FriendshipDate).Select(friendship => friendship.CustomerFriend));
            while (count != resultCount)
            {
                foreach (var w in from u in treesQueue from w in context.Friendships.Where(x => (x.Customer.Id == u.Id && x.CustomerFriend.Id != customerId)).ToArray() where MatchFriendsByPersonalPreferences(w.Customer.Id, customerId) select w)
                {
                    if (result.Exists(x => x.Id == w.Customer.Id)) continue;
                    result.Add(new FriendItem { Id = w.Customer.Id, FirstName = w.Customer.FirstName, LastName = w.Customer.LastName, Image = w.Customer.Image });
                    leavesQueue.Enqueue(w.Customer);
                    count++;
                    if (count == resultCount)
                    {
                        return result;
                    }
                }
                if (leavesQueue.Count == 0)
                {
                    return result;
                }
                treesQueue = leavesQueue;
                leavesQueue.Clear();
            }
            return result;
        }

        /// <summary>
        /// Tryies to attach offer owner to results.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="offer"></param>
        /// <param name="result">Success or failure</param>
        /// <returns></returns>
        private bool AttachOwner(int customerId, CouchsurfingOffer offer, List<FriendItem> result)
        {
            if (offer.Client == null) return true;
            if (offer.Client.Id == customerId) return true;
            if (context.Friendships.Any(x => x.Customer.Id == customerId && x.CustomerFriend.Id == offer.Client.Id))
                return true;
            if (!MatchFriendsByPersonalPreferences(customerId, offer.Client.Id)) return true;
            if (result.Exists(x => x.Id == offer.Client.Id)) return true;
            result.Add(
                new FriendItem
                {
                    Id = offer.Client.Id,
                    FirstName = offer.Client.FirstName,
                    LastName = offer.Client.LastName,
                    Image = offer.Client.Image
                });
            return false;
        }
        /// <summary>
        /// Tryies to attach offer client to results.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="offer"></param>
        /// <param name="result">Success or failure</param>
        /// <returns></returns>
        private bool AttachCustomer(int customerId, CouchsurfingOffer offer, List<FriendItem> result)
        {
            if (offer.Owner.Id == customerId)
                return true;
            if (context.Friendships.Any(x => x.Customer.Id == customerId && x.CustomerFriend.Id == offer.Owner.Id))
                return true;
                if (MatchFriendsByPersonalPreferences(customerId, offer.Owner.Id)) return true;
            if (result.Exists(x => x.Id == offer.Owner.Id)) return true;
            result.Add(
                new FriendItem
                {
                    Id = offer.Owner.Id,
                    FirstName = offer.Owner.FirstName,
                    LastName = offer.Owner.LastName,
                    Image = offer.Owner.Image
                });
            return false;
        }
        /// <summary>
        /// Acceptance predicate
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="otherCustomerId"></param>
        /// <returns></returns>
        public bool MatchFriendsByPersonalPreferences(int customerId, int otherCustomerId)
        {
            if (customerId == otherCustomerId)
            {
                return false;
            }
            if (context.Friendships.Any(x => x.Customer.Id == customerId && x.CustomerFriend.Id == otherCustomerId))
            {
                return false;
            }
            var customer = context.Customers.FirstOrDefault(x => x.Id == customerId);
            var other = context.Customers.FirstOrDefault(x => x.Id == otherCustomerId);
            if (customer == null || other == null)
            {
                return false;
            }
            var matchLanguages = customer.UserPreference.Languages.Select(x => x.Language).Intersect(other.UserPreference.Languages.Select(x => x.Language)).Any();
            var matchOtherPreferences = customer.UserPreference.AdditionalFeatures.Select(x => x.AdditionalFeature).Intersect(other.UserPreference.AdditionalFeatures.Select(x => x.AdditionalFeature)).Any();
            return matchLanguages || matchOtherPreferences;
        }
    }
}