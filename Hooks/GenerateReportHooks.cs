using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using Specflow_Selenium.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace Specflow_Selenium.Hooks
{
    [Binding]
    public class GenerateReportHooks
    {
        private ScenarioContext _scenarioContext;
        private FeatureContext _featureContext;
        private TestContext _testContext;

        private static List<string> _logs;
        private static ReportLogs _report;
        private static ExtentReports _extent;
        private static ExtentV3HtmlReporter _v3HtmlReporter;

        public GenerateReportHooks(ScenarioContext scenarioContext, FeatureContext featureContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _testContext = testContext;
        }

        [BeforeTestRun(Order = 4)]
        public static void InitializeReport()
        {
            _logs = new List<string>();

            _report = new ReportLogs
            {
                Name = "Trade Me Sandbox Test Automation"
            };

            var reportsFilenamePrefix = "Trade Me Sandbox";

            var reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }

            var reportsFilename = $"{reportsFilenamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}_Test Automation.html";

            var reportsPath = Path.Combine(reportsDirectory, reportsFilename);

            _v3HtmlReporter = new ExtentV3HtmlReporter(reportsPath);

            _v3HtmlReporter.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extent-config.xml"));

            _v3HtmlReporter.Start();

            _extent = new ExtentReports();

            _extent.AttachReporter(_v3HtmlReporter);
        }

        [BeforeStep(Order = 0)]
        public void AddLogsArray()
        {
            _logs = new List<string>();
        }

        [AfterStep(Order = 3)]
        public void CreateStepNode()
        {
            if (_report.Features == null)
            {
                _report.Features = new List<FeatureNode>();
            }

            var feature = _report.Features.Find(f => f.Name == _featureContext.FeatureInfo.Title);

            if (feature == null)
            {
                feature = new FeatureNode
                {
                    Name = _featureContext.FeatureInfo.Title
                };

                _report.Features.Add(feature);
            }

            if (feature.Scenarios == null)
            {
                feature.Scenarios = new List<ScenarioNode>();
            }

            var scenario = feature.Scenarios.Find(s => s.Name == _testContext.Test.Name);

            if (scenario == null)
            {
                scenario = new ScenarioNode
                {
                    Name = _testContext.Test.Name,
                    Tags = _scenarioContext.ScenarioInfo.Tags
                };

                feature.Scenarios.Add(scenario);
            }

            var keyword = _scenarioContext.StepContext.StepInfo.StepInstance.Keyword;
            var step = _scenarioContext.StepContext.StepInfo.StepInstance.Text;

            if (String.IsNullOrEmpty(keyword))
            {
                keyword = _scenarioContext.StepContext.StepInfo.StepInstance.StepDefinitionKeyword.ToString();

                _logs.Add($"{keyword} {step}");
            }
            else
            {
                if (_logs != null)
                {
                    _logs = _logs.Select(x => "&emsp;" + x.Replace("<br />", "<br />&emsp;")).ToList();
                }

                if (scenario.Steps == null)
                {
                    scenario.Steps = new List<StepNode>();
                }

                if (_scenarioContext.TestError == null)
                {
                    scenario.Steps.Add(new StepNode
                    {
                        Keyword = keyword,
                        Name = step,
                        Pass = true,
                        Logs = new List<string>(_logs),
                        ErrorMessage = string.Empty,
                    });
                }
                else
                {
                    scenario.Steps.Add(new StepNode
                    {
                        Keyword = keyword,
                        Name = step,
                        Pass = _scenarioContext.TestError == null ? true : false,
                        Logs = new List<string>(_logs),
                        ErrorMessage = _scenarioContext.TestError.Message.ToString(),
                    });
                }
            }
        }

        [AfterTestRun(Order = 3)]
        public static void GenerateTestReport()
        {
            _v3HtmlReporter.Stop();

            foreach (var feature in _report.Features)
            {
                var _featureNode = _extent.CreateTest<Feature>(feature.Name);

                foreach (var scenario in feature.Scenarios)
                {
                    var _scenarioNode = _featureNode.CreateNode<Scenario>(scenario.Name);

                    foreach (var tag in scenario.Tags)
                    {
                        _scenarioNode.AssignCategory(tag);
                    }

                    foreach (var step in scenario.Steps)
                    {
                        var gherkinKeyword = new GherkinKeyword(step.Keyword.Trim());

                        var stepNode = _scenarioNode.CreateNode(gherkinKeyword, step.Keyword + step.Name);

                        foreach (var log in step.Logs)
                        {
                            stepNode.Info(log);
                        }

                        if (!step.Pass)
                        {
                            stepNode.Fail(step.ErrorMessage);
                        }
                    }
                }
            }

            _extent.Flush();
        }
    }
}
