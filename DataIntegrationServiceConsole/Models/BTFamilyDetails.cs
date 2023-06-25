using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{

    [JsonObject("familyDetails")]
    public class BTFamilyDetails
    {
        public string NameasPerAadhar { get; set; }
        public string RelationshipWithEmployee { get; set; }
        public string AadharNo { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DependentYN { get; set; }
        public string ResidingWithEmployee { get; set; }
        public string AddressasPerAadhar { get; set; }
        public string Address1 { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string FamilyDetailsDocument { get; set; }

    }
}