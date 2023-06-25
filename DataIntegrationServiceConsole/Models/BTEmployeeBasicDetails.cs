using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{   
    [JsonObject("employeeBasicDetails")]
    public class BTEmployeeBasicDetails
    {       
        public string AadharAddress { get; set; }       
        public string AadharNo { get; set; }        
        public string AadharVerified { get; set; }
        public string Active { get; set; }        
        public BTEmployeeDetailsAddress Address { get; set; }
        public string BloodGroup { get; set; }
        public string ColorCode { get; set; }
        public string Customer { get; set; }
        public string CustomerLocation { get; set; }
        public string DateofBirth { get; set; }
        public string DateofVerification { get; set; }
        public string Email { get; set; }
        public string EmployeeNumber { get; set; }
        public string EsiNo { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string HomePhone { get; set; }
        public string JobRoleName { get; set; }
        public string LastName { get; set; }
        public string LocationName { get; set; }
        public string MaritalStatus { get; set; }
        public string MiddleName { get; set; }
        public string MobileNo { get; set; }
        public string MotherName { get; set; }
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string OtherName { get; set; }
        public string Region { get; set; }
        public string SpouseName { get; set; }
        public string Status { get; set; }        
        public string Verifier { get; set; }
        public string WorkLocation { get; set; }
        public string WorkLocationName { get; set; }
        public string CreateUser { get; set; }
        public string CreatedDate { get; set; }
        public string EmployeePhoto { get; set; }
        public string LocationID { get; set; }
    }
    
}