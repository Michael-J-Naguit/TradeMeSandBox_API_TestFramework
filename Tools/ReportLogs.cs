using System.Collections.Generic;

namespace Specflow_Selenium.Tools
{
    public class ReportLogs
    {
        public string Name { get; set; }
        public List<FeatureNode> Features { get; set; }
    }

    public class FeatureNode
    {
        public string Name { get; set; }
        public List<ScenarioNode> Scenarios { get; set; }
    }

    public class ScenarioNode
    {
        public string Name { get; set; }
        public List<StepNode> Steps { get; set; }
        public string[] Tags { get; set; }
    }

    public class StepNode
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public bool Pass { get; set; }
        public List<string> Logs { get; set; }
        public string ErrorMessage { get; set; }
        public string Screenshot { get; set; }
    }
}
