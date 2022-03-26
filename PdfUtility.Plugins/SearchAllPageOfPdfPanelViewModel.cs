using System;
using System.Reactive;
using PanelPluginInterface;
using PdfUtility.Business;
using PdfUtility.Infrastructure;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PdfUtility.Plugins
{
    public class PageResult
    {
        public int Page { get; set; }
        public string Word { get; set; }
        public string Line { get; set; }
    }

    public class SearchAllPageOfPdfPanelViewModel
    {
        public IPluginHost Host;
        public ReactiveProperty<string> PdfFilePath { get; private set; }
        public ReactiveProperty<string> Keyword { get; private set; }
        public ObservableCollection<PageResult> Results { get; private set; }
        public bool EnableRegexp { get; set; }
        public ReactiveCommand SearchCommand { get; }
        public ReactiveCommand<PageResult> ShowPdfCommand { get; }
        public Action<string, int>  ShowPdf{ get; set; }

        private SearchPdfService searchPdfService = new SearchPdfService();
        private PdfService pdfService = new PdfService();

        public SearchAllPageOfPdfPanelViewModel(IPluginHost host)
        {
            Host = host;
            PdfFilePath = new ReactiveProperty<string>();
            Keyword = new ReactiveProperty<string>();
            EnableRegexp = false;
            Results = new ObservableCollection<PageResult>();
            SearchCommand = new ReactiveCommand();
            SearchCommand.Subscribe(() =>
            {
                try
                {
                    var target = new List<SearchTarget> { new SearchTarget(Keyword.Value, EnableRegexp, true) };
                    searchPdfService.Search(target, PdfFilePath.Value);

                    Results.Clear();
                    foreach(var hit in target[0].Hits)
                    {
                        Results.Add(new PageResult { Page = hit.Page, Word = hit.Word, Line=hit.Line });
                    }

                    if(Results.Count == 0)
                        MessageBox.Show("キーワードは見つかりませんでした。", "エラー");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー");
                }
            });
            ShowPdfCommand = new ReactiveCommand<PageResult>();
            ShowPdfCommand.Subscribe((x) =>
            {
                try
                {
                    ShowPdf(PdfFilePath.Value, x.Page);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー");
                }
            });
        }
    }
}
