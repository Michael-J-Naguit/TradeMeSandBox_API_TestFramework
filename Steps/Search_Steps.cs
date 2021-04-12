using RestSharp;
using RestSharp.Authenticators;
using TechTalk.SpecFlow;
using TradeMeSandBox_API_TestFramework.Contexts;

namespace TradeMeSandBox_API_TestFramework.Steps
{
    [Binding]
    public sealed class Search_Steps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private readonly DataContext _data;

        public Search_Steps(ScenarioContext scenarioContext, DataContext data)
        {
            _scenarioContext = scenarioContext;
            _data = data;
        }

        [Given(@"Setup General Search request")]
        public void GivenGeneralSetupSearchRequest()
        {
            _data.client = new RestClient(_data.searchGeneralURL);
            _data.client.Authenticator = OAuth1Authenticator.ForProtectedResource(_data.consumerKey, _data.consumerSecret, _data.oAuthToken, _data.oAuthTokenSecret);

            /*
            _data.client = new RestClient(_data.searchGeneralURL);
            _data.client.Proxy = new WebProxy("prodproxy.test1.net:8080");
            _data.client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials; */
        }

        [Given(@"Setup General Search request parameters")]
        public void GivenSetupGeneralSearchRequestParameters(Table table)
        {
            _data.request = new RestRequest(Method.GET);
            string[] headers = new string[table.Header.Count];
            table.Header.CopyTo(headers, 0);

            for (int i=0; i< table.Header.Count; i++)
            {
                _data.request.AddParameter(headers[i], table.Rows[0][i]);
            }
        }

    }
}
