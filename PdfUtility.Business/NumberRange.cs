using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PdfUtility.Business
{
    /// <summary>
    /// 数値範囲（0以上の数値のみ対応）
    /// ※マイナスは最大値から後ろに数えて何番目を意味する。(-1は最大値)
    /// ※単一の値のみを示す場合もある(begin==end)
    /// </summary>
    public class NumberRange
    {
        // MEMO:後ろ(上限)から数えて何番目を表現する場合は、マイナスの値が入る
        private int begin;
        private int? end;

        public NumberRange(int single)
        {
            begin = single;
            end = single;
        }
        public NumberRange(int begin, int? end)
        {
            if (begin < 0 && end > 0)
                throw new ArgumentException($"数値範囲の指定が不正です。");

            if (((begin > 0 && end > 0) || (begin < 0 && end < 0)) && (begin > end))
                throw new Exception($"数値範囲の開始値が終了値より大きいです。");

            this.begin = begin;
            this.end = end;
        }

        public List<int> GetNumberList(int max)
        {
            // 例：range=100
            // "1"-> [1]、"$2"->[99]、 "3-"->[3～100]、  "$4-"->[97～100]、  "5-6"->[5,6]、  "$8-$7"->[93,94]、  "9-$10"->[9～91]
            int realBegin = begin > 0 ? begin : (begin + max + 1);
            int realEnd = end ?? max;
            if (realEnd < 0)
                realEnd += max + 1;

            if (realBegin > realEnd)
                throw new Exception($"数値範囲の開始値が終了値より大きいです。");

            if(realBegin < 1  ||  max < realBegin || realEnd < 1  ||  max < realEnd)
                throw new ArgumentException($"範囲外の数値を取得しようとしました。");

            return Enumerable.Range(realBegin, realEnd - realBegin + 1).ToList();
        }

        static public NumberRange Parse(string range)
        {
            Regex reg = new Regex(@"^(?<reverse_begin>\$)?(?<begin>\d+)(?<end_range>\s*-\s*(?<reverse_end>\$)?(?<end>\d+)?)?$");
            Match match = reg.Match(range);
            if (match.Success == false)
                throw new ArgumentException($"数値範囲のフォーマットが正しくありません。({range})");

            var begin = match.Groups["begin"].Value;
            var beginVal = int.Parse(begin) * (string.IsNullOrEmpty(match.Groups["reverse_begin"].Value) ? 1 : - 1);
            if (string.IsNullOrEmpty(match.Groups["end_range"].Value))
                return new NumberRange(beginVal);

            var end = match.Groups["end"].Value;
            if (string.IsNullOrEmpty(end))
                return new NumberRange(beginVal, null);

            var endVal = int.Parse(end) * (string.IsNullOrEmpty(match.Groups["reverse_end"].Value) ? 1 : - 1);
            return new NumberRange(beginVal, endVal);
        }
    }
}
