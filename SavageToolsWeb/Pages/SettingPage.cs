using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using SavageTools.Characters;
using System.IO;

namespace SavageToolsWeb.Pages
{
    public abstract class SettingPage : PageBase<CharacterOptions>
    {
        [Inject] IWebHostEnvironment Environment { get; set; } = null!;

        protected abstract string SettingFileName { get; }

        protected override void ParametersSet()
        {
            if (Model == null)
            {
                var file = new FileInfo(Path.Combine(Environment.WebRootPath, "App_Data", SettingFileName));
                var cg = new CharacterGenerator(file);
                Model = new CharacterOptions(cg);
                PageTitle = Model.CharacterGenerator.SettingName;
            }
        }
    }
}
