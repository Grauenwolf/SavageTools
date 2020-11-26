using Microsoft.AspNetCore.Components;
using SavageTools;
using SavageTools.Characters;
using System;

namespace SavageToolsWeb.Controls
{
    partial class CharacterBuilderDisplay
    {
        CharacterOptions? options;

        [Parameter]
        public CharacterOptions? Options
        {
            get => options;
            set
            {
                if (options != null)
                {
                    options.PropertyChanged -= Options_PropertyChanged;
                    options.Squad.CollectionChanged -= Squad_CollectionChanged;
                }
                options = value;
                if (options != null)
                {
                    options.PropertyChanged += Options_PropertyChanged;
                    options.Squad.CollectionChanged += Squad_CollectionChanged;
                }
            }
        }

        private void Squad_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StateHasChanged();
        }

        private void Options_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        public string? ErrorDisplay;

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
            }
            catch (Exception ex)
            {
                ErrorDisplay = "Unable to create character: " + ex.Message;
            }
        }
    }
}
