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
        // 이벤트를 등록함
        private void LastChildren(ItemsControl control)
        {
            if(control is TreeViewItem)
            {
                control.PreviewMouseLeftButtonDown += XAML_TreeView_PreviewMouseLeftButtonDown;
                control.DragOver += TreeViewItem_DragOver;
                control.Drop += TreeViewItem_Drop;
                control.DragLeave += TreeViewItem_DragLeave;
                control.MouseMove += TreeViewItem_MouseMove;
            }
            foreach (ItemsControl i in control.Items)
            {
                LastChildren(i);
            }


        }

        TreeViewItem selectedTreeViewItem;
        TreeViewItem changedTreeViewItem;
        Collocate collocate;
        enum Collocate
        {
            Upper,
            Child,
            Lower
        }
        Point mousePos;
        // 마우스를 클릭 했을 때
        private void XAML_TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedTreeViewItem = sender as TreeViewItem;
            Console.WriteLine("SELECT {0}", selectedTreeViewItem.Header);
        }

        // 드래그시에 마우스를 위에 올릴 때
        private void TreeViewItem_DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true; // 부모가 강조되는 것 방지
            changedTreeViewItem = sender as TreeViewItem;
            TreeViewItem selfSender = sender as TreeViewItem;
            
            (sender as TreeViewItem).FontWeight = FontWeights.UltraBold; // 글자 굵게하여 강조


            // TODO 3단계 분류
            // 상단 : 아이템을 같은 Depth로 상단 배치
            // 중간 : 아이템을 child로 배치 (마우스오버를 일정 시간 이상하면 하위보이게)
            // 하단 : 아이템을 같은 Depth로 하단 배치
            double actHeight = (sender as TreeViewItem).ActualHeight;
            double mousePos = e.GetPosition(sender as TreeViewItem).Y;
            if (mousePos < actHeight / 4)
            {
                collocate = Collocate.Upper;
                Console.WriteLine("상단 배치");
            }
            else if(mousePos < actHeight * 3 / 4)
            {
                collocate = Collocate.Child;

                Console.WriteLine("자식 배치");

            }
            else
            {
                collocate = Collocate.Lower;
                Console.WriteLine("하단 배치");

            }


        }

        // 교환
        private void TreeViewItem_Drop(object sender, DragEventArgs e)
        {
            try
            {
                // null이 있다면
                if (selectedTreeViewItem is null || changedTreeViewItem is null)
                {
                    throw(new Exception());
                }
                // 본인과 같으면 
                if (selectedTreeViewItem.Equals(changedTreeViewItem))
                {
                    throw (new Exception());
                }

                var parent = changedTreeViewItem.Parent as ItemsControl;
                int index = parent.Items.IndexOf(changedTreeViewItem);

                // selected 아이템을 분리함
                ItemsControl originParent = selectedTreeViewItem.Parent as ItemsControl;
                int originIndex = originParent.Items.IndexOf(selectedTreeViewItem);
                originParent.Items.Remove(selectedTreeViewItem);
                try
                {
                    switch (collocate)
                    {
                        case Collocate.Upper:
                            parent.Items.Insert(index, selectedTreeViewItem);
                            break;
                        case Collocate.Child:
                            changedTreeViewItem.Items.Add(selectedTreeViewItem);
                            break;
                        case Collocate.Lower:
                            index++;
                            parent.Items.Insert(index, selectedTreeViewItem);
                            break;
                    }

                }
                catch (Exception)
                {
                    // first가 child보다 상위 Depth일때 되돌리기
                    originParent.Items.Insert(originIndex, selectedTreeViewItem);
                }
            }
            catch
            {

            }
            finally
            {
                try
                {
                    changedTreeViewItem.FontWeight = FontWeights.Normal;
                }
                catch
                {

                }
                
                (XAML_TreeView.Items[0] as TreeViewItem).IsSelected = false;
                selectedTreeViewItem = null;
                changedTreeViewItem = null;
            }

        }

        // 아이템이 나가면
        private void TreeViewItem_DragLeave(object sender, DragEventArgs e)
        {
            changedTreeViewItem = null;
            (sender as TreeViewItem).FontWeight = FontWeights.Normal;

        }

        private void TreeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            // 일정량 만큼 움직였을 때 드래그 시작
            int distance = 3;
            if (selectedTreeViewItem != null)
            {
                if( ((Mouse.GetPosition(null).X - mousePos.X) > distance || (Mouse.GetPosition(null).X - mousePos.X) < -distance) ||
                    ((Mouse.GetPosition(null).Y - mousePos.Y) > distance || (Mouse.GetPosition(null).Y - mousePos.Y) < -distance))
                {
                    DragDrop.DoDragDrop(selectedTreeViewItem, selectedTreeViewItem, DragDropEffects.Move);
                }
                else
                {

                }
                
            }
        }

        private void XAML_TreeView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedTreeViewItem = null;
            changedTreeViewItem = null;
        }
    }
}
