using Allure.Net.Commons;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework;
using Microsoft.Playwright;


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

        Context.SetDefaultTimeout(Settings.TimeoutMs);
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

        Page = await Context.NewPageAsync();   
    }

    [TearDown]
    public async Task TearDown()
    {
        var test = TestContext.CurrentContext.Test;
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;

        if (failed)
        {
            var screenshotPath = Path.Combine("artifacts", $"{safeName}.png");
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });

            AllureApi.AddAttachment("screenshot", "image/png", screenshotPath);

            if (Settings.TraceOnFailure)
            {
                var tracePath = Path.Combine("artifacts", $"{safeName}-trace.zip");
                await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                AllureApi.AddAttachment("trace", "application/zip", tracePath);
            }
         }
        else
        {
            if (Settings.TracesOnFailure)
            {
                try 
                {
                    await Context.Tracing.StopAsync();
                }
                catch
                {
                    /* Ignore errors */
                }
            }
        }

        await Context.CloseAsync();
        await Browser.CloseAsync();
        Playwright.Dispose();
     }
}


