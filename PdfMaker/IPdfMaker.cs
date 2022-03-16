using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PdfMaker
{
    interface IPdfMaker
    {
        void makePdf(string fromPath, string toPath);
        bool isSupported(string filePath);
    }

}
