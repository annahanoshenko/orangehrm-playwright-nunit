using Microsoft.Playwright;

namespace OrangeHrm.Automation.Framework;

public static class DriverFactory
{
    public static async Task<IBrowser> LaunchAsync(IPlaywright pw, Settings settings)
    {
        return await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = settings.Headless,
            SlowMo = settings.SlowMoMs
        });
    }
}