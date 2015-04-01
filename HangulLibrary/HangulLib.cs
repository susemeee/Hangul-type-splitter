using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangulLibrary
{
    class Jamo
    {
        public static char[] choArr = {
                'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ',
                'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ',
                'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ',
                'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
                                    };

        public static char[] jungArr = {
                'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ',
                'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ',
                'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ',
                'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ',
                'ㅣ' 
                                   };
        public static char[] jongArr = {
                '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ',
                'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ',
                'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ',
                'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ',
                'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ',
                'ㅌ', 'ㅍ', 'ㅎ'
                                   };

        public int cho { get; set; }
        public int jung { get; set; }
        public int jong { get; set; }
    }

    class HangulLib
    {
        private const int beginCodeHan = 0xac00;
        private const int beginCodeCho = 0x1100;

        private static char TransHanChar(int cho, int? jung, int? jong)
        {
            int newCho = cho, newJung, newJong;

            if (jong == null)
            {
                newJong = 0;
            }
            else
            {
                newJong = (int)jong;
            }

            if (jung == null)
            {
                newJung = -1;
            }
            else
            {
                newJung = (int)jung;
            }

            if (newJung == -1 && newJong == 0)
            {
                return (char)(beginCodeCho + newCho);
            }
            return (char)(beginCodeHan + ((newCho * 21) + newJung) * 28 + newJong);
        }

        private static Jamo CharDivider(char uni)
        {
            int remainder = (int)(uni - beginCodeHan);

            Jamo hanCode = new Jamo();

            hanCode.jong = (char)(remainder % 28);
            hanCode.jung = (char)(((remainder - hanCode.jong) / 28) % 21);
            hanCode.cho = (char)((((remainder - hanCode.jong) / 28) - hanCode.jung) / 21);

            return hanCode;
        }

        private static char[] CharCombiner(Jamo c)
        {
            List<char> arr = new List<char>();

            arr.Add(TransHanChar(c.cho, null, null));

            if (c.jung > Array.IndexOf(Jamo.jungArr, 'ㅗ') && c.jung < Array.IndexOf(Jamo.jungArr, 'ㅛ'))
            {
                arr.Add(TransHanChar(c.cho, Array.IndexOf(Jamo.jungArr, 'ㅗ'), null));
            }
            else if (c.jung > Array.IndexOf(Jamo.jungArr, 'ㅜ') && c.jung < Array.IndexOf(Jamo.jungArr, 'ㅠ'))
            {
                arr.Add(TransHanChar(c.cho, Array.IndexOf(Jamo.jungArr, 'ㅜ'), null));
            }
            else if (c.jung > Array.IndexOf(Jamo.jungArr, 'ㅡ') && c.jung < Array.IndexOf(Jamo.jungArr, 'ㅣ'))
            {
                arr.Add(TransHanChar(c.cho, Array.IndexOf(Jamo.jungArr, 'ㅡ'), null));
            }
            arr.Add(TransHanChar(c.cho, c.jung, null));

            if (c.jong != (char)0)
            {
                if (c.jong > Array.IndexOf(Jamo.jongArr, 'ㄲ') && c.jong < Array.IndexOf(Jamo.jongArr, 'ㄴ'))
                {
                    arr.Add(TransHanChar(c.cho, c.jung, Array.IndexOf(Jamo.jongArr, 'ㄱ')));
                }
                else if (c.jong > Array.IndexOf(Jamo.jongArr, 'ㄴ') && c.jong < Array.IndexOf(Jamo.jongArr, 'ㄷ'))
                {
                    arr.Add(TransHanChar(c.cho, c.jung, Array.IndexOf(Jamo.jongArr, 'ㄴ')));
                }
                else if (c.jong > Array.IndexOf(Jamo.jongArr, 'ㄹ') && c.jong < Array.IndexOf(Jamo.jongArr, 'ㅁ'))
                {
                    arr.Add(TransHanChar(c.cho, c.jung, Array.IndexOf(Jamo.jongArr, 'ㄹ')));
                }
                else if (c.jong > Array.IndexOf(Jamo.jongArr, 'ㅂ') && c.jong < Array.IndexOf(Jamo.jongArr, 'ㅅ'))
                {
                    arr.Add(TransHanChar(c.cho, c.jung, Array.IndexOf(Jamo.jongArr, 'ㅂ')));
                }
                arr.Add(TransHanChar(c.cho, c.jung, c.jong));
            }

            return arr.ToArray();
        }

        public static char[] Run(char input)
        {
            if ((0xAC00 <= input && input <= 0xD7A3) || (0x3131 <= input && input <= 0x318E))
            {
                return CharCombiner(CharDivider(input));
            }
            else
            {
                return new char[] { input };
            }
        }

        public static string[] Run(string input)
        {
            string lastStr = "";
            List<string> l = new List<string>();
            string buffer = "";
            foreach (char c in input)
            {
                char[] rc = Run(c);
                foreach (char _rc in rc)
                {
                    if (String.Empty != lastStr)
                    {
                        buffer += lastStr;
                    }
                    buffer += _rc;

                    l.Add(buffer);
                    buffer = "";
                }
                lastStr += rc[rc.Length - 1].ToString();
            }
            return l.ToArray();
        }
    }
}
