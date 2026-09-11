using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,             

        [EnumMember(Value = "Confirmed")]
        Confirmed,           

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,     

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,       

        [EnumMember(Value = "Shipped")]
        Shipped,            

        [EnumMember(Value = "Delivered")]
        Delivered,      

        [EnumMember(Value = "Canceled")]
        Canceled

    }
}
