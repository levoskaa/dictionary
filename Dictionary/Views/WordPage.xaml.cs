using Dictionary.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Dictionary.Views
{
    /// <summary>
    /// A page that can be used to visualize translation data.
    /// </summary>
    public sealed partial class WordPage : Page
    {
        /// <summary>
        /// Contains the logic and data belonging to the View.
        /// </summary>
        public WordPageViewModel ViewModel { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WordPage()
        {
            this.InitializeComponent();
            ViewModel = new WordPageViewModel();
            DataContext = ViewModel;
            this.Loaded += new RoutedEventHandler(PageLoadedHandler);
        }

        /// <summary>
        /// Loads data from the Repository when the page is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PageLoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
        }

        /// <summary>
        /// Handles the click event of the search Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void SearchClickHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(tbSearch.Text);
        }

        /// <summary>
        /// Handles the press of the enter key in the search TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Provides event data for the KeyUp and KeyDown routed events.</param>
        private void tbSearchKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SearchClickHandler(sender, e);
            }
        }
    }
}
