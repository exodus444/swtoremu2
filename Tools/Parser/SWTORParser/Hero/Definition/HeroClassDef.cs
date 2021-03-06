using System;
using System.Collections.Generic;
using System.IO;

namespace SWTORParser.Hero.Definition
{
    public class HeroClassDef : HeroDefinition
    {
        public List<DefinitionId> Fields;
        public List<DefinitionId> ParentClasses;
        public DefinitionId vers2_18;
        public DefinitionId vers2_20;

        public HeroClassDef(byte[] data, int version)
            : base(data, version)
        {
            ParentClasses = new List<DefinitionId>();
            Fields = new List<DefinitionId>();
            short num1;
            short num2;
            short num3;
            short num4;
            if (version == 1)
            {
                num1 = BitConverter.ToInt16(data, 22);
                num2 = BitConverter.ToInt16(data, 24);
                num3 = BitConverter.ToInt16(data, 26);
                num4 = BitConverter.ToInt16(data, 28);
            }
            else
            {
                if (version != 2)
                    throw new InvalidDataException("Invalid version");
                vers2_18 = new DefinitionId(BitConverter.ToUInt64(data, 24));
                vers2_20 = new DefinitionId(BitConverter.ToUInt64(data, 32));
                num1 = BitConverter.ToInt16(data, 42);
                num2 = BitConverter.ToInt16(data, 44);
                num3 = BitConverter.ToInt16(data, 46);
                num4 = BitConverter.ToInt16(data, 48);
            }
            for (int index = 0; index < (int) num1; ++index)
                ParentClasses.Add(new DefinitionId(BitConverter.ToUInt64(data, num2 + 8*index)));
            for (int index = 0; index < (int) num3; ++index)
                Fields.Add(new DefinitionId(BitConverter.ToUInt64(data, num4 + 8*index)));
        }

        public override string ToString()
        {
            return "Class " + Name;
        }

        public HeroFieldDef GetField(string name)
        {
            foreach (DefinitionId definitionId in Fields)
            {
                var heroFieldDef = definitionId.Definition as HeroFieldDef;
                if (heroFieldDef != null && heroFieldDef.Name == name)
                    return heroFieldDef;
            }
            foreach (DefinitionId definitionId in ParentClasses)
            {
                var heroClassDef = definitionId.Definition as HeroClassDef;
                if (heroClassDef != null)
                {
                    HeroFieldDef field = heroClassDef.GetField(name);
                    if (field != null)
                        return field;
                }
            }
            return null;
        }
    }
}