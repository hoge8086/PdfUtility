using PanelPluginInterface;
using PdfUtility.Infrastructure;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PdfUtility.Plugins
{
    class ExtractTextFromPdfPanelViewModel
    {
        public ReactiveProperty<int> DisplayPageIndex { get; }
        public ReactiveProperty<string> PdfFilePath { get; }
        //public AsyncReactiveCommand ReadPdfCommand { get; }
        public ObservableCollection<Page> Pages{ get; }
        private PdfService pdfService = new PdfService();

        public ExtractTextFromPdfPanelViewModel(IPluginHost host)
        {
            try
            {
                Pages = new ObservableCollection<Page>();
                DisplayPageIndex = new ReactiveProperty<int>();
                PdfFilePath = new ReactiveProperty<string>();
                //ReadPdfCommand = new AsyncReactiveCommand();
                //ReadPdfCommand.Subscribe(async () =>
                PdfFilePath.Subscribe(path =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(path))
                            return;

                        var pages = pdfService.GetPages(path);
                        Pages.Clear();
                        foreach (var page in pages.Pages)
                            Pages.Add(page);

                        DisplayPageIndex.Value = 0;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "エラー");
                    }

                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー");
            }
        }
    }
}
