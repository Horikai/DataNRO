using System;
using System.Collections.Generic;

namespace DataNRO
{
    internal static class GameData
    {
        public class NpcTemplate
        {
            public int npcTemplateId, headId, bodyId, legId;
            public string name;
            public string[][] menu;
        }
        public class MobTemplate
        {
            public int mobTemplateId, rangeMove, speed, type, dartType, hp;
            public string name;
        }
        public class ItemOptionTemplate
        {
            public int id, type;
            public string name;
        }
        public class ItemTemplate
        {
            public bool isUpToUp;
            public int id, type, gender, level, strRequire, iconID, part;
            public string name, description;
        }
        public class NClass
        {
            public int classId;
            public string name;
            public SkillTemplate[] skillTemplates;
        }
        public class SkillTemplate
        {
            public int id, maxPoint, manaUseType, type, iconId;
            public string name, description, damInfo;
            public Skill[] skills;
        }
        public class Skill
        {
            public int point, maxFight, manaUse, skillId, dx, dy, damage, price, coolDown;
            public long powRequire;
            public string moreInfo;
        }

        internal static NpcTemplate[] NpcTemplates { get; set; }
        internal static MobTemplate[] MobTemplates { get; set; }
        internal static ItemOptionTemplate[] ItemOptionTemplates { get; set; }
        internal static NClass[] NClasses { get; set; }
        internal static Dictionary<int, string> Maps { get; set; } = new Dictionary<int, string>();
        internal static List<ItemTemplate> ItemTemplates { get; set; } = new List<ItemTemplate>();

        internal static void Reset()
        {
            NpcTemplates = null;
            MobTemplates = null;
            ItemOptionTemplates = null;
            NClasses = null;
            Maps = new Dictionary<int, string>();
            ItemTemplates = new List<ItemTemplate>();
        }
    }
}
