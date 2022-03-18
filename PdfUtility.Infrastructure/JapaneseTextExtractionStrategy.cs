using iTextSharp.text.pdf.parser;

namespace PdfUtility.Infrastructure
{
    //http://blog.nsmr.me/2015/02/itextpdfpdf.html
    class JapaneseTextExtractionStrategy : LocationTextExtractionStrategy
    {
        protected override bool IsChunkAtWordBoundary(TextChunk chunk, TextChunk previousChunk)
        {
            return chunk.DistanceFromEndOf(previousChunk) > 10;
        }
    }
}
