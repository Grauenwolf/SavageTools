using Microsoft.AspNetCore.Components;
using SavageTools;
using SavageTools.Characters;
using System;

namespace SavageToolsWeb.Controls
{
    partial class CharacterBuilderForm
    {
        [Parameter] public CharacterOptions? Options { get; set; }

        public string? ErrorDisplay;
        public string DisplayMode = "HTML";

        protected void SubmitChanges()
        {
            //These should never happen.
            if (Options == null)
                return;

            ErrorDisplay = null;

            try
            {
                var dice = new Dice();
                for (var i = 0; i < Options.Count; i++)
                {
                    Options.Squad.Insert(0, Options.CharacterGenerator.GenerateCharacter(Options, dice));
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorDisplay = "Unable to create character: " + ex.Message;
            }
        }
    }
}
