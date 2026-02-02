using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace OrangeHrm.Automation.Pages
{
    public sealed class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) => _page = page;

        private ILocator UsernameInput => _page.Locator("input[name='username']");
        private ILocator PasswordInput => _page.Locator("input[name='password']");
        private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });

        public async Task OpenAsync()
        {
            await _page.GotoAsync("/web/index.php/auth/login");
        }

        public async Task<(string username, string password)> ReadDemoCredentialsAsync()
        {
            var bodyText = await _page.Locator("body").InnerTextAsync();
            var userMatch = Regex.Match(bodyText, @"Username\s*:\s*(\S+)", RegexOptions.IgnoreCase);
            var passMatch = Regex.Match(bodyText, @"Password\s*:\s*(\S+)", RegexOptions.IgnoreCase);

            if (!userMatch.Success || !passMatch.Success)
                throw new InvalidOperationException("Failed to read demo credentials from login page");

            return (userMatch.Groups[1].Value.Trim(), passMatch.Groups[1].Value.Trim());
        }

            public async Task LoginWithDemoCredentialsAsync()
            {
                var (username, password) = await ReadDemoCredentialsAsync();

                await UsernameInput.FillAsync(username);
                await PasswordInput.FillAsync(password);
                await LoginButton.ClickAsync();

            }     
    }
}

