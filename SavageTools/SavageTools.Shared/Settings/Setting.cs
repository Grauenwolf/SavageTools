namespace SavageTools.Settings
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Setting
    {

        private SettingSkill[] skillsField;

        private SettingHindrance[] hindrancesField;

        private SettingEdge[] edgesField;

        private SettingArchetype[] archetypesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Skill", IsNullable = false)]
        public SettingSkill[] Skills
        {
            get
            {
                return this.skillsField;
            }
            set
            {
                this.skillsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Hindrance", IsNullable = false)]
        public SettingHindrance[] Hindrances
        {
            get
            {
                return this.hindrancesField;
            }
            set
            {
                this.hindrancesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Edge", IsNullable = false)]
        public SettingEdge[] Edges
        {
            get
            {
                return this.edgesField;
            }
            set
            {
                this.edgesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Archetype", IsNullable = false)]
        public SettingArchetype[] Archetypes
        {
            get
            {
                return this.archetypesField;
            }
            set
            {
                this.archetypesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingSkill
    {

        private string nameField;

        private string attributeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingHindrance
    {

        private string nameField;

        private string typeField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingEdge
    {

        private SettingEdgeSkill[] skillField;

        private SettingEdgeTrait[] traitField;

        private SettingEdgeFeature[] featureField;

        private string nameField;

        private string requiresField;

        private string descriptionField;

        private string uniqueGroupField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Skill")]
        public SettingEdgeSkill[] Skill
        {
            get
            {
                return this.skillField;
            }
            set
            {
                this.skillField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Trait")]
        public SettingEdgeTrait[] Trait
        {
            get
            {
                return this.traitField;
            }
            set
            {
                this.traitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Feature")]
        public SettingEdgeFeature[] Feature
        {
            get
            {
                return this.featureField;
            }
            set
            {
                this.featureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Requires
        {
            get
            {
                return this.requiresField;
            }
            set
            {
                this.requiresField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UniqueGroup
        {
            get
            {
                return this.uniqueGroupField;
            }
            set
            {
                this.uniqueGroupField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingEdgeSkill
    {

        private string nameField;

        private string attributeField;

        private byte levelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingEdgeTrait
    {

        private string nameField;

        private sbyte bonusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public sbyte Bonus
        {
            get
            {
                return this.bonusField;
            }
            set
            {
                this.bonusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingEdgeFeature
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingArchetype
    {

        private string nameField;

        private byte unusedAttributesField;

        private byte unusedSkillsField;

        private byte unusedEdgesField;

        private byte experienceField;

        private bool experienceFieldSpecified;

        private byte unusedAdvancesField;

        private bool unusedAdvancesFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte UnusedAttributes
        {
            get
            {
                return this.unusedAttributesField;
            }
            set
            {
                this.unusedAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte UnusedSkills
        {
            get
            {
                return this.unusedSkillsField;
            }
            set
            {
                this.unusedSkillsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte UnusedEdges
        {
            get
            {
                return this.unusedEdgesField;
            }
            set
            {
                this.unusedEdgesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Experience
        {
            get
            {
                return this.experienceField;
            }
            set
            {
                this.experienceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExperienceSpecified
        {
            get
            {
                return this.experienceFieldSpecified;
            }
            set
            {
                this.experienceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte UnusedAdvances
        {
            get
            {
                return this.unusedAdvancesField;
            }
            set
            {
                this.unusedAdvancesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnusedAdvancesSpecified
        {
            get
            {
                return this.unusedAdvancesFieldSpecified;
            }
            set
            {
                this.unusedAdvancesFieldSpecified = value;
            }
        }
    }



}
