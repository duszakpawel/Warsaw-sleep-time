using System;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Models.Models.FriendsModels
{
    public class FriendModel
    {
        public byte[] Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int PhoneNumber { get; set; }
        public bool IsFriend { get; set; }
    }
}