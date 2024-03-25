using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.Converters
{
    public class ViewLocatorConverter : IValueConverter
    {
        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value is IViewModelBase viewModel)
            {
                return ViewLocator.Instance.BuildWithContext(viewModel);
            }
            return null;
        }

        public object? ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value is Control control && control.DataContext is IViewModelBase viewModel)
            {
                return viewModel;
            }
            return null;
        }
    }
}
