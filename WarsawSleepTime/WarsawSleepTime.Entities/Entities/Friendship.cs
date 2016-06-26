using System;

namespace WarsawSleepTime.Entities.Entities
{
    public class Friendship : Entity
    {
        public DateTime FriendshipDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer CustomerFriend { get; set; }
    }
}