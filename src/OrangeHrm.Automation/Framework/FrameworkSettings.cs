using Microsoft.Extentions.Configuration;

namespace OrangeHrm.Automation.Framework;

public sealed class Settings
{
    public string BaseUrl { get; init; } = "";
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
    public bool Headless{ get; init; } = true;
    public int TimeoutMs { get; init; } = 30000;
    public int SlowMoMs { get; init; } = 0;
    public bool TracesOnFailure { get; init; } = true;


    public static Settings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("settings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "E2E_")
            .Build();

        return config.Get<Settings>() ?? new Settings();
    }