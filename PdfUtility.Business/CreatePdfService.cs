using PdfUtility.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System;

namespace PdfUtility.Business
{
    public class CreatePdfTarget
    {
        public string Path;
        public List<NumberRange> ExtractPageNumbers;
        public bool ExtractContainingKeywordPage;

        public CreatePdfTarget(string path, string pageNumbers, bool extractContainingKeywordPage)
        {
            Path = path;
            if(string.IsNullOrEmpty(pageNumbers))
                ExtractPageNumbers = null;
            else
                ExtractPageNumbers = pageNumbers.Split(',').Select(x => NumberRange.Parse( x.Trim())).ToList();

            ExtractContainingKeywordPage = extractContainingKeywordPage;
        }
    }

    public class ProessingFile
    {
        public string CurrentPath;
        public CreatePdfTarget Target;

        public ProessingFile(CreatePdfTarget target)
        {
            CurrentPath = target.Path;
            Target = target;
        }
    }

    public class CreatePdfService
    {
        private static readonly string PdfExtension = ".pdf";
        private TempDirectory tempDir;
        private PdfService pdfService;
        public CreatePdfService(string tempDirPath)
        {
            tempDir = new TempDirectory(tempDirPath);
            pdfService = new PdfService();
        }
        public void CreatePdf(List<CreatePdfTarget> createTargets, List<SearchTarget> keywords, string OutputDirectory,  string MargeFileName)
        {
            if (createTargets.Count < 1)
                throw new ArgumentException("PDF化対象のファイルがありません。");

            if (MargeFileName != null && !FileUtility.IsExtension(MargeFileName, PdfExtension))
                MargeFileName += PdfExtension;

            tempDir.DeleteAllFiles();
            try
            {
                var createdPdfList = createTargets.Select(x => new ProessingFile(x))
                    .Select(x => CopyToTempDir(x))
                    .Select(x => ConvertToPdf(x))
                    .Select(x => ExtractPageByPageNumbers(x))
                    .Select(x => ExtractPageByPageKeywords(x, keywords));

                if (MargeFileName == null)
                    MoveToOutputDirectory(OutputDirectory, createdPdfList);
                else
                    pdfService.Join(createdPdfList.Select(x => x.CurrentPath).ToList(), Path.Combine(OutputDirectory, MargeFileName));

            }finally
            {
                tempDir.DeleteAllFiles();
            }
        }

        private void MoveToOutputDirectory(string OutputDirectory, IEnumerable<ProessingFile> createdPdfList)
        {
            foreach (var x in createdPdfList)
            {
                var destPath = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(x.Target.Path) + PdfExtension);
                if (File.Exists(destPath))
                    File.Delete(destPath);

                File.Move(x.CurrentPath, destPath);
            }
        }

        private ProessingFile CopyToTempDir(ProessingFile file)
        {
            file.CurrentPath = tempDir.CopyFile(file.CurrentPath);
            return file;
        }

        private ProessingFile ConvertToPdf(ProessingFile file)
        {
            if (!FileUtility.IsExtension(file.CurrentPath, PdfExtension))
            {
                var outPath = Path.ChangeExtension(file.CurrentPath, PdfExtension);
                pdfService.ConvertToPdf(file.CurrentPath, outPath);
                file.CurrentPath = outPath;
            }
            return file;
        }
        private ProessingFile ExtractPageByPageNumbers(ProessingFile file)
        {
            if(file.Target.ExtractPageNumbers != null && file.Target.ExtractPageNumbers.Count > 0)
            {
                var outPath = Path.ChangeExtension(file.CurrentPath, ".page");
                var pageNum = pdfService.GetPages(file.CurrentPath).NumberOfPages;
                var extractPages = file.Target.ExtractPageNumbers.SelectMany(x => x.GetNumberList(pageNum)).Distinct().ToList();
                extractPages.Sort();
                pdfService.ExtractPages(extractPages, file.CurrentPath, outPath);
                file.CurrentPath = outPath;
            }
            return file;
        }
        private ProessingFile ExtractPageByPageKeywords(ProessingFile target, List<SearchTarget> keywords)
        {
            if(target.Target.ExtractContainingKeywordPage && keywords != null && keywords.Count > 0)
            {
                var outPath = Path.ChangeExtension(target.CurrentPath, ".keyword");
                var results =new SearchPdfService().Search(keywords, target.CurrentPath);
                var extractPages = results.SelectMany(t => t.Hits.Select(h => h.Page)).Distinct().ToList();
                extractPages.Sort();

                pdfService.ExtractPages(extractPages, target.CurrentPath, outPath);
                target.CurrentPath = outPath;
            }
            return target;
        }

    }
}
