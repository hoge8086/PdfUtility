using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PdfUtility.Plugins
{
    public class TextBoxFileDropBehavior : Behavior<TextBox>
    {
        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.PreviewDragOver += TextBox_PreviewDragOver;
            AssociatedObject.Drop += TextBox_Drop;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.PreviewDragOver -= TextBox_PreviewDragOver;
            AssociatedObject.Drop -= TextBox_Drop;
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
            AssociatedObject.Text = dropFiles[0];
        }
    }
}
