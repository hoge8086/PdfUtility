﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PdfUtility
{
    interface IPdfConverter
    {
        void ConvertToPdf(string fromPath, string toPath);
        bool IsSupported(string filePath);
    }
}
