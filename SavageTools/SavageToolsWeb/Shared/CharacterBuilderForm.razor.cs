using Microsoft.AspNetCore.Components;
using SavageTools;
using SavageTools.Characters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SavageToolsWeb.Shared
{
    partial class CharacterBuilderForm
    {
        [Parameter] public CharacterOptions? Options { get; set; }

        public List<Character> Squad = new List<Character>();
        public string? ErrorDisplay;

        protected void SubmitChanges()
        {
            //These should never happen.
            if (Options == null)
                return;

            ErrorDisplay = null;

            try
            {
                var dice = new Dice(); //TODO: Capture seed
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
    }
}
