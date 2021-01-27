using Microsoft.AspNetCore.Components.Web;
using SavageTools;
using SavageTools.Utilities;
using System;
using System.Collections.Generic;

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

        protected NameDescriptionPair? HighStrangeness = null;
        protected List<NameDescriptionPair>? RitualComponents = null;

        void ClearAll(MouseEventArgs args)
        {
            Adventure = null;
            HighStrangeness = null;
            StateHasChanged();
        }

        void CreateRitual(MouseEventArgs args)
        {
            try
            {
                var gen = new EtuAdventureGenerator(Model!.CharacterGenerator);

                RitualComponents = gen.CreateRitual(new Dice(), 0);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = ex.ToString();
            }
        }

        void CreateHighStrangness(MouseEventArgs args)
        {
            try
            {
                var gen = new EtuAdventureGenerator(Model!.CharacterGenerator);

                HighStrangeness = gen.HighStrangeness(new Dice());
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = ex.ToString();
            }
        }
    }
}
