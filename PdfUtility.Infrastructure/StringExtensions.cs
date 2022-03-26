using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <https://kan-kikuchi.hatenablog.com/entry/CountOf>
namespace PdfUtility.Infrastructure
{
    /// <summary>
    /// string 型の拡張メソッドを管理するクラス
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 指定した文字列がいくつあるか
        /// </summary>
        public static int CountOf(this string self, params string[] strArray)
        {
            int count = 0;

            foreach (string str in strArray)
            {
                int index = self.IndexOf (str, 0);

                while (index != -1)
                {
                    count++;
                    index = self.IndexOf (str, index + str.Length);
                }
            }

            return count;
        }
    } 
}

