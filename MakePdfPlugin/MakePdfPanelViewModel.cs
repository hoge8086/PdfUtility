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
using PdfMaker;

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
        public MakePdfCommand MakePdfCmd { get; set; }
        public OpenPrinterAndDeviceCommand OpenPrinterAndDeviceCmd { get; set; }

        private string outputDirectoryPath;
        private string tempDirectoryPath;

        public class OpenPrinterAndDeviceCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter) { return true; }
            public void Execute(object parameter){ System.Diagnostics.Process.Start("control.exe", "/name Microsoft.DevicesAndPrinters");}
        }
        public class MakePdfCommand : ICommand
        {
            MakePdfPanelViewModel viewModel;
            public MakePdfCommand(MakePdfPanelViewModel viewModel) { this.viewModel = viewModel; }
            public bool CanExecute(object parameter) { return true; }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                try
                {
                    var paths = CollectionViewSource.GetDefaultView(viewModel.FilePaths).Cast<TargetPath>().Select(f => f.FilePath).ToList();
                    var pdfmaker = new BatchPdfMaker(viewModel.tempDirectoryPath);
                    var keywords = viewModel.Keywords.Where(x => x.Word != "").Select(x => x.Word).ToList();
                    if(viewModel.MergePdf)
                        pdfmaker.MakePdfAndMerge(paths, Path.Combine(viewModel.outputDirectoryPath, viewModel.MergedPdfName), keywords);
                    else
                        pdfmaker.MakePdfs(paths, viewModel.outputDirectoryPath, keywords);
                    MessageBox.Show("完了!");
                    System.Diagnostics.Process.Start(viewModel.outputDirectoryPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("エラー:\n" + ex.Message);
                }
            }
        }
        public MakePdfPanelViewModel(
            string outputDirectoryPath,
            string tempDirectoryPath)
        {
            FilePaths = new ObservableCollection<TargetPath>();
            Keywords = new ObservableCollection<Keyword>();
            MakePdfCmd = new MakePdfCommand(this);
            OpenPrinterAndDeviceCmd = new OpenPrinterAndDeviceCommand();
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
