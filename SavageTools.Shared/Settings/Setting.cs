using System.Xml.Serialization;
using Tortuga.Anchor;

#pragma warning disable RCS1139 // Add summary element to documentation comment.
#nullable disable

namespace SavageTools.Settings
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Setting
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string NamePrefix { get; set; }

        [XmlAttribute]
        public bool BornAHero { get; set; }

        [XmlAttribute]
        public bool MoreSkills { get; set; }

        [XmlAttribute]
        public bool UseStrain { get; set; }

        [XmlAttribute]
        public bool UseReason { get; set; }

        [XmlAttribute]
        public bool UseStatus { get; set; }

        //[XmlAttribute]
        //public bool ShowSetting { get; set; }

        /// <remarks/>
        [XmlArrayItem("Skill", IsNullable = false)]
        public SettingSkillOption[] Skills { get; set; }

        /// <remarks/>
        [XmlArrayItem("Skill", IsNullable = false)]
        public SettingRemoveSkill[] RemoveSkills { get; set; }

        /// <remarks/>
        [XmlArrayItem("Hindrance", IsNullable = false)]
        public SettingHindrance[] Hindrances { get; set; }

        /// <remarks/>
        [XmlArrayItem("Edge", IsNullable = false)]
        public SettingEdge[] Edges { get; set; }

        /// <remarks/>
        [XmlArrayItem("Archetype", IsNullable = false)]
        public SettingArchetype[] Archetypes { get; set; }

        /// <remarks/>
        [XmlArrayItem("Race", IsNullable = false)]
        public SettingRace[] Races { get; set; }

        /// <remarks/>
        [XmlArrayItem("Rank", IsNullable = false)]
        public SettingRank[] Ranks { get; set; }

        /// <remarks/>
        [XmlArrayItem("Reference", IsNullable = false)]
        public SettingReference[] References { get; set; }

        /// <remarks/>
        [XmlArrayItem("Power", IsNullable = false)]
        public SettingPower[] Powers { get; set; }

        /// <remarks/>
        [XmlArrayItem("Trapping", IsNullable = false)]
        public SettingTrapping[] Trappings { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingRemoveSkill
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingSkillOption
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Attribute { get; set; }

        [XmlAttribute()]
        public bool IsCore { get; set; }

        [XmlAttribute()]
        public bool NoAnimals { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingHindrance
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Description { get; set; }

        /// <remarks/>
        [XmlElement("Skill")]
        public SettingSkill[] Skill { get; set; }

        /// <remarks/>
        [XmlElement("Trait")]
        public SettingTrait[] Trait { get; set; }

        /// <remarks/>
        [XmlElement("Feature")]
        public SettingFeature[] Features { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingEdge
    {
        /// <remarks/>
        [XmlElement("Skill")]
        public SettingSkill[] Skills { get; set; }

        /// <remarks/>
        [XmlElement("Trait")]
        public SettingTrait[] Traits { get; set; }

        /// <remarks/>
        [XmlElement("Feature")]
        public SettingFeature[] Features { get; set; }

        /// <remarks/>
        [XmlElement("Edge")]
        public SettingEdge[] Edges { get; set; }

        /// <remarks/>
        [XmlElement("Hindrance")]
        public SettingHindrance[] Hindrances { get; set; }

        /// <remarks/>
        [XmlElement("AvailablePower")]
        public SettingAvailablePower[] AvailablePowers { get; set; }

        [XmlElement("Power")]
        public SettingPower[] Powers { get; set; }

        /// <remarks/>
        [XmlElement("Trigger")]
        public SettingTrigger[] Triggers { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Requires { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Description { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string UniqueGroup { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PowerType { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingPower
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Requires { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PowerType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Description { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Trapping { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Trigger { get; set; }

        /// <remarks/>
        [XmlAttribute("PowerPoints")]
        public string PowerPointsString { get; set; }

        public int? PowerPoints => !PowerPointsString.IsNullOrEmpty() ? (int?)int.Parse(PowerPointsString) : null;
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingTrapping
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingSkill
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Attribute { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Level { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int MaxLevelBonus { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingFeature
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingGear
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Description { get; set; }

        /// <remarks/>
        [XmlElement("Trait")]
        public SettingTrait[] Traits { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingAvailablePower
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingTrigger
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingArchetype
    {
        /// <remarks/>
        [XmlElement("Skill")]
        public SettingSkill[] Skills { get; set; }

        /// <remarks/>
        [XmlElement("Trait")]
        public SettingTrait[] Traits { get; set; }

        /// <remarks/>
        [XmlElement("Edge")]
        public SettingEdge[] Edges { get; set; }

        [XmlArrayItem("Edge", IsNullable = false)]
        public SettingEdge[] IconicEdges { get; set; }

        /// <remarks/>
        [XmlElement("Hindrance")]
        public SettingHindrance[] Hindrances { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool WildCard { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int UnusedAttributes { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int UnusedSkills { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Agility { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Smarts { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Spirit { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Strength { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Vigor { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool IsWildCard { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Race { get; set; }

        /// <remarks/>
        [XmlElement("Feature")]
        public SettingFeature[] Features { get; set; }

        /// <remarks/>
        [XmlElement("Gear")]
        public SettingGear[] Gear { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingTrait
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Bonus { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class SettingRace
    {
        /// <remarks/>
        [XmlElement("Skill")]
        public SettingSkill[] Skills { get; set; }

        /// <remarks/>
        [XmlElement("Hindrance")]
        public SettingHindrance[] Hindrances { get; set; }

        /// <remarks/>
        [XmlElement("Trait")]
        public SettingTrait[] Traits { get; set; }

        /// <remarks/>
        [XmlElement("Edge")]
        public SettingEdge[] Edges { get; set; }

        [XmlArrayItem("Edge", IsNullable = false)]
        public SettingEdge[] RacialEdges { get; set; }

        /// <remarks/>
        [XmlElement("Power")]
        public SettingPower[] Powers { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool AnimalIntelligence { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool IsLimited { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingRank
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        [XmlAttribute()]
        public string DisplayName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int Experience { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public int UnusedAdvances { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class SettingReference
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }
    }
}
