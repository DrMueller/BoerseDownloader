using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.XamlFunctionalities
{
    public static class DataGridMultiSelectBinding
    {
        public static readonly DependencyProperty SelectedItemsChangedHandlerProperty = DependencyProperty.RegisterAttached("SelectedItemsChangedHandler", typeof(ICommand), typeof(DataGridMultiSelectBinding), new FrameworkPropertyMetadata(OnSelectedItemsChangedHandlerChanged));

        public static ICommand GetSelectedItemsChangedHandler(DependencyObject element)
        {
            return (ICommand)element.GetValue(SelectedItemsChangedHandlerProperty);
        }

        public static void ItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var itemsChangedHandler = GetSelectedItemsChangedHandler(dataGrid);
            itemsChangedHandler.Execute(dataGrid.SelectedItems);
        }

        public static void SetSelectedItemsChangedHandler(DependencyObject element, ICommand value)
        {
            element.SetValue(SelectedItemsChangedHandlerProperty, value);
        }

        private static void OnSelectedItemsChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;

            if (e.OldValue == null && e.NewValue != null)
            {
                dataGrid.SelectionChanged += ItemsControl_SelectionChanged;
            }

            if (e.OldValue != null && e.NewValue == null)
            {
                dataGrid.SelectionChanged -= ItemsControl_SelectionChanged;
            }
        }
    }
}