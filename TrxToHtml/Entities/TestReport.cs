using System.Xml.Serialization;

namespace TrxToHtml.Entities;

[XmlRoot(ElementName = "TestRun", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestReport
{
    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("Times")]
    public Times Times { get; set; }

    [XmlElement("TestSettings")]
    public TestSettings TestSettings { get; set; }

    [XmlArray("Results")]
    [XmlArrayItem("UnitTestResult")]
    public List<UnitTestResult> Results { get; set; }

    [XmlArray("TestDefinitions")]
    [XmlArrayItem("UnitTest")]
    public List<UnitTest> TestDefinitions { get; set; }

    [XmlElement("ResultSummary")]
    public ResultSummary ResultSummary { get; set; }
}

public class Times
{
    [XmlAttribute("creation")]
    public DateTime Creation { get; set; }

    [XmlAttribute("start")]
    public DateTime Start { get; set; }

    [XmlAttribute("finish")]
    public DateTime Finish { get; set; }
}

public class TestSettings
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlElement("Deployment")]
    public Deployment Deployment { get; set; }
}

public class Deployment
{
    [XmlAttribute("runDeploymentRoot")]
    public string RunDeploymentRoot { get; set; }
}

public class UnitTestResult
{
    [XmlAttribute("testName")]
    public string TestName { get; set; }

    [XmlAttribute("computerName")]
    public string ComputerName { get; set; }

    [XmlAttribute("outcome")]
    public string Outcome { get; set; }

    [XmlElement("Output")]
    public Output Output { get; set; }
}

public class Output
{
    [XmlElement("StdOut")]
    public string StdOut { get; set; }
}

public class UnitTest
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("TestMethod")]
    public TestMethod TestMethod { get; set; }
}

public class TestMethod
{
    [XmlAttribute("className")]
    public string ClassName { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }
}

public class ResultSummary
{
    [XmlAttribute("outcome")]
    public string Outcome { get; set; }

    [XmlElement("Counters")]
    public Counters Counters { get; set; }
}

public class Counters
{
    [XmlAttribute("total")]
    public int Total { get; set; }

    [XmlAttribute("executed")]
    public int Executed { get; set; }

    [XmlAttribute("passed")]
    public int Passed { get; set; }

    [XmlAttribute("failed")]
    public int Failed { get; set; }
}