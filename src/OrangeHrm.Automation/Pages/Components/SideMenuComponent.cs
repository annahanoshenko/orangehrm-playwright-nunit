using Microsoft.Playwright;


namespace OrangeHrm.Automation.Pages.Components
{
    public sealed class SideMenuComponent
    {
        private readonly IPage _page;

        public SideMenuComponent(IPage page) => _page = page;

        private ILocator AdminMenuItem => _page.GetByRole(AriaRole.Link, new() { Name = "Admin" });

        public async Task OpenAdminAsync()
        {
            await AdminMenuItem.ClickAsync();
        }
    }

}
