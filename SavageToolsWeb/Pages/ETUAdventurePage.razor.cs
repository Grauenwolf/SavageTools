using Microsoft.AspNetCore.Components.Web;
using SavageTools;
using SavageTools.Utilities;
using System;

namespace SavageToolsWeb.Pages
{
    partial class ETUAdventurePage
    {
        protected override string SettingFileName => "ETU.savage-setting";
        public string? ErrorDisplay;

        protected void CreateAdventure()
        {
            try
            {
                var gen = new EtuAdventureGenerator(Model!.CharacterGenerator);
                Adventure = gen.GenerateAdventure();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = ex.ToString();
            }
        }

        protected EtuAdventure? Adventure = null;

        protected NameDescriptionPair? HighStrangness = null;

        void ClearAll(MouseEventArgs args)
        {
            Adventure = null;
            HighStrangness = null;
            StateHasChanged();
        }

        void CreateHighStrangness(MouseEventArgs args)
        {
            try
            {
                var gen = new EtuAdventureGenerator(Model!.CharacterGenerator);

                HighStrangness = gen.HighStrangeness(new Dice());
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = ex.ToString();
            }
        }
    }
}
