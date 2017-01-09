using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.XamlFunctionalities
{
    public abstract class EventCommandBinding
    {
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.RegisterAttached("EventName", typeof(string), typeof(EventCommandBinding), new PropertyMetadata(null, null));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventCommandBinding), new PropertyMetadata(null, CommandPropertyChangedCallback));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(EventCommandBinding), new PropertyMetadata(null, null));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }

        public static string GetEventName(DependencyObject obj)
        {
            return (string)obj.GetValue(EventNameProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        public static void SetEventName(DependencyObject obj, string value)
        {
            obj.SetValue(EventNameProperty, value);
        }

        private static void CommandPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;

            if (uiElement != null)
            {
                var routedEvent = GetRoutedEvent(uiElement);

                if (e.OldValue != null)
                {
                    uiElement.RemoveHandler(routedEvent, new RoutedEventHandler(OnEvent));
                }

                if (e.NewValue != null)
                {
                    uiElement.AddHandler(routedEvent, new RoutedEventHandler(OnEvent), true);
                }
            }
        }

        private static RoutedEvent GetRoutedEvent(UIElement uiElement)
        {
            var eventName = GetEventName(uiElement);

            if (string.IsNullOrEmpty(eventName))
            {
                throw new NotImplementedException($"EventName for the Element {uiElement.Uid} is not set.");
            }

            // The RoutedEvents end all with Event, so we concat this to make sure we find the Field via Reflection
            eventName += "Event";

            var allUiElementRoutedEvents = typeof(UIElement).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => typeof(RoutedEvent).IsAssignableFrom(f.FieldType));
            var allControlRoutedEvents = typeof(Control).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => typeof(RoutedEvent).IsAssignableFrom(f.FieldType));
            var allRoutedEvents = allUiElementRoutedEvents.Concat(allControlRoutedEvents);

            var fieldInfoRoutedEvent = allRoutedEvents.FirstOrDefault(f => f.Name == eventName);
            var result = (RoutedEvent)fieldInfoRoutedEvent?.GetValue(null);

            if (result == null)
            {
                throw new NotImplementedException($"Routed Event {eventName} not found on UIElement.");
            }

            return result;
        }

        private static void OnEvent(object sender, RoutedEventArgs e)
        {
            var uiElement = sender as UIElement;
            ICommand cmd = uiElement?.GetValue(CommandProperty) as ICommand;

            if (cmd != null)
            {
                var parameter = uiElement.GetValue(CommandParameterProperty);

                if (cmd.CanExecute(parameter))
                {
                    cmd.Execute(parameter);
                }
            }
        }
    }
}