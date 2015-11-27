# IoT Presentation Evaluator

The IoT Presentation Evaluator is a Windows IoT Core application that allows attendees to like or dislike a presentation with their smiles and emotions.

This application is powered by Microsoft's Project Oxford  https://www.projectoxford.ai/.


## How it works?

This application uses the camera attached to your computer or IoT device, and shows a live preview stream on your display.
By pressing a button, you can create a photo of your face, and the application will immediately send it to Microsoft's Project Oxford Service.
Using Project Oxford, the app detects your gender, your age and your emotions, and uses the text-to-speech engine built into Windows 10 to pronounce the results.
If you are a man, you will be greeted by a female voice, if you are a woman, you will get a compliment from a male voice.

If you run this application on a Windows 10 IoT Core device, you can use a physical push button to take the photo, and also to start the detection again.
A LED attached to your Windows IoT Core board indicates whether the camera is on.


## What do you need to run this sample?

You can run this application on a desktop Windows 10, or a Windows 10 IoT Core device, like a Raspberry Pi 2.

In both cases, you have to:

- attach a web camera to your computer (or have a built-in one),
- attach a speaker to your computer (or have a built-in one),
- request an API key for Project Oxford on the http://projectoxford.ai site, and enter it into the `MainPage.xaml.cs`:

```
private const string FACE_API_KEY = "YOUR API KEY FOR THE PROJECT OXFORD FACE API";
private const string EMOTION_API_KEY = "YOUR API KEY FOR THE PROJECT OXFORD EMOTION API";
```

If you run this application on a real Windows 10 IoT Core device, you have to attach a LED to GPIO 6, and a push button to GPIO 16.
(See the wiring diagrams in the Fritzing folder.)


## Limitations

Although Project Oxford can detect multiple faces on an image, for the simplicity of this sample, the code deals only with the first detected face.


## About the Windows IoT Core Interaction Pack (WICIP)

The `Wicip` folder contains copy-pasted classes from the **Windows IoT Core Interaction Pack** project. To get more reusable components for Windows IoT Core solutions navigate to https://github.com/balassy/iot-interaction-pack/.


## Disclaimer

You are free to use any part of this sample, but keep in mind, that:

- this is a sample application with quick-and-dirty, not perfectly structured code.
- this sample application is tested to work with an early beta release of Project Oxford.
- this application sends your photos to the Project Oxford service of Microsoft.


## Privacy Policy

The image understanding capabilities of the IoT Presentation Evaluator app use the Microsoft Project Oxford APIs. Microsoft will receive the images you take via this app for service improvement purposes. To report abuse of the Project Oxford APIs to Microsoft, please visit the Microsoft Project Oxford website at www.projectoxford.ai, and use the "Report Abuse" link at the bottom of the page to contact Microsoft. For more information about Microsoft privacy policies please see their privacy statement here: http://go.microsoft.com/fwlink/?LinkId=521839. 


## About the author

This project is maintained by **[György Balássy](https://hu.linkedin.com/in/balassy/)**.