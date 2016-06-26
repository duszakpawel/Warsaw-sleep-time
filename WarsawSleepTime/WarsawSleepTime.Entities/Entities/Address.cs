using System.ComponentModel.DataAnnotations.Schema;

namespace WarsawSleepTime.Entities.Entities
{
    [ComplexType]
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}