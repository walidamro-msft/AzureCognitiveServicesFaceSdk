# AzureCognitiveServicesFaceSdk
This is a wrapper class for Azure Face API to make it easy to develop with Azure Face API as it is hard to find ressources to develop with the Face API SDK. 
This is a sample project to enroll a face in to the Azure Face API and identify the face from a group of enrolled faces.

Required NuGet Packages for the Azure Face API SDK: 
1. Install-Package System.Drawing.Common
2. Install-Package Microsoft.Azure.CognitiveServices.Vision.Face -Version 2.7.0-preview.1

Required NuGet Packages for the WPF Web Camera Capturing:
1. Install-Package Microsoft.Toolkit.Wpf.UI.XamlHost
2. Install-Package System.Drawing.Common
3. Install-Package Microsoft.VCRTForwarders.140

This solution is developed using .NET Core 3.1. The AzureCognitiveServices library is also tested with .NET 5, but Microsoft.VCRTForwarders.140 library in the WPF project will not build with .NET 5, so I had to drop it to .NET Core 3.1.

This project is still not complate and it is not ready for production code.
