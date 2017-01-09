using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.XamlFunctionalities
{
    public static class SwitchStyleOnClickBinding
    {
        public static readonly DependencyProperty SwitchStyleOnClickProperty = DependencyProperty.RegisterAttached("SwitchStyleOnClick", typeof(bool), typeof(SwitchStyleOnClickBinding), new PropertyMetadata(false, SwitchStyleOnClickChangedCallback));
        private static Style _dataGridRowStyleMaximized;
        private static Style _dataGridRowStyleMinimized;

        public static bool GetSwitchStyleOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(SwitchStyleOnClickProperty);
        }

        public static void SetSwitchStyleOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(SwitchStyleOnClickProperty, value);
        }

        private static void AssureStylesAreInitialized()
        {
            if (_dataGridRowStyleMinimized == null)
            {
                _dataGridRowStyleMinimized = Application.Current.FindResource("StyleDataGridRowMinimized") as Style;
            }

            if (_dataGridRowStyleMaximized == null)
            {
                _dataGridRowStyleMaximized = Application.Current.FindResource("StyleDataGridRowMaximized") as Style;
            }
        }

        private static void DataGridRowHeader_Click(object sender, RoutedEventArgs e)
        {
            AssureStylesAreInitialized();
            var obj = (DependencyObject)e.OriginalSource;
            while (!(obj is DataGridRow) && obj != null)
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            var dataGridRow = obj as DataGridRow;

            if (dataGridRow != null)
            {
                if (dataGridRow.Style == _dataGridRowStyleMaximized)
                {
                    dataGridRow.IsSelected = false;
                    dataGridRow.Style = _dataGridRowStyleMinimized;
                }
                else
                {
                    dataGridRow.IsSelected = true;
                    dataGridRow.Style = _dataGridRowStyleMaximized;
                }
            }
        }

        private static bool GetBoolOrDefault(object boolValue)
        {
            var result = false;
            if (boolValue != null && boolValue is bool)
            {
                result = (bool)boolValue;
            }

            return result;
        }

        private static void SwitchStyleOnClickChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as ToggleButton;
            var oldVal = GetBoolOrDefault(e.OldValue);
            var newVal = GetBoolOrDefault(e.NewValue);

            if (oldVal && !newVal)
            {
                uiElement.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(DataGridRowHeader_Click));
            }

            if (!oldVal && newVal)
            {
                uiElement.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(DataGridRowHeader_Click), true);
            }
        }
    }
}