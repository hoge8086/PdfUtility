﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PanelPluginInterface;

namespace PdfUtility.Plugins
{
    /// <summary>
    /// PdfSearchPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchPdfPanel : UserControl, IPanelPlugin
    {
        public string PluginName => "PDF検索(ページ単位)";

        public UserControl Panel => this;

        public SearchPdfPanel(IPluginHost host)
        {
            InitializeComponent();
            this.DataContext = new SearchPdfPanelViewModel(host)
            {

                ShowPdf = (string pdfPath, int page) =>
                {

                    var browser = new BrowsPdfWindow();
                    browser.Owner = Window.GetWindow(this);
                    browser.Show();
                    browser.ShowPdf(pdfPath, page);
                }
            };
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true)) {
                e.Effects = System.Windows.DragDropEffects.Copy;
            } else {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            var dropFiles = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (dropFiles == null) return;
            ((TextBox)sender).Text = dropFiles[0];
        }
    }
}
