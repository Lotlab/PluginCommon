using System;

namespace Lotlab.PluginCommon.FFXIV.Parser
{
    public static class Util
    {
        /// <summary>
        /// Get generic type
        /// </summary>
        /// <see cref="https://stackoverflow.com/a/1075059"/>
        /// <param name="givenType"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static Type GetGenericBaseTypeOf(this Type givenType, Type genericType)
        {
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return givenType;

            Type baseType = givenType.BaseType;
            if (baseType == null) return null;

            return baseType.GetGenericBaseTypeOf(genericType);
        }

        /// <summary>
        /// Whether this type could assign to the generic type
        /// </summary>
        /// <param name="givenType"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            return givenType.GetGenericBaseTypeOf(genericType) != null;
        }

        public static string ToHexString(this byte[] barray)
        {
            char[] c = new char[barray.Length * 2];
            byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }

            return new string(c);
        }

        public static string GetUTF8String(this byte[] barray)
        {
            int strLen = barray.Length;
            for (int i = 0; i < strLen; i++)
            {
                if (barray[i] == 0)
                {
                    strLen = i;
                    break;
                }
            }

            return System.Text.UTF8Encoding.UTF8.GetString(barray, 0, strLen);
        }

        public static byte[] ToByteArray(this string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];
            Func<char, int> getHexVal = c => c - (c < 'A' ? '0' : (c < 'a' ? ('A' - 10) : ('a' - 10)));

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((getHexVal(hex[i << 1]) << 4) + (getHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
    }
}
