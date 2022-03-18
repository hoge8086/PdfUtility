using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfUtility
{
    public class ExcelToPdf : IPdfMaker
    {
        public void makePdf(string fromPath, string toPath)
        {
            Application application = null;
            Workbooks workbooks = null;
            Workbook workbook = null;

            try {
                application = new Microsoft.Office.Interop.Excel.Application();
                application.DisplayAlerts = false;
                application.Visible = false;

                /*
                 * application.Workbooks.Open(...は、Workbooksオブジェクトの解放処理ができないので不可。
                 * 必ず変数経由でComRelease.FinalReleaseComObjectsを呼び出すこと。
                 */
                workbooks = application.Workbooks;

                workbook = workbooks.Open(
                    fromPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing
                    , Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing
                    , Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // http://msdn.microsoft.com/ja-jp/library/microsoft.office.tools.excel.workbook.exportasfixedformat(v=vs.90).aspx
                workbook.ExportAsFixedFormat(
                    XlFixedFormatType.xlTypePDF,
                    toPath,
                    XlFixedFormatQuality.xlQualityStandard,
                    false,
                    false,  //設定された印刷範囲で印刷する
                    Type.Missing,
                    Type.Missing,
                    false,
                    Type.Missing);
                workbook.Close(false, Type.Missing, Type.Missing);
            } finally {
                if (workbook != null) {
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (workbooks != null) {
                    Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }

                if (application != null) {
                    GC.Collect();
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }
                GC.Collect();
            }
        }
        public bool isSupported(string filePath)
        {
            var supportedExtensions = new string[]{ ".xls", ".xlsx" };
            string extension = Path.GetExtension(filePath);
            return supportedExtensions.Contains(extension);
        }
    }

}
