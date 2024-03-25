using System;
using Avalonia.Controls;
using ConsoleLaunchpad.ViewModels;

namespace ConsoleLaunchpad.Views
{
    public partial class ErrorView : UserControl
    {
        public ErrorView(Exception error)
        {
            this.DataContext = new ErrorViewModel(error);
            InitializeComponent();
        }

        public ErrorView(ErrorViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        public ErrorView()
        {
            InitializeComponent();
        }
    }
}
