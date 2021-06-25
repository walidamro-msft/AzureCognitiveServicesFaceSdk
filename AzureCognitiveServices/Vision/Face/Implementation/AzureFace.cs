/*
# This Sample Code is provided for the purpose of illustration only and is not intended to be used 
# in a production environment. THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" 
# WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
# WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. We grant You a nonexclusive, 
# royalty-free right to use and modify the Sample Code and to reproduce and distribute the object code 
# form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to 
# market Your software product in which the Sample Code is embedded; (ii) to include a valid copyright 
# notice on Your software product in which the Sample Code is embedded; and (iii) to indemnify, hold 
# harmless, and defend Us and Our suppliers from and against any claims or lawsuits, including attorneys’ 
# fees, that arise or result from the use or distribution of the Sample Code.
# This sample script is not supported under any Microsoft standard support program or service. 
# The sample script is provided AS IS without warranty of any kind. Microsoft further disclaims 
# all implied warranties including, without limitation, any implied warranties of merchantability 
# or of fitness for a particular purpose. The entire risk arising out of the use or performance of 
# the sample scripts and documentation remains with you. In no event shall Microsoft, its authors, 
# or anyone else involved in the creation, production, or delivery of the scripts be liable for any 
# damages whatsoever (including, without limitation, damages for loss of business profits, business 
# interruption, loss of business information, or other pecuniary loss) arising out of the use of or 
# inability to use the sample scripts or documentation, even if Microsoft has been advised of the 
# possibility of such damages 
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Drawing;
using System.Diagnostics;

namespace AzureCognitiveServices
{
    /// <summary>
    /// This class deals with Azure Face API using Azure Face SDK.
    /// Required NuGet Packages: 
    /// 1. Install-Package System.Drawing.Common
    /// 2. Install-Package Microsoft.Azure.CognitiveServices.Vision.Face -Version 2.7.0-preview.1
    /// </summary>
    public partial class AzureFace
    {
        // Public Static
        public static readonly string[] SupportedDetection1Attributes = { "Blur", "Exposure", "Noise", "Age",
                                                                          "Gender", "FacialHair", "Glasses", "Hair",
                                                                          "Makeup", "Accessories", "Occlusion", "HeadPose",
                                                                          "Emotion", "Smile" };
        public static readonly string[] SupportedDetection3Attributes = { "HeadPose", "Mask" };
        public static readonly int DefaultMaximumTrainingWaitTimeoutInMilliseconds = 60000; // Maximum wait for training PersonGroup (60 seconds).

        

        // Public
        /// <summary>
        /// Azure Face API Endpoint URL. 
        /// </summary>
        public readonly Uri FaceEndpointUri = null;

        /// <summary>
        /// Azure Face API Endpoint. Get this value from the Azure portal.
        /// </summary>
        public readonly string FaceEndpoint = null;

        /// <summary>
        /// 'recognition_01': The default recognition model for PersonGroup - Create.All those person groups created before 2019 March are bonded with this recognition model.
        /// 'recognition_02': Recognition model released in 2019 March.
        /// 'recognition_03': Recognition model released in 2020 May.
        /// 'recognition_04': Recognition model released in 2021 February. 'recognition_04' is recommended since its accuracy is improved on faces wearing masks compared with 'recognition_03', and its overall accuracy is improved compared with 'recognition_01' and 'recognition_02'.
        /// </summary>
        public readonly string DefaultRecognitionModel = RecognitionModel.Recognition04;
        /// <summary>
        /// 'detection_01': The default detection model for PersonGroup Person - Add Face.Recommend for near frontal face detection. For scenarios with exceptionally large angle (head-pose) faces, occluded faces or wrong image orientation, the faces in such cases may not be detected.
        /// 'detection_02': Detection model released in 2019 May with improved accuracy especially on small, side and blurry faces.
        /// 'detection_03': Detection model released in 2021 February with improved accuracy especially on small faces.
        /// </summary>
        public readonly string DefaultDetectionModel = DetectionModel.Detection01;

        /// <summary>
        /// 1. (Default): Free-tier subscription quota: 1,000 person groups.Each holds up to 1,000 persons.
        /// 2. (Default): S0-tier subscription quota: 1,000,000 person groups. Each holds up to 10,000 persons.
        /// 3. (Large):   to handle larger scale face identification problem, please consider using LargePersonGroup.
        /// </summary>
        public readonly PersonGroupSize DefaultPersonGroupSize = PersonGroupSize.Default;

        /// <summary>
        /// Minimum Face Identification Confidence Rate
        /// </summary>
        public readonly double MinFaceIdentificationConfidence;

        // Private
        /// <summary>
        /// Azure Face API Endpoint. Get this value from the Azure portal. 
        /// </summary>
        private readonly string FaceApiKey = null;

        #region Constructor(s)
        /// <summary>
        /// Main AzureFace class constructor. There is no other constructors for this class.
        /// </summary>
        /// <param name="azureFaceApiParameters">Azure Face API Parameters Object</param>
        public AzureFace(IAzureFaceApiParameters azureFaceApiParameters)
        {
            if (string.IsNullOrWhiteSpace(azureFaceApiParameters.FaceEndpoint))
            {
                throw new Exception("FaceEndpoint cannot be null, empty, or whitespace!");
            }

            if (!Uri.TryCreate(azureFaceApiParameters.FaceEndpoint, UriKind.Absolute, out FaceEndpointUri))
            {
                throw new Exception("Bad FaceEndpoint URI!");
            }

            if (string.IsNullOrWhiteSpace(azureFaceApiParameters.FaceApiKey))
            {
                throw new Exception("FaceApiKey cannot be null, empty, or whitespace!");
            }

            if (string.IsNullOrWhiteSpace(azureFaceApiParameters.DefaultRecognitionModel))
            {
                throw new Exception("DefaultRecognitionModel cannot be null, empty, or whitespace!");
            }

            // Create List of Recognition Models
            string[] RecognitionModelsList =  { RecognitionModel.Recognition01, RecognitionModel.Recognition02,
                                                RecognitionModel.Recognition03, RecognitionModel.Recognition04 };

            if (!RecognitionModelsList.Any(s => s.Contains(azureFaceApiParameters.DefaultRecognitionModel)))
            {
                throw new Exception($"Unsupported DefaultRecognitionModel! Supported RecognitionModels: {string.Join(", ", RecognitionModelsList)}.");
            }

            if (string.IsNullOrWhiteSpace(azureFaceApiParameters.DefaultDetectionModel))
            {
                throw new Exception("DefaultDetectionModel cannot be null, empty, or whitespace!");
            }

            // Create List of Detection Models
            string[] DefaultDetectionModelsList = { DetectionModel.Detection01, DetectionModel.Detection02, DetectionModel.Detection03 };

            if (!DefaultDetectionModelsList.Any(s => s.Contains(azureFaceApiParameters.DefaultDetectionModel)))
            {
                throw new Exception($"Unsupported DefaultDetectionModel! Supported DetectionModels: {string.Join(", ", DefaultDetectionModelsList)}.");
            }

            if (azureFaceApiParameters.MinFaceIdentificationConfidenceRate < 0)
            {
                azureFaceApiParameters.MinFaceIdentificationConfidenceRate = 0.00;
            }

            if (azureFaceApiParameters.MinFaceIdentificationConfidenceRate > 1)
            {
                azureFaceApiParameters.MinFaceIdentificationConfidenceRate = 1.00;
            }

            FaceEndpoint = azureFaceApiParameters.FaceEndpoint;
            FaceApiKey = azureFaceApiParameters.FaceApiKey;
            DefaultRecognitionModel = azureFaceApiParameters.DefaultRecognitionModel;
            DefaultDetectionModel = azureFaceApiParameters.DefaultDetectionModel;
            DefaultPersonGroupSize = azureFaceApiParameters.DefaultPersonGroupSize;
            MinFaceIdentificationConfidence = azureFaceApiParameters.MinFaceIdentificationConfidenceRate;

        }
        #endregion Constructor(s)

        #region Private Methods
        /// <summary>
        /// Convert an array of bytes to a Stream object.
        /// </summary>
        /// <param name="bytes">Bytes array</param>
        /// <returns>Stream</returns>
        public static Stream ConvertByteArrayToStream(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// Convert image from a file to an array of bytes.
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        /// <returns>byte[]</returns>
        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            //image to byteArray
            Image img = Image.FromFile(imagePath);
            ImageConverter imgCon = new ImageConverter();
            return (byte[])imgCon.ConvertTo(img, typeof(byte[]));
        }
               

        /// <summary>
        /// Uses subscription key and region to create a client.
        /// </summary>
        /// <param name="endpoint">Face API Endpoint URI</param>
        /// <param name="key">Face API Key</param>
        /// <returns>IFaceClient</returns>
        private static IFaceClient CreateFaceClient(string endpoint, string key)
        {
            Debug.Print("Attempting to create a Face Client...");
            Stopwatch sw = new Stopwatch(); // Create Stopwatch
            sw.Restart();   // Start Stopwatch

            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };

            sw.Stop(); // Stop Stopwatch
            Debug.Print($"Created Face Client in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");

            return faceClient;
        }

        

        private static string ReadableElapsedMilliseconds(long milliseconds)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(milliseconds);

            return string.Format("{0:D1}h {1:D1}m {2:D1}s {3:D3}ms",
                            t.Hours,
                            t.Minutes,
                            t.Seconds,
                            t.Milliseconds);
        }


        #endregion Private Methods

        #region Public Static Methods
        /// <summary>
        /// Returns all the Azure Face Attributes in case the list is null.
        /// </summary>
        /// <param name="faceAttributes">Azure Face Attributes List</param>
        /// <returns>List<FaceAttributeType></returns>
        public static IList<FaceAttributeType> GetAllFaceAttributes(string detectionModel)
        {
            IList<FaceAttributeType> faceAttributes;

            switch (detectionModel)
            {
                case DetectionModel.Detection01:
                    //Only face attributes 'blur,exposure,noise,age,gender,facialhair,glasses,hair,makeup,accessories,occlusion,headpose,emotion,smile'
                    faceAttributes = new List<FaceAttributeType>
                        {
                            FaceAttributeType.Age,
                            FaceAttributeType.Emotion,
                            FaceAttributeType.FacialHair,
                            FaceAttributeType.Gender,
                            FaceAttributeType.Glasses,
                            FaceAttributeType.Hair,
                            FaceAttributeType.HeadPose,
                            FaceAttributeType.Makeup,
                            FaceAttributeType.Smile,
                            FaceAttributeType.Occlusion,
                            FaceAttributeType.Noise,
                            FaceAttributeType.Exposure,
                            FaceAttributeType.Blur,
                            FaceAttributeType.Accessories
                        };
                    break;

                case DetectionModel.Detection02:
                    faceAttributes =  null;
                    break;

                case DetectionModel.Detection03:
                    faceAttributes = new List<FaceAttributeType>
                        {
                            FaceAttributeType.Mask,
                            FaceAttributeType.HeadPose
                        };
                    break;

                default:
                    throw new Exception($"Unsupported DetectionModel [{detectionModel}]!");

            }

            return faceAttributes;

        }

        /// <summary>
        /// Constructs a list of Azure Face Attributes and return the list.
        /// </summary>
        /// <param name="detectionModel">Detection Model. This will affect the returned Face Attributes supported by the detection model.</param>
        /// <param name="faceAttributes">List of FaceAttibuteType Enumerators</param>
        /// <returns>IList<FaceAttributeType></returns>
        public static IList<FaceAttributeType> ConstructFaceAttributes(string detectionModel, params FaceAttributeType[] faceAttributes)
        {
            HashSet<FaceAttributeType> uniqueFaceAttributes = new HashSet<FaceAttributeType>();

            foreach (FaceAttributeType faceAttribute in faceAttributes)
            {
                switch (detectionModel)
                {
                    case DetectionModel.Detection01:
                        //Only face attributes 'blur,exposure,noise,age,gender,facialhair,glasses,hair,makeup,accessories,occlusion,headpose,emotion,smile' are supported by detection_01.
                        if (!SupportedDetection1Attributes.Any(s => s.Contains(faceAttribute.ToString())))
                        {
                            // Unsupported Attribute by the Detection Model, please skip this attribute
                            continue;
                        }
                        break;

                    case DetectionModel.Detection02:
                        // No attributes are supported by detection_02.
                        return null;

                    case DetectionModel.Detection03:
                        //Only face attributes 'headpose,mask' are supported by detection_03.
                        if (!SupportedDetection3Attributes.Any(s => s.Contains(faceAttribute.ToString())))
                        {
                            // Unsupported Attribute by the Detection Model, please skip this attribute
                            continue;
                        }
                        break;

                    default:
                        throw new Exception($"Unsupported DetectionModel [{detectionModel}]!");
                }


                uniqueFaceAttributes.Add(faceAttribute);
            }

            return uniqueFaceAttributes.ToArray();
        }

        /// <summary>
        /// Returns a new Guid string.
        /// </summary>
        /// <returns>String</returns>
        public static string GenerateNewGuidString()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion Public Static Methods


    }
}
