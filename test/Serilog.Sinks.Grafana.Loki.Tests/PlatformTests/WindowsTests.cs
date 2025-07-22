using System.Text.RegularExpressions;
using Serilog.Sinks.Grafana.Loki.Tests.TestHelpers;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Serilog.Sinks.Grafana.Loki.Tests.PlatformTests;

public class WindowsTests
{
  private readonly ITestOutputHelper _testOutputHelper;
  private readonly TestLokiHttpClient _client;

  public WindowsTests(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
    _client = new TestLokiHttpClient();
  }

  [Fact]
  public void LogInformation_ShouldNotThrow()
  {
    var logger = new LoggerConfiguration()
                 .WriteTo.GrafanaLoki(
                   "http://10.10.0.11:3100",
                   propertiesAsLabels: new[] { "app", "environment" },
                   labels: new[]
                   {
                     new LokiLabel { Key = "app", Value = "MD3" },
                     new LokiLabel { Key = "environment", Value = "Test" }
                   },
                   textFormatter: new LokiJsonTextFormatter(),
                   httpClient: _client)
                 .CreateLogger();

    logger.Information("WORKS!!!");
    logger.Dispose();
  }
}