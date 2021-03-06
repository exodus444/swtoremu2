﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GomLib
{
    public enum Endianness
    {
        LittleEndian,
        BigEndian
    }

    public class GomBinaryReader : BinaryReader
    {
        public GomBinaryReader(Stream str) : base(str) { }
        public GomBinaryReader(Stream str, Encoding encoding) : base(str, encoding) { }

        public ulong ReadNumber()
        {
            ulong val = 0;
            byte b0 = this.ReadByte();
            if (b0 < 0xC0) { return (ulong)b0; }
            if (b0 < 0xC8)
            {
                int numBytes = b0 - 0xBF;
                for (var i = 0; i < numBytes; i++)
                {
                    val <<= 8;
                    val = val + this.ReadByte();
                }
            }
            else if (b0 < 0xD0)
            {
                int numBytes = b0 - 0xC7;
                for (var i = 0; i < numBytes; i++)
                {
                    val <<= 8;
                    val = val + this.ReadByte();
                }
            }
            else if (b0 == 0xD2)
            {
                return (ulong)this.ReadByte();
            }
            else
            {
                throw new InvalidOperationException(String.Format("Unknown Number Prefix: 0x{0:X}", b0));
            }

            //if (val == 16141034714338427555)
            //{
            //    Console.WriteLine("Gotcha! ulong");
            //}

            return val;
        }

        public long ReadSignedNumber()
        {
            long val = 0;
            byte b0 = this.ReadByte();
            if (b0 < 0xC0) { return (long)b0; }
            if (b0 < 0xC8)
            {
                int numBytes = b0 - 0xBF;
                for (var i = 0; i < numBytes; i++)
                {
                    val <<= 8;
                    val = val | this.ReadByte();
                }
                val = -val;
            }
            else if (b0 < 0xD0)
            {
                int numBytes = b0 - 0xC7;
                for (var i = 0; i < numBytes; i++)
                {
                    val <<= 8;
                    val = val | this.ReadByte();
                }
            }
            else if (b0 == 0xD2)
            {
                return (long)this.ReadByte();
            }
            else
            {
                throw new InvalidOperationException(String.Format("Unknown Number Prefix: 0x{0:X}", b0));
            }

            //if (val == -2305634256081214423)
            //{
            //    Console.WriteLine("Gotcha! long");
            //}

            return val;
        }

        public TypedValue ReadTypedValue()
        {
            TypedValueType valType = (TypedValueType)this.ReadByte();
            TypedValue result = new TypedValue(valType);
            result.Parse(this);

            return result;
        }

        public UInt64 ReadVariableWidthUInt64(int lengthInBytes)
        {
            if ((lengthInBytes < 1) || (lengthInBytes > 8))
            {
                throw new ArgumentOutOfRangeException("lengthInBytes", "Length in bytes must be between 1 and 8");
            }

            UInt64 result = 0;

            byte[] bytes = this.ReadBytes(lengthInBytes);
            int shiftAmt = lengthInBytes - 1;
            for (var i = 0; i < lengthInBytes; i++)
            {
                UInt64 val = bytes[i];
                val <<= (8 * shiftAmt);
                shiftAmt--;
                result += val;
            }

            return result;
        }

        public GomType ReadGomType()
        {
            return GomTypeLoader.Load(this);
        }

        public string ReadNullTerminatedString()
        {
            return this.ReadNullTerminatedString(Encoding.UTF8);
        }

        public string ReadNullTerminatedString(Encoding encoding)
        {
            List<byte> byteBuffer = new List<byte>();
            byte b = this.ReadByte();
            // Read until we encounter a null byte
            while (b != 0)
            {
                byteBuffer.Add(b);
                b = this.ReadByte();
            }

            return encoding.GetString(byteBuffer.ToArray());
        }

        public short ReadInt16(Endianness endianness)
        {
            short val = base.ReadInt16();
            if (endianness == Endianness.LittleEndian) { return val; }
            else { return System.Net.IPAddress.NetworkToHostOrder(val); }
        }

        public ushort ReadUInt16(Endianness endianness)
        {
            if (endianness == Endianness.LittleEndian) { return base.ReadUInt16(); }

            byte[] b = base.ReadBytes(2);
            return BitConverter.ToUInt16(b.Reverse().ToArray(), 0);
        }

        public int ReadInt32(Endianness endianness)
        {
            int val = base.ReadInt32();

            if (endianness == Endianness.LittleEndian) { return val; }
            else { return System.Net.IPAddress.NetworkToHostOrder(val); }
        }

        public uint ReadUInt32(Endianness endianness)
        {
            if (endianness == Endianness.LittleEndian) { return base.ReadUInt32(); }

            byte[] b = base.ReadBytes(4);
            return BitConverter.ToUInt32(b.Reverse().ToArray(), 0);
        }

        public float ReadSingle(Endianness endianness)
        {
            if (endianness == Endianness.LittleEndian) { return base.ReadSingle(); }

            byte[] b = base.ReadBytes(4);
            return BitConverter.ToSingle(b.Reverse().ToArray(), 0);
        }

        public string ReadLengthPrefixString()
        {
            return ReadLengthPrefixString(Encoding.UTF8);
        }

        public string ReadLengthPrefixString(Encoding encoding)
        {
            int len = (int)this.ReadNumber();
            return ReadFixedLengthString(len, encoding);
        }

        public string ReadFixedLengthString(int length)
        {
            return this.ReadFixedLengthString(length, Encoding.UTF8);
        }

        public string ReadFixedLengthString(int length, Encoding encoding)
        {
            byte[] buff = this.ReadBytes(length);
            string result = encoding.GetString(buff);
            //if (result.Equals("plc.location.tatooine.item.treasure_chest.chest", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    Console.WriteLine("Gotcha! string");
            //}
            return result;
        }
    }
}
