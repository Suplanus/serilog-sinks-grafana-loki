using System.Text.RegularExpressions;
using Serilog.Sinks.Grafana.Loki.Tests.TestHelpers;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.Grafana.Loki.Tests.PlatformTests;

public class WindowsTests
{
  private const string ApprovalsFolderName = "Approvals";
  private const string ExceptionStackTraceRegEx = @"(?<= in)(.*?)(?=},\\)";
  private const string ExceptionStackTraceReplacement = " <stack-trace>";
  private const string TimeStampRegEx = "\"[0-9]{19}\"";
  private const string TimeStampReplacement = "\"<unixepochinnanoseconds>\"";
  private readonly TestLokiHttpClient _client;

  public WindowsTests()
  {
    _client = new TestLokiHttpClient();
  }

  [Fact]
  public void LogInformation_ShouldNotThrow()
  {
    var logger = new LoggerConfiguration()
                 .WriteTo.GrafanaLoki(
                   "http://10.10.0.11:3100",
                   httpClient: _client)
                 .CreateLogger();

    logger.Information("This is an information without params");
    logger.Dispose();
    _client.Content.ShouldMatchApproved(
      c =>
      {
        c.SubFolder(ApprovalsFolderName);
        c.WithScrubber(
          s => Regex.Replace(
            s,
            TimeStampRegEx,
            TimeStampReplacement));
      });
  }
}