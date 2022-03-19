using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;
using PdfUtility;
using PdfUtility.Business;
using Reactive.Bindings;

namespace MakePdfPlugin
{
    //ドラッグ&ドロップの実装
    //https://qiita.com/tomboyboy/items/cf58a1d5cbe6cd5b3155
    public class MakePdfPanelViewModel : IDropTarget
    {
        public ObservableCollection<Keyword> Keywords { get;  set; }
        public ObservableCollection<TargetPath> FilePaths { get; }
        public bool MergePdf { get; set; }
        public string MergedPdfName { get; set; }
        public ReactiveCommand MakePdfCmd { get; set; }
        public ReactiveCommand OpenPrinterAndDeviceCmd { get; set; }

        private string outputDirectoryPath;
        private string tempDirectoryPath;

        public MakePdfPanelViewModel(
            string outputDirectoryPath,
            string tempDirectoryPath)
        {
            FilePaths = new ObservableCollection<TargetPath>();
            Keywords = new ObservableCollection<Keyword>();
            MakePdfCmd = new ReactiveCommand();
            MakePdfCmd.Subscribe(() =>
            {
                try
                {
                    var createPdfService = new CreatePdfService(tempDirectoryPath);
                    var paths = CollectionViewSource.GetDefaultView(FilePaths).Cast<TargetPath>().Select(f => f.FilePath).ToList();
                    var keywords = Keywords.Where(x => x.Word != "").Select(x => x.Word).ToList();
                    createPdfService.CreatePdf(paths.Select(x => new CreatePdfTarget(x, "", true)).ToList(), keywords, outputDirectoryPath, MergePdf ? MergedPdfName : null);
                    MessageBox.Show("完了!");
                    System.Diagnostics.Process.Start(outputDirectoryPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("エラー:\n" + ex.Message);
                }
            });

            OpenPrinterAndDeviceCmd = new ReactiveCommand();
            OpenPrinterAndDeviceCmd.Subscribe(() =>
            {
                System.Diagnostics.Process.Start("control.exe", "/name Microsoft.DevicesAndPrinters");
            });

            this.outputDirectoryPath = outputDirectoryPath;
            this.tempDirectoryPath = tempDirectoryPath;
            if (!Directory.Exists(outputDirectoryPath))
                Directory.CreateDirectory(outputDirectoryPath);
        }
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public void Drop(IDropInfo dropInfo)
        {

            var paths = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

            foreach (var path in paths)
            {
                if(File.Exists(path))
                {
                    AddFile(path);
                }
                else if (Directory.Exists(path))
                {
                    foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                        AddFile(file);
                }
            }
        }

        private void AddFile(string path)
        {
            if(TargetPath.IsTargetFile(path))
            {
                FilePaths.Add(new TargetPath(path));
            }
        }
    }
    public class Keyword
    {
        public string Word { get; set; } = "";
    } 

    public class TargetPath
    {
        public static string[] targetExtensions = { ".doc", ".docx", ".xls", ".xlsx", ".pdf" }; 
        static public bool IsTargetFile(string path) { return targetExtensions.Any(e => e.Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase)); }
        public string FilePath { get; }
        public string DirectoryPath { get { return Path.GetDirectoryName(FilePath); } }
        public string FileName { get { return Path.GetFileName(FilePath); } }
        public TargetPath(string path) { this.FilePath = path; }
    }
}
