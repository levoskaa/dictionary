using Dictionary.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Dictionary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WordPage : Page
    {
        public WordPageViewModel ViewModel { get; private set; }

        public WordPage()
        {
            this.InitializeComponent();
            ViewModel = new WordPageViewModel();
            DataContext = ViewModel;
            this.Loaded += new RoutedEventHandler(PageLoadedHandler);
        }

        private void PageLoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
        }

        private void SearchClickHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(tbSearch.Text);
        }

        private void tbSearchKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SearchClickHandler(sender, e);
            }
        }
    }
}
