using SavageTools;
using SavageTools.Utilities;

namespace SavageToolsWeb.Pages
{
    partial class ETUDemonPage
    {
        protected override string SettingFileName => "ETU.savage-setting";

        protected void CreateDemon()
        {
            var gen = new EtuDemonGenerator(Model!.CharacterGenerator);
            Model.Squad.Insert(0, gen.GenerateDemon(new Dice()));
            StateHasChanged();
        }
    }
}
