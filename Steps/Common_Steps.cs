using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TradeMeSandBox_API_TestFramework.Contexts;

namespace TradeMeSandBox_API_TestFramework.Steps
{
    [Binding]
    public sealed class Common_Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly DataContext _data;

        public Common_Steps(ScenarioContext scenarioContext, DataContext data)
        {
            _scenarioContext = scenarioContext;
            _data = data;
        }

        [When(@"Send request to API")]
        public void GivenSendRequestToAPI()
        {
            _data.response = _data.client.Execute(_data.request);

            JObject obs = JObject.Parse(_data.response.Content);
        }

        [Then(@"Response status is '(.*)'")]
        public void ThenResponseStatusIs(string status)
        {
            Assert.AreEqual(status, _data.response.StatusCode.ToString());
        }

    }
}
