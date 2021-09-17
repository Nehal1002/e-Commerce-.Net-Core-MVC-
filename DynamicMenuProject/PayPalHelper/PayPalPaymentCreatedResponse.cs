using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.PayPalHelper
{
    public class PayPalPaymentCreatedResponse
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Transaction[] transactions { get; set; }
        public DateTime create_time { get; set; }
        public Link[] links { get; set; }

        //public Guid UniqueId { get; set; }
        //public string description { get; set; }
        //public string invoice_number { get; set; }
        public string custom { get; set; }

        public class Payer
        {
            public string payment_method { get; set; }

        }

        public class Transaction
        {
            //public Guid unique { get; set; }
            public Amount amount { get; set; }
            //public string description { get; set; }
            //public string invoice_number { get; set; }
            public string custom { get; set; }
            public /*object[]*/ List<object> related_resources { get; set; }

        }

        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
            
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }


        }

    }

}
