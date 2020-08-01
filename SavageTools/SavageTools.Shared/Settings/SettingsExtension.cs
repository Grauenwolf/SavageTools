namespace SavageTools.Settings
{
    public static class SettingsExtension
    {
        public static bool IsMajor(this SettingHindrance hindrance)
        {
            return hindrance.Type == "Major" || hindrance.Type == "Minor/Major";
        }

        public static bool IsMinor(this SettingHindrance hindrance)
        {
            return hindrance.Type == "Minor" || hindrance.Type == "Minor/Major";
        }
    }
}
