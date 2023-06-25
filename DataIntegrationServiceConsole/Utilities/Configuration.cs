using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataIntegrationServiceConsole
{
    public static class Extensions
    {
        public static bool IsBase64String(this string s)
        {
            if (string.IsNullOrEmpty(s)) {  return false; }
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.None);

        }
    }
    public class Configuration
    {
       
        public static string BasicAuthUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["username"].ToString();
            }
        }
        public static string BasicAuthPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["password"].ToString();
            }
        }
        public static string EsiBasicAuthUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["EsiUserName"].ToString();
            }
        }
        public static string EsiBasicAuthPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["EsiPassword"].ToString();
            }
        }
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); } }
        public static string BlueTreeURL { get { return ConfigurationManager.AppSettings["BlueTreeURL"].ToString(); } }
        public static string BlueTreeESIURL { get { return ConfigurationManager.AppSettings["blueTreeESIURL"].ToString(); } }
        public static string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        public static readonly List<string> entitlementList = new List<string>() { "Basic", "F.D.A", "Unit Allowance", "P.F. Deduction", "E.S.I. Deduction", "Weekly Off Allowance", "Professional Tax", "Gun Duity Allowance" };
        public static readonly List<string> entitlementSalHead = new List<string>() { "SHC000001", "SHC000003", "SHC000004", "SHC000005", "SHC000006", "SHC000007", "SHC000017", "SHC000019" };
    }

    public class Utility
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static List<string> Hours()
        {
            List<string> hours = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                hours.Add(i.ToString("D2"));                
            }
            return hours;
        }
        public static List<string> Minutes()
        {
            List<string> minutes = new List<string>();
            for (int i = 0; i <= 59; i++)
            {

                minutes.Add(i.ToString("D2"));
            }
            return minutes;
        }

        
      
    }
}
