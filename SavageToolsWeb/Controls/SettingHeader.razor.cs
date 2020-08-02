using Microsoft.AspNetCore.Components;
using SavageTools.Characters;

namespace SavageToolsWeb.Controls
{
    partial class SettingHeader
    {
        [Parameter]
        public CharacterOptions CharacterOptions { get; set; } = null!;
    }
}
