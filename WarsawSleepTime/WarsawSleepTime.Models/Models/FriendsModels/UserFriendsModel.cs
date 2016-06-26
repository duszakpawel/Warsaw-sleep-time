using System.Collections.Generic;

namespace WarsawSleepTime.Models.Models.FriendsModels
{
    public class FriendItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public byte[] Image { get; set; }
        public string LastName { get; set; }
    }
    public class UserFriendsModel
    {
        public IEnumerable<FriendItem> Friends { get; set; } = new List<FriendItem>();
        public IEnumerable<FriendItem> FriendsToAdd { get; set; } = new List<FriendItem>();
    }
}