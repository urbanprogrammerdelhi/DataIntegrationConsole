using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("permanentAddress")]
    public class BTPermanentAddress
    {
        public string AddressAsPerAadhar { get; set; }
        public string AddressAsPerCurrentAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }      
        public string District { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

    }
}