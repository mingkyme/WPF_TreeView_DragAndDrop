using System;
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

namespace DragAndDrop
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }
        private bool isDrag;
        private void XAML_TreeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrag = true;
        }

        private void XAML_TreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(isDrag)
            {


            }
        }

        private void XAML_TreeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDrag = false;
            Console.WriteLine("OUTPUT");
            try
            {
                focusedTreeViewItem.FontWeight = FontWeights.Normal;
                focusedTreeViewItem = null;
            }
            catch
            {

            }
            
        }


        private void XAML_TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        TreeViewItem focusedTreeViewItem = null;
        private void XAML_TreeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if(focusedTreeViewItem != null)
                {
                    focusedTreeViewItem.FontWeight = FontWeights.Normal;
                }
                (e.Source as TreeViewItem).FontWeight = FontWeights.UltraBold;
                focusedTreeViewItem = e.Source as TreeViewItem;
            }
            catch
            {

            }
            
        }
    }
}
