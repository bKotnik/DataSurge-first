using DataSurge.Classes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DataSurge.User_controls
{
    /// <summary>
    /// Interaction logic for Sort.xaml
    /// </summary>
    public partial class Sort : UserControl
    {
        bool pressed = false;

        public Sort()
        {
            InitializeComponent();
        }

        private void SortByNotes(object sender, RoutedEventArgs e)
        {
            ICollectionView sorted = CollectionViewSource.GetDefaultView(Utility.ListData);
            ListSortDirection direction;

            if (pressed == false)
            {
                sorted.SortDescriptions.Clear();
                direction = ListSortDirection.Ascending;
                sorted.SortDescriptions.Add(new SortDescription("Note", direction));
                pressed = true;
                sorted.Refresh();

                // flip icon
                sort_icon.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform flipIcon = new ScaleTransform();
                flipIcon.ScaleY = -1;
                sort_icon.RenderTransform = flipIcon;
            }

            else if (pressed == true)
            {
                sorted.SortDescriptions.Clear();
                direction = ListSortDirection.Descending;
                sorted.SortDescriptions.Add(new SortDescription("Note", direction));
                pressed = false;
                sorted.Refresh();

                // flip icon
                sort_icon.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform flipIcon = new ScaleTransform();
                flipIcon.ScaleY = 1;
                sort_icon.RenderTransform = flipIcon;
            }
        }
    }
}
