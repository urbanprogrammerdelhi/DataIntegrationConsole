using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("nomineeDetails")]
    public class BTNomineeDetails
    {
        public string Name { get; set; }
        public string RelationshipwithEmployees { get; set; }
        public string AadharNo { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string MobileNo { get; set; }
        public string AddressasPerAadhar { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Pincode { get; set; }
        public string ResidingWithEmployee { get; set; }
        public string SupportingDoc1Upload { get; set; }

    }
}