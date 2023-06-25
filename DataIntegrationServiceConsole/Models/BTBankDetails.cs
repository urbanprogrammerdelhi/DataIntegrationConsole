using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("bankAccountDetails")]
    public class BTBankDetails
    {
        public string PANNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string BankDocument { get; set; }

    }
}