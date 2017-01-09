using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.XamlFunctionalities
{
    // http://stackoverflow.com/questions/1027051/how-to-autoscroll-on-wpf-datagrid
    public static class ScrollToLastItemBinding
    {
        public static readonly DependencyProperty AutoscrollProperty = DependencyProperty.RegisterAttached("Autoscroll", typeof(bool), typeof(ScrollToLastItemBinding), new PropertyMetadata(default(bool), AutoscrollChangedCallback));
        private static readonly Dictionary<DataGrid, NotifyCollectionChangedEventHandler> _handlersDict = new Dictionary<DataGrid, NotifyCollectionChangedEventHandler>();

        public static bool GetAutoscroll(DependencyObject element)
        {
            return (bool)element.GetValue(AutoscrollProperty);
        }

        public static void SetAutoscroll(DependencyObject element, bool value)
        {
            element.SetValue(AutoscrollProperty, value);
        }

        private static void AutoscrollChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var dataGrid = dependencyObject as DataGrid;
            if (dataGrid == null)
            {
                throw new InvalidOperationException("Dependency object is not DataGrid.");
            }

            if ((bool)args.NewValue)
            {
                Subscribe(dataGrid);
                dataGrid.Unloaded += DataGridOnUnloaded;
                dataGrid.Loaded += DataGridOnLoaded;
            }
            else
            {
                Unsubscribe(dataGrid);
                dataGrid.Unloaded -= DataGridOnUnloaded;
                dataGrid.Loaded -= DataGridOnLoaded;
            }
        }

        private static void DataGridOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            if (GetAutoscroll(dataGrid))
            {
                Subscribe(dataGrid);
            }
        }

        private static void DataGridOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            if (GetAutoscroll(dataGrid))
            {
                Unsubscribe(dataGrid);
            }
        }

        private static void ScrollToEnd(DataGrid datagrid)
        {
            if (datagrid.Items.Count == 0)
            {
                return;
            }

            datagrid.ScrollIntoView(datagrid.Items[datagrid.Items.Count - 1]);
        }

        private static void Subscribe(DataGrid dataGrid)
        {
            var handler = new NotifyCollectionChangedEventHandler((sender, eventArgs) => ScrollToEnd(dataGrid));
            if (!_handlersDict.ContainsKey(dataGrid))
            {
                _handlersDict.Add(dataGrid, handler);
                ((INotifyCollectionChanged)dataGrid.Items).CollectionChanged += handler;
                ScrollToEnd(dataGrid);
            }
        }

        private static void Unsubscribe(DataGrid dataGrid)
        {
            NotifyCollectionChangedEventHandler handler;
            _handlersDict.TryGetValue(dataGrid, out handler);
            if (handler == null)
            {
                return;
            }

            ((INotifyCollectionChanged)dataGrid.Items).CollectionChanged -= handler;
            _handlersDict.Remove(dataGrid);
        }
    }
}