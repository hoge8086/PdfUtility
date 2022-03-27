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
    public class SearchPdfPanelViewModel
    {

        public IPluginHost Host;
        public ReactiveProperty<string> PdfFilePath { get; private set; }

        public SearchEachPageOfPdfPanelViewModel EachPage { get; set; }
        public SearchAllPageOfPdfPanelViewModel AllPage { get; set; }

        public SearchPdfPanelViewModel(IPluginHost host, Action<string, int> showPdf)
        {
            Host = host;
            PdfFilePath = new ReactiveProperty<string>();

            AllPage = new SearchAllPageOfPdfPanelViewModel(host, PdfFilePath) { ShowPdf = showPdf };
            EachPage = new SearchEachPageOfPdfPanelViewModel(host, PdfFilePath) { ShowPdf = showPdf };
        }

    }
}
