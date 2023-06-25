using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    public class BTEmployeeInformation
    {        
        public List<BTEmployeeBasicDetails> employeeBasicDetails { get; set; }
        public BTAdditionalDetails additionalDetails { get; set; }
        public BTBankDetails bankAccountDetails { get; set; }
        public BTCurrentAddress currentAddress { get; set; }
        public BTFamilyDetails familyDetails { get; set; }
        public BTNomineeDetails nomineeInformation { get; set; }
        public BTPermanentAddress permanentAddress { get; set; }
        public BTPFDetails PFDetails { get; set; }
        public List<BTAadharDetails> AadharDetails {get;set;}
        public BTESIInformation esiInformation { get; set; }
        public BTBackgroundVerificationDetails backgroundVerificationDetails { get; set; }
    }
   
}