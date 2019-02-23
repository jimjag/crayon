using Interpreter.Structs;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter.Libraries.TextEncoding
{
    public static class LibraryWrapper
    {
        private static readonly int[] PST_IntBuffer16 = new int[16];
        private static readonly double[] PST_FloatBuffer16 = new double[16];
        private static readonly string[] PST_StringBuffer16 = new string[16];
        private static readonly System.Random PST_Random = new System.Random();

        public static bool AlwaysTrue() { return true; }
        public static bool AlwaysFalse() { return false; }

        public static string PST_StringReverse(string value)
        {
            if (value.Length < 2) return value;
            char[] chars = value.ToCharArray();
            return new string(chars.Reverse().ToArray());
        }

        private static readonly string[] PST_SplitSep = new string[1];
        private static string[] PST_StringSplit(string value, string sep)
        {
            if (sep.Length == 1) return value.Split(sep[0]);
            if (sep.Length == 0) return value.ToCharArray().Select<char, string>(c => "" + c).ToArray();
            PST_SplitSep[0] = sep;
            return value.Split(PST_SplitSep, System.StringSplitOptions.None);
        }

        private static string PST_FloatToString(double value)
        {
            string output = value.ToString();
            if (output[0] == '.') output = "0" + output;
            if (!output.Contains('.')) output += ".0";
            return output;
        }

        private static readonly System.DateTime PST_UnixEpoch = new System.DateTime(1970, 1, 1);
        private static double PST_CurrentTime
        {
            get { return System.DateTime.UtcNow.Subtract(PST_UnixEpoch).TotalSeconds; }
        }

        private static string PST_Base64ToString(string b64Value)
        {
            byte[] utf8Bytes = System.Convert.FromBase64String(b64Value);
            string value = System.Text.Encoding.UTF8.GetString(utf8Bytes);
            return value;
        }

        // TODO: use a model like parse float to avoid double parsing.
        public static bool PST_IsValidInteger(string value)
        {
            if (value.Length == 0) return false;
            char c = value[0];
            if (value.Length == 1) return c >= '0' && c <= '9';
            int length = value.Length;
            for (int i = c == '-' ? 1 : 0; i < length; ++i)
            {
                c = value[i];
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        public static void PST_ParseFloat(string strValue, double[] output)
        {
            double num = 0.0;
            output[0] = double.TryParse(strValue, out num) ? 1 : -1;
            output[1] = num;
        }

        private static List<T> PST_ListConcat<T>(List<T> a, List<T> b)
        {
            List<T> output = new List<T>(a.Count + b.Count);
            output.AddRange(a);
            output.AddRange(b);
            return output;
        }

        private static List<Value> PST_MultiplyList(List<Value> items, int times)
        {
            List<Value> output = new List<Value>(items.Count * times);
            while (times-- > 0) output.AddRange(items);
            return output;
        }

        private static bool PST_SubstringIsEqualTo(string haystack, int index, string needle)
        {
            int needleLength = needle.Length;
            if (index + needleLength > haystack.Length) return false;
            if (needleLength == 0) return true;
            if (haystack[index] != needle[0]) return false;
            if (needleLength == 1) return true;
            for (int i = 1; i < needleLength; ++i)
            {
                if (needle[i] != haystack[index + i]) return false;
            }
            return true;
        }

        private static void PST_ShuffleInPlace<T>(List<T> list)
        {
            if (list.Count < 2) return;
            int length = list.Count;
            int tIndex;
            T tValue;
            for (int i = length - 1; i >= 0; --i)
            {
                tIndex = PST_Random.Next(length);
                tValue = list[tIndex];
                list[tIndex] = list[i];
                list[i] = tValue;
            }
        }

        public static Value lib_textencoding_convertBytesToText(VmContext vm, Value[] args)
        {
            if ((args[0].type != 6))
            {
                return Interpreter.Vm.CrayonWrapper.buildInteger(vm.globals, 2);
            }
            ListImpl byteList = (ListImpl)args[0].internalValue;
            int format = (int)args[1].internalValue;
            ListImpl output = (ListImpl)args[2].internalValue;
            string[] strOut = PST_StringBuffer16;
            int length = byteList.size;
            int[] unwrappedBytes = new int[length];
            int i = 0;
            Value value = null;
            while ((i < length))
            {
                value = byteList.array[i];
                if ((value.type != 3))
                {
                    return Interpreter.Vm.CrayonWrapper.buildInteger(vm.globals, 3);
                }
                unwrappedBytes[i] = (int)value.internalValue;
                i += 1;
            }
            int sc = TextEncodingHelper.BytesToText(unwrappedBytes, format, strOut);
            if ((sc == 0))
            {
                Interpreter.Vm.CrayonWrapper.addToList(output, Interpreter.Vm.CrayonWrapper.buildString(vm.globals, strOut[0]));
            }
            return Interpreter.Vm.CrayonWrapper.buildInteger(vm.globals, sc);
        }

        public static Value lib_textencoding_convertTextToBytes(VmContext vm, Value[] args)
        {
            string value = (string)args[0].internalValue;
            int format = (int)args[1].internalValue;
            bool includeBom = (bool)args[2].internalValue;
            ListImpl output = (ListImpl)args[3].internalValue;
            List<Value> byteList = new List<Value>();
            int[] intOut = PST_IntBuffer16;
            int sc = TextEncodingHelper.TextToBytes(value, includeBom, format, byteList, vm.globals.positiveIntegers, intOut);
            int swapWordSize = intOut[0];
            if ((swapWordSize != 0))
            {
                int i = 0;
                int j = 0;
                int length = byteList.Count;
                Value swap = null;
                int half = (swapWordSize >> 1);
                int k = 0;
                while ((i < length))
                {
                    k = (i + swapWordSize - 1);
                    j = 0;
                    while ((j < half))
                    {
                        swap = byteList[(i + j)];
                        byteList[(i + j)] = byteList[(k - j)];
                        byteList[(k - j)] = swap;
                        j += 1;
                    }
                    i += swapWordSize;
                }
            }
            if ((sc == 0))
            {
                Interpreter.Vm.CrayonWrapper.addToList(output, Interpreter.Vm.CrayonWrapper.buildList(byteList));
            }
            return Interpreter.Vm.CrayonWrapper.buildInteger(vm.globals, sc);
        }
    }
}
