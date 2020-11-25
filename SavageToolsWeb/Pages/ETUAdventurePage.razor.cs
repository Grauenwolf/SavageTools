using Microsoft.AspNetCore.Components.Web;
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
                Adventures.Clear();
                Adventures.Add(gen.GenerateAdventure());
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = ex.ToString();
            }
        }

        protected List<EtuAdventure> Adventures { get; } = new List<EtuAdventure>();

        void ClearAdventures(MouseEventArgs args)
        {
            Adventures.Clear();
            StateHasChanged();
        }
    }
}
