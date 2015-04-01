using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangulLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(HangulLib.Run('낣'));
            string[] str = HangulLib.Run("강남APPLE어학원");
            foreach (string s in str)
            {
                Console.WriteLine(s);
            }
        }
    }
}
