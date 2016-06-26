using System.Collections.Generic;
using WarsawSleepTime.Models.Models.FriendsModels;

namespace WarsawSleepTime.Algorithms.Algorithm
{
    public interface IAlgorithmService
    {
        /// <summary>
        /// Returns collections of friends suggestions for specified user.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="resultCount"></param>
        /// <returns></returns>
        IEnumerable<FriendItem> FriendsByAssociation(int customerId, int resultCount);
    }
}