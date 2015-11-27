using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Wicip;

using Face = Microsoft.ProjectOxford.Face;
using Emotion = Microsoft.ProjectOxford.Emotion;
using System.IO;
using Windows.Media.SpeechSynthesis;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PresentationEvaluator.Views
{
	enum OperationMode
	{
		Preview,
		Detect,
		Done
	}

	public sealed partial class MainPage : Page
	{
		private const string FACE_API_KEY = "YOUR API KEY FOR THE PROJECT OXFORD FACE API";

		private const string EMOTION_API_KEY = "YOUR API KEY FOR THE PROJECT OXFORD EMOTION API";

		private Camera camera;

		private Speaker speaker;

		private Led led;

		private PushButton pushButton;

		private OperationMode operationMode;


		public MainPage()
		{
			this.InitializeComponent();
		}


		protected override async void OnNavigatedTo( NavigationEventArgs e )
		{
			base.OnNavigatedTo( e );

			this.operationMode = OperationMode.Preview;

			this.speaker = new Speaker();

			if( GpioDeviceBase.IsAvailable )
			{
				this.led = new Led( 6 );
				this.led.TurnOn();

				this.pushButton = new PushButton( 16 );
				this.pushButton.Pushed += this.OnPushButtonPushed;

				this.pnlButtons.Visibility = Visibility.Collapsed;
			}

			this.camera = new Camera();
			await this.camera.InitializeAsync();

			this.previewElement.Source = this.camera.CaptureManager;
			await this.camera.CaptureManager.StartPreviewAsync();
		}


		private void OnPushButtonPushed( object sender, EventArgs e )
		{
			var task = Dispatcher.RunAsync( CoreDispatcherPriority.Low, () =>
			 {
				 switch( this.operationMode )
				 {
					 case OperationMode.Preview:
						 this.DetectAsync();
						 break;
					 case OperationMode.Detect:
						 break;
					 case OperationMode.Done:
						 this.ResetAsync();
						 break;
					 default:
						 break;
				 }
			 } );
		}


		private void btnDetect_Click( object sender, RoutedEventArgs e )
		{
			this.DetectAsync();
		}


		private void btnReset_Click( object sender, RoutedEventArgs e )
		{
			this.ResetAsync();
		}


		private async void DetectAsync()
		{
			Shell.SetBusyVisibility( Visibility.Visible, "Taking photo.." );

			this.operationMode = OperationMode.Detect;

			this.viewModel.PhotoFile = await this.camera.CapturePhotoToFileAsync();
			await this.camera.CaptureManager.StopPreviewAsync();

			if( this.led != null )
			{
				this.led.TurnOff();
			}

			Shell.SetBusyVisibility( Visibility.Visible, "Detecting your face.." );
			
			Face.FaceServiceClient faceClient = new Face.FaceServiceClient( FACE_API_KEY );
			Stream stream = await this.viewModel.PhotoFile.OpenStreamForReadAsync();
			Face.Contract.Face[] faces = await faceClient.DetectAsync( stream, analyzesAge: true, analyzesGender: true );

			VoiceGender voiceGender = VoiceGender.Male;
			if( faces.Length == 1 )
			{
				Face.Contract.FaceAttribute face = faces[ 0 ].Attributes;
				string greet;

				if( face.Gender == "male" )
				{
					greet = "Hello Handsome!";
					voiceGender = VoiceGender.Female;
				}
				else
				{
					greet = "Hey, Sexy!";
					voiceGender = VoiceGender.Male;
				}
				this.viewModel.Greet = $"{greet} You look {face.Age} today.";

				await this.SpeakAsync( this.viewModel.Greet, voiceGender, true );
			}
			else
			{
				this.viewModel.Greet = "I cannot see your face :(";
			}

			Shell.SetBusyVisibility( Visibility.Visible, "Detecting your emotions.." );
			
			Emotion.EmotionServiceClient emotionClient = new Emotion.EmotionServiceClient( EMOTION_API_KEY );

			stream = await this.viewModel.PhotoFile.OpenStreamForReadAsync();
			Emotion.Contract.Emotion[] emotions = await emotionClient.RecognizeAsync( stream );
			if( emotions.Length == 1 )
			{
				Emotion.Contract.Scores scores = emotions[ 0 ].Scores;
				this.viewModel.Scores = scores;

				bool like = scores.Happiness > scores.Anger + scores.Sadness + scores.Disgust;

				this.viewModel.EvaluationResult = like
					? "So you liked it! I'm so happy to hear that! :)"
					: "Oh, really? I'm terribly sorry! :(";
				await this.SpeakAsync( this.viewModel.EvaluationResult, voiceGender, false );
			}
			else
			{
				this.viewModel.EvaluationResult = "I cannot see your emotions :(";
			}

			this.operationMode = OperationMode.Done;

			Shell.SetBusyVisibility( Visibility.Collapsed );
		}


		private async void ResetAsync()
		{
			Shell.SetBusyVisibility( Visibility.Visible, "One moment.." );

			this.operationMode = OperationMode.Preview;

			this.viewModel.Reset();
			await this.camera.CaptureManager.StartPreviewAsync();

			if( this.led != null )
			{
				this.led.TurnOn();
			}

			Shell.SetBusyVisibility( Visibility.Collapsed );
		}


		private async Task SpeakAsync(string text, VoiceGender gender, bool waitUntilCompleted)
		{
			this.speaker.SetVoice( gender );
			SpeechSynthesisStream stream = await this.speaker.SynthesizeText( text );
			this.mediaElement.SetSource( stream, stream.ContentType );

			if( waitUntilCompleted )
			{
				TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
				this.mediaElement.MediaEnded += ( sender, args ) =>
				{
					tcs.TrySetResult( true );
				};
				this.mediaElement.Play();

				await tcs.Task;
			}
			else
			{
				this.mediaElement.Play();
			}
		}

	}
}
