using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaloriesPlan.DAL.DataModel
{
    [Table(name: "UserSubscription")]
    public class UserSubscription
    {
        [Key, Column(Order = 0)]
        public string SubscriberID { get; set; }

        [Key, Column(Order = 1)]
        public string UserID { get; set; }

        [ForeignKey("SubscriberID")]
        public User Subscriber { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
