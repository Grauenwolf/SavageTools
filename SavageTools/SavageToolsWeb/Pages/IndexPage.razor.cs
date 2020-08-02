using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;

namespace SavageToolsWeb.Pages
{
    partial class IndexPage
    {
        [Inject] IWebHostEnvironment Environment { get; set; } = null!;
    }
}
