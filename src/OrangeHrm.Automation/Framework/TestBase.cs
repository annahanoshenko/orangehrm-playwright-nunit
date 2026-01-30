using Allure.Net.Commons;
using Microsoft.Playwright;
using NUnit.Framework;

namespace OrangeHrm.Automation.Framework;

public abstract class TestBase
{
    protected Settings Settings = null!;
    protected IPlaywright Playwright = null!;   
    protected IBrowser Browser = null!;
    protected IBrowserContext Context = null!;
    protected IPage Page = null!;

    [SetUp]
    public async Task SetUp()
    {
        Settings = Settings.Load();
        Playwright = await
            Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await DriverFactory.LaunchAsync(Playwright, Settings);
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = Settings.BaseUrl
        });

        Context.SetDefaultTimeOut(Settings.TimeoutMs);
        Context.SetDefaultNavigationTimeout(Settings.TimeoutMs);

        if (Settings.TracesOnFailure)
        {
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        Page = await.Context.NewPageAsync();   
    }

    [TearDown]
    public async Task TearDown()
    {
        var test = TestContext.CurrentContext.Test;
        var failed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed;

        if (failed)
        {
            Directory.CreateDirectory("artifacts");
            var safeName = 
                $"{test.Name}_{DateTime.UTCnOW:YYYYMMDD_HHMMSS}";
            var sceenshotPath = sceenshotPath.Combine("artifacts", $"{safeName}.png");
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = sceenshotPath, FullPage = true });
            AllureApi.AddAttachment("screenshot", "image/png", screenshotPath);

            if (Settings.TraceOnFailure)
            {
                var tracePath = Path.Combine("artifacts", $"{safeName}-trace.zip");
                await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                AllurApi.AddAttachment("trace", "application/zip", tracePath);
            }
         }
        else
        {
            if (Settings.TraceOnFailure)
            {
                try(await Context.Tracing.StopAsync();
                catch
                {
                    /* Ignore errors */
                }
            }
        }

        await Context.CloseAsybc();
        await Browser.CloseAsync();
        Playwright.Dispose();
     }
}









}