using Microsoft.Playwright;
using System.Reflection.Metadata.Ecma335;

namespace OrangeHrm.Automation.Pages.Components
{
    public sealed class AdminTopNavComponent
    {
        private readonly IPage _page;

        public AdminTopNavComponent(IPage page)
        {
            _page = page;
        }

        private ILocator JobMenuTab => 
            _page.GetByRole(AriaRole.Listitem).Filter( new() { HasText = "Job" });

        private ILocator JobTitleMenuItem =>
            _page.GetByRole(AriaRole.Menuitem, new() { Name = "Job Titles" });

        public async Task OpenJobTitlesAsync()
        {
            await JobMenuTab.ClickAsync();
            await JobTitleMenuItem.ClickAsync();
        }

    }
}
