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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;

namespace AzureCognitiveServices.Tests.Vision.Face
{
    [TestClass]
    [TestCategory("Vision.Face.Detect")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AzureFaceDetectTests
    {
        private static readonly AzureCognitiveServicesTestsSettings settings = new AzureCognitiveServices.Tests.AzureCognitiveServicesTestsSettings();
        
        [TestMethod]        
        public void Vision_AzureFace_Detect_1Woman1Man_Success()
        {
            // Arrange
            const string imagePath = @"Vision\Face\Resources\1Woman1Man.jpg";
            const int expectedNumberOfFacesDetected = 2;
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default
            };

            IList<FaceAttributeType> faceAttributes = AzureFace.GetAllFaceAttributes(DetectionModel.Detection01);


            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Act
            IList<DetectedFace> actualList = azureFace.Detect(imagePath, faceAttributes);

            // Assert
            Assert.IsNotNull(actualList);
            Assert.AreEqual(expectedNumberOfFacesDetected, actualList.Count);

            if (actualList[0].FaceAttributes.Gender == Gender.Female)
            {
                // First detected face was the female face.

                // Gender
                Assert.AreEqual(Gender.Female, actualList[0].FaceAttributes.Gender);
                Assert.AreEqual(Gender.Male, actualList[1].FaceAttributes.Gender);

                // Makeup
                Assert.AreEqual(true, actualList[0].FaceAttributes.Makeup.LipMakeup);
                Assert.AreEqual(false, actualList[1].FaceAttributes.Makeup.LipMakeup);

                // Exposure
                Assert.AreEqual(ExposureLevel.OverExposure, actualList[0].FaceAttributes.Exposure.ExposureLevel);
                Assert.AreEqual(ExposureLevel.GoodExposure, actualList[1].FaceAttributes.Exposure.ExposureLevel);
            }
            else
            {
                // First detected face was the male face.

                // Gender
                Assert.AreEqual(Gender.Male, actualList[0].FaceAttributes.Gender);
                Assert.AreEqual(Gender.Female, actualList[1].FaceAttributes.Gender);

                // Makeup
                Assert.AreEqual(false, actualList[0].FaceAttributes.Makeup.LipMakeup);
                Assert.AreEqual(true, actualList[1].FaceAttributes.Makeup.LipMakeup);

                // Exposure
                Assert.AreEqual(ExposureLevel.GoodExposure, actualList[0].FaceAttributes.Exposure.ExposureLevel);
                Assert.AreEqual(ExposureLevel.OverExposure, actualList[1].FaceAttributes.Exposure.ExposureLevel);
            }

            // Age
            Assert.IsTrue(actualList[0].FaceAttributes.Age > 50);
            Assert.IsTrue(actualList[1].FaceAttributes.Age > 50);

            // Blur
            Assert.AreEqual(BlurLevel.Low, actualList[0].FaceAttributes.Blur.BlurLevel);
            Assert.AreEqual(BlurLevel.Low, actualList[1].FaceAttributes.Blur.BlurLevel);

            // Emotion
            Assert.AreEqual(1, actualList[0].FaceAttributes.Emotion.Happiness);
            Assert.AreEqual(1, actualList[1].FaceAttributes.Emotion.Happiness);

            // Facial Hair
            Assert.IsTrue(actualList[0].FaceAttributes.FacialHair.Beard < 0.2);
            Assert.IsTrue(actualList[1].FaceAttributes.FacialHair.Beard < 0.2);

            // Glasses
            Assert.AreEqual(GlassesType.NoGlasses, actualList[0].FaceAttributes.Glasses.Value);
            Assert.AreEqual(GlassesType.NoGlasses, actualList[1].FaceAttributes.Glasses.Value);

            // Hair
            Assert.AreEqual(false, actualList[0].FaceAttributes.Hair.Invisible);
            Assert.AreEqual(false, actualList[1].FaceAttributes.Hair.Invisible);

            // Occlusion
            Assert.AreEqual(false, actualList[0].FaceAttributes.Occlusion.EyeOccluded);
            Assert.AreEqual(false, actualList[1].FaceAttributes.Occlusion.EyeOccluded);

            // Smile
            Assert.AreEqual(1, actualList[0].FaceAttributes.Smile);
            Assert.AreEqual(1, actualList[1].FaceAttributes.Smile);

        }
    }
}
