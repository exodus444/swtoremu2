﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GomLib.GomTypes
{
    public class Map : GomType
    {
        public Map() : base(GomTypeId.Map) { }

        public GomType KeyType { get; internal set; }
        public GomType ValueType { get; internal set; }

        internal override void Link()
        {
            KeyType.Link();
            ValueType.Link();
        }

        public override string ToString()
        {
            return System.String.Format("Map<{0}, {1}>", KeyType, ValueType);
        }

        public override bool ConfirmType(GomBinaryReader reader)
        {
            //return base.ConfirmType(reader) && KeyType.ConfirmType(reader) && ValueType.ConfirmType(reader);
            return base.ConfirmType(reader);
        }

        public override object ReadData(GomBinaryReader reader)
        {
            GomType keyType = GomTypeLoader.Load(reader, false);
            if ((KeyType != null) && (keyType.TypeId == KeyType.TypeId))
            {
                keyType = KeyType;
            }

            GomType valType = GomTypeLoader.Load(reader, false);
            if ((ValueType != null) && (valType.TypeId == ValueType.TypeId))
            {
                valType = ValueType;
            }

            int len = (int)reader.ReadNumber();
            int len2 = (int)reader.ReadNumber();
            if (len != len2)
            {
                throw new InvalidOperationException("Map length values aren't the same?!");
            }

            Dictionary<object, object> result = new Dictionary<object, object>(len);

            for (var i = 0; i < len; i++)
            {
                var key = keyType.ReadItem(reader);
                var val = valType.ReadItem(reader);
                result.Add(key, val);
            }

            return result;
        }
    }
}
