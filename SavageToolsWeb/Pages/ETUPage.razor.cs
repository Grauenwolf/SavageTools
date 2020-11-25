using Microsoft.AspNetCore.Components.Web;
using SavageTools.Utilities;
using System.Collections.Generic;

namespace SavageToolsWeb.Pages
{
    partial class ETUPage
    {
        protected override string SettingFileName => "ETU.savage-setting";

        protected void CreateAdventure()
        {
            var gen = new EtuAdventureGenerator();
            Adventures.Add(gen.GenerateAdventure());
            StateHasChanged();
        }

        protected List<EtuAdventure> Adventures { get; } = new List<EtuAdventure>();

        void ClearAdventures(MouseEventArgs args)
        {
            Adventures.Clear();
            StateHasChanged();
        }
    }
}
