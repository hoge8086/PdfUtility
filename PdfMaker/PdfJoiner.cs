using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfMaker
{
    public class PdfJoiner
    {
        List<string> pdfPaths = new List<string>();

        public void Append(string pdfPath)
        {
            pdfPaths.Add(pdfPath);
        }

        public void JoinPdf(string toPdfPath)
        {
            Document objITextDoc = null;
            PdfCopy  objPDFCopy  = null;

            try
            {

                objITextDoc = new Document();
                objPDFCopy  = new PdfCopy( objITextDoc, new FileStream( toPdfPath, FileMode.Create ) );

                objITextDoc.Open();

                // 出力するPDFのプロパティを設定
                //objITextDoc.AddKeywords("キーワードです。");
                //objITextDoc.AddAuthor  ("zero0nine.com");
                //objITextDoc.AddTitle   ("結合したPDFファイルです。");
                //objITextDoc.AddCreator ("PDFファイル結合くん");
                //objITextDoc.AddSubject ("結合したPDFファイル");

                // ソートが必要ない場合は、コメント
                //sStrList.Sort();

                // 結合対象ファイル分ループ
                foreach (var fromPath in pdfPaths)
                {
                    PdfReader objPdfReader = new PdfReader(fromPath); // 結合元のPDFファイル読込
                    objPDFCopy.AddDocument(objPdfReader); // 結合元のPDFファイルを追加（全ページ）
                    objPdfReader.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("pdfの結合に失敗しました.\n" + ex.Message);
            }
            finally
            {
                if(objITextDoc != null)
                    objITextDoc.Close();

                if(objPDFCopy != null)
                    objPDFCopy.Close();
            }
        }
    }
}
