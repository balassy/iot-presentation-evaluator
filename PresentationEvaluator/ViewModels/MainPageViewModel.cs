using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

using Emotion = Microsoft.ProjectOxford.Emotion;

namespace PresentationEvaluator.ViewModels
{
	internal class MainPageViewModel : Template10.Mvvm.ViewModelBase
	{
		private StorageFile photoFile;

		public StorageFile PhotoFile
		{
			get { return this.photoFile; }
			set
			{
				this.photoFile = value;
				base.RaisePropertyChanged();

				this.PhotoImage = NotifyTaskCompletion.Create<BitmapImage>( LoadImageFromFile( this.photoFile ) );
			}
		}


		private INotifyTaskCompletion<BitmapImage> photoImage;

		public INotifyTaskCompletion<BitmapImage> PhotoImage
		{
			get { return this.photoImage; }
			set
			{
				this.photoImage = value;
				base.RaisePropertyChanged();
				base.RaisePropertyChanged( "IsPreviewVisible" );
				base.RaisePropertyChanged( "IsPhotoVisible" );
			}
		}


		private string greet;

		public string Greet
		{
			get { return this.greet; }
			set
			{
				this.greet = value;
				base.RaisePropertyChanged();
			}
		}


		private string evaluationResult;

		public string EvaluationResult
		{
			get { return this.evaluationResult; }
			set
			{
				this.evaluationResult = value;
				base.RaisePropertyChanged();
			}
		}


		private Emotion.Contract.Scores scores;

		public Emotion.Contract.Scores Scores
		{
			get { return this.scores; }
			set
			{
				this.scores = value;
				base.RaisePropertyChanged();
			}
		}



		public Visibility IsPreviewVisible
		{
			get
			{
				return this.photoFile == null
					? Visibility.Visible
					: Visibility.Collapsed;
			}
		}


		public Visibility IsPhotoVisible
		{
			get
			{
				return this.photoFile != null
					? Visibility.Visible
					: Visibility.Collapsed;
			}
		}


		public void Reset()
		{
			this.PhotoFile = null;
			this.Scores = null;
      this.Greet = this.EvaluationResult = String.Empty;			
		}


		private static async Task<BitmapImage> LoadImageFromFile( StorageFile file )
		{
			BitmapImage image = new BitmapImage();
			using( IRandomAccessStream stream = await file.OpenReadAsync() )
			{
				await image.SetSourceAsync( stream );
			}
			return image;
		}
	}
}
