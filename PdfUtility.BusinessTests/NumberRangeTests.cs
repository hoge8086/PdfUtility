using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfUtility.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfUtility.Business.Tests
{
    [TestClass()]
    public class NumberRangeTests
    {

        [TestMethod()]
        public void GetIntListTest()
        {
            var a = NumberRange.Parse("1").GetNumberList(11);
            var b = NumberRange.Parse("$1").GetNumberList(11);
            var c = NumberRange.Parse("1-10").GetNumberList(11);
            var d = NumberRange.Parse("1-").GetNumberList(11);
            var e = NumberRange.Parse("$2-").GetNumberList(11);
            var f = NumberRange.Parse("2-$3").GetNumberList(11);
            //var a = new NumberRange(-2, -1).GetNumberList(5);
            //var b = new NumberRange(5, null).GetNumberList(5);
            //var c = new NumberRange(3, -10).GetNumberList(22);
            //var e = new NumberRange(-2, null).GetNumberList(22);
            //var d = new NumberRange(14, -10).GetNumberList(22);
        }

    }
}