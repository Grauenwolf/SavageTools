using Microsoft.AspNetCore.Components;
using SavageTools;
using SavageTools.Characters;
using System;
using System.Collections.Generic;

namespace SavageToolsWeb.Controls
{
    partial class CharacterBuilderForm
    {
        [Parameter] public CharacterOptions? Options { get; set; }

        public List<Character> Squad = new List<Character>();
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
                    Squad.Insert(0, Options.CharacterGenerator.GenerateCharacter(Options, dice));
                }
            }
            catch (Exception ex)
            {
                ErrorDisplay = "Unable to create character: " + ex.Message;
            }
        }

        protected void OnClick()
        {
            StateHasChanged();
        }

        protected void OnKeyUp()
        {
            StateHasChanged();
        }
    }
}
