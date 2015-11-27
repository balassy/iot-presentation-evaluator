using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

using PresentationEvaluator.ViewModels;
using PresentationEvaluator.Views;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;

namespace PresentationEvaluator
{
	/// Documentation on APIs used in this page:
	/// https://github.com/Windows-XAML/Template10/wiki

	sealed partial class App : Template10.Common.BootStrapper
	{
		public App()
		{
			this.InitializeComponent();
		}

		public override async Task OnStartAsync( StartKind startKind, IActivatedEventArgs args )
		{
			NavigationService.Navigate( typeof( MainPage ) );
			await Task.Yield();
		}


		public override Task OnInitializeAsync( IActivatedEventArgs args )
		{
			// Setup the hamburger shell.
			NavigationService navigationService = NavigationServiceFactory( BackButton.Attach, ExistingContent.Include );
			Window.Current.Content = new Shell( navigationService );
			return Task.FromResult<object>( null );
		}
	}
}

