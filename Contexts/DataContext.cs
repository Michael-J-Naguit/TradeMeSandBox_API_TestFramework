using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMeSandBox_API_TestFramework.Contexts
{
    public class DataContext
    {
        // Request
        public string oAuthToken = "F5CD20AB69FCCC6F4FA3A1787C1EF0CE";
        public string oAuthTokenSecret = "9B78629E1BBCABFCF4CE1110CB837DB8";
        public string consumerKey = "301835CA9BB0E377E7FE6642F85BCBD7";
        public string consumerSecret = "7C1FF59DA144781F979014321712DE69";
        public string searchGeneralURL = "https://api.tmsandbox.co.nz/v1/Search/General.json";
        public RestClient client;
        public IRestRequest request;

        // Response
        public IRestResponse response;
    }
}
