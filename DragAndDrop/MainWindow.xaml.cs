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
            LastChildren(XAML_TreeView);


        }
        private void LastChildren(ItemsControl control)
        {
            if(control.Items.Count == 0)
            {
                control.PreviewDragOver += TreeViewItem_PreviewDragOver;
                control.PreviewDrop += TreeViewItem_PreviewDrop;
                control.PreviewMouseLeftButtonDown += XAML_TreeView_PreviewMouseLeftButtonDown;
                control.PreviewDragLeave += TreeViewItem_DragLeave;
                control.PreviewMouseMove += TreeViewItem_PreviewMouseMove;
            }
            else
            {
                foreach (ItemsControl i in control.Items)
                {
                    LastChildren(i);
                }
            }
        }
        
        TreeViewItem selectedFirstTreeViewItem;
        Point mousePos;
        private void XAML_TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedFirstTreeViewItem = sender as TreeViewItem;

            Console.WriteLine("SELECT {0}", selectedFirstTreeViewItem.Header);
            selectedFirstTreeViewItem.IsSelected = true;
            mousePos = Mouse.GetPosition(null);
            

        }

        private void TreeViewItem_PreviewDragOver(object sender, DragEventArgs e)
        {
            // 드래그 될 위치를 강조
            (sender as TreeViewItem).FontWeight = FontWeights.UltraBold;
        }
        // 교환
        private void TreeViewItem_PreviewDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("DROP {0}", (sender as TreeViewItem).Header);
            Console.WriteLine("Change {0} to {1}", selectedFirstTreeViewItem.Header, (sender as TreeViewItem).Header);
            if( !sender.Equals(selectedFirstTreeViewItem))
            {
                var parent = (sender as TreeViewItem).Parent as ItemsControl;

                int index = parent.Items.IndexOf(sender);
                (selectedFirstTreeViewItem.Parent as ItemsControl).Items.Remove(selectedFirstTreeViewItem);
                parent.Items.Insert(index, selectedFirstTreeViewItem);


            }
            (sender as TreeViewItem).FontWeight = FontWeights.Normal;
            (XAML_TreeView.Items[0] as TreeViewItem).IsSelected = false;
            selectedFirstTreeViewItem = null;



        }


        private void TreeViewItem_DragLeave(object sender, DragEventArgs e)
        {
            (sender as TreeViewItem).FontWeight = FontWeights.Normal;

        }

        private void TreeViewItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // 일정량 만큼 움직였을 때 드래그 시작
            int distance = 3;
            if (selectedFirstTreeViewItem != null)
            {
                if( ((Mouse.GetPosition(null).X - mousePos.X) > distance || (Mouse.GetPosition(null).X - mousePos.X) < -distance) ||
                    ((Mouse.GetPosition(null).Y - mousePos.Y) > distance || (Mouse.GetPosition(null).Y - mousePos.Y) < -distance))
                {
                    DragDrop.DoDragDrop(selectedFirstTreeViewItem, selectedFirstTreeViewItem, DragDropEffects.Move);
                }
                
            }
        }
    }
}
