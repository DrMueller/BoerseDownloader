using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MMU.BoerseDownloader.WpfUI.Models.Enumerations;
using PropertyChanged;

namespace MMU.BoerseDownloader.WpfUI.UserControls
{
    /// <summary>
    /// Interaction logic for InformationBarControl.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class InformationBarControl : UserControl
    {
        public static readonly DependencyProperty SelectedInformationTypeProperty = DependencyProperty.Register("SelectedInformationType", typeof(InformationType), typeof(InformationBarControl), new PropertyMetadata(InformationType.None, SelectedInformationTypeChangedCallback));
        public static readonly DependencyProperty InformationTextProperty = DependencyProperty.Register("InformationText", typeof(string), typeof(InformationBarControl), new PropertyMetadata(InformationTextChangedCallback));

        public InformationBarControl()
        {
            InitializeComponent();
        }

        public string ConcatedInformationText { get; private set; }

        public Brush ForegroundColor { get; private set; }

        public string InformationText
        {
            get
            {
                return (string)GetValue(InformationTextProperty);
            }
            set
            {
                SetValue(InformationTextProperty, value);
            }
        }

        public InformationType SelectedInformationType
        {
            get
            {
                return (InformationType)GetValue(SelectedInformationTypeProperty);
            }
            set
            {
                SetValue(SelectedInformationTypeProperty, value);
                CalculateInformationAppearance();
            }
        }

        private void CalculateInformationAppearance()
        {
            string prefix;

            switch (SelectedInformationType)
            {
                case InformationType.None:
                {
                    ForegroundColor = (Brush)FindResource("MaterialDesignBody");
                    prefix = string.Empty;
                    break;
                }

                case InformationType.Information:
                {
                    ForegroundColor = Brushes.Green;
                    prefix = "Info: ";
                    break;
                }

                case InformationType.Warning:
                {
                    ForegroundColor = Brushes.Orange;
                    prefix = "Warning: ";
                    break;
                }

                case InformationType.Error:
                {
                    ForegroundColor = Brushes.Red;
                    prefix = "Error: ";
                    break;
                }

                default:
                {
                    throw new NotImplementedException(SelectedInformationType.ToString());
                }
            }

            var info = (string)GetValue(InformationTextProperty);
            var concatedMessage = prefix + info;
            ConcatedInformationText = concatedMessage;
        }

        private static void InformationTextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (InformationBarControl)d;
            obj.CalculateInformationAppearance();
        }

        private static void SelectedInformationTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (InformationBarControl)d;
            obj.CalculateInformationAppearance();
        }
    }
}