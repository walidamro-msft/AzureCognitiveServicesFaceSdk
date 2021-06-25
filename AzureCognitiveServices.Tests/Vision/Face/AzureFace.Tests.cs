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

using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices.Tests.Vision.Face
{
    [TestClass]
    [TestCategory("Vision.Face.Constructor")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AzureFaceTests
    {

        [TestMethod]        
        public void Vision_AzureFace_Constructor_GoodParameters_Success()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.AreEqual(azureFaceApiParameters.FaceEndpoint, azureFace.FaceEndpoint);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, azureFace.DefaultRecognitionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultDetectionModel, azureFace.DefaultDetectionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultPersonGroupSize, azureFace.DefaultPersonGroupSize);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceEndpoint_NullValue_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = null,
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceEndpoint_EmptyString_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceEndpoint_WhiteSpaces_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "    ",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceEndpoint_NoneAbsoluteUri_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "www.test.com",
                FaceApiKey = "7a14a2e4515c45039d2458401ef37ef9",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceApiKey_NullValue_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = null,
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceApiKey_EmptyString_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_FaceApiKey_WhiteSpaces_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "   ",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultRecognitionModel_NullValue_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = null,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultRecognitionModel_EmptyString_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = "",
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultRecognitionModel_WhiteSpaces_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = "   ",
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultRecognitionModel_UnsupportedRecognitionModel_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = "Unsupported",
                DefaultDetectionModel = DetectionModel.Detection01
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        public void Vision_AzureFace_ConstructFaceAttributes_DetectionModel1_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedSupportedDetection1Attributes = new List<FaceAttributeType>();
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Age);
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Gender);

            // Act
            List<FaceAttributeType> actualSupportedDetection1Attributes = AzureFace.ConstructFaceAttributes(DetectionModel.Detection01, FaceAttributeType.Age, FaceAttributeType.Gender).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedSupportedDetection1Attributes, actualSupportedDetection1Attributes);
            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultDetectionModel_NullValue_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition01,
                DefaultDetectionModel = null
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultDetectionModel_EmptyString_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition01,
                DefaultDetectionModel = ""
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultDetectionModel_WhiteSpaces_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition01,
                DefaultDetectionModel = "   "
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_Constructor_BadParameters_DefaultDetectionModel_UnsupportedDetectionModel_Failure()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition01,
                DefaultDetectionModel = "Unsupported"
            };

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.Fail("The constructor is supposed to throw an exception as we passed a bad parameter! Test Failed!");

        }

        [TestMethod]
        public void Vision_AzureFace_ConstructFaceAttributes_DetectionModel1_3Attributes_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedSupportedDetection1Attributes = new List<FaceAttributeType>();
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Age);
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Gender);
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Smile);

            // Act
            List<FaceAttributeType> actualSupportedDetection1Attributes = AzureFace.ConstructFaceAttributes(DetectionModel.Detection01, FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Smile).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedSupportedDetection1Attributes, actualSupportedDetection1Attributes);

        }

        [TestMethod]
        public void Vision_AzureFace_ConstructFaceAttributes_DetectionModel1_UnsupportedFaceAttributeType_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedSupportedDetection1Attributes = new List<FaceAttributeType>();
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Age);
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Gender);
            expectedSupportedDetection1Attributes.Add(FaceAttributeType.Smile);
            
            // Act
            List<FaceAttributeType> actualSupportedDetection1Attributes = AzureFace.ConstructFaceAttributes(DetectionModel.Detection01, FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Smile, FaceAttributeType.Mask).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedSupportedDetection1Attributes, actualSupportedDetection1Attributes);

        }


        [TestMethod]
        public void Vision_AzureFace_ConstructFaceAttributes_DetectionModel2_Success()
        {
            // Arrange

            // Act
            IList<FaceAttributeType> actualSupportedDetection2Attributes = AzureFace.ConstructFaceAttributes(DetectionModel.Detection02, FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Smile);

            // Assert
            Assert.IsNull(actualSupportedDetection2Attributes);
        }

        [TestMethod]
        public void Vision_AzureFace_ConstructFaceAttributes_DetectionModel3_3Attributes_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedSupportedDetection3Attributes = new List<FaceAttributeType>();
            expectedSupportedDetection3Attributes.Add(FaceAttributeType.HeadPose);
            expectedSupportedDetection3Attributes.Add(FaceAttributeType.Mask);

            // Act
            List<FaceAttributeType> actualSupportedDetection3Attributes = AzureFace.ConstructFaceAttributes(DetectionModel.Detection03, FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Smile, FaceAttributeType.HeadPose, FaceAttributeType.Mask).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedSupportedDetection3Attributes, actualSupportedDetection3Attributes);

        }

        [TestMethod]
        public void Vision_AzureFace_ClassPublicProperties_3Attributes_Success()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.81
            };



            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.AreEqual(azureFaceApiParameters.FaceEndpoint, azureFace.FaceEndpoint);
            Assert.AreEqual(new Uri(azureFaceApiParameters.FaceEndpoint), azureFace.FaceEndpointUri);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, azureFace.DefaultRecognitionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultDetectionModel, azureFace.DefaultDetectionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultPersonGroupSize, azureFace.DefaultPersonGroupSize);
            Assert.AreEqual(azureFaceApiParameters.MinFaceIdentificationConfidenceRate, azureFace.MinFaceIdentificationConfidence);

        }

        [TestMethod]
        public void Vision_AzureFace_ClassPublicProperties_MinFaceIdentificationConfidenceRate_LessThanZero_Success()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = -0.45
            };

            double expectedMinFaceIdentificationConfidenceRate = 0.00;

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.AreEqual(azureFaceApiParameters.FaceEndpoint, azureFace.FaceEndpoint);
            Assert.AreEqual(new Uri(azureFaceApiParameters.FaceEndpoint), azureFace.FaceEndpointUri);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, azureFace.DefaultRecognitionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultDetectionModel, azureFace.DefaultDetectionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultPersonGroupSize, azureFace.DefaultPersonGroupSize);
            Assert.AreEqual(expectedMinFaceIdentificationConfidenceRate, azureFace.MinFaceIdentificationConfidence);

        }

        [TestMethod]
        public void Vision_AzureFace_ClassPublicProperties_MinFaceIdentificationConfidenceRate_MoreThanOne_Success()
        {
            // Arrange
            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = "https://xyz.cognitiveservices.azure.com",
                FaceApiKey = "sdfdsfdsfsdfsd",
                DefaultRecognitionModel = RecognitionModel.Recognition04,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 2.45
            };

            double expectedMinFaceIdentificationConfidenceRate = 1.00;

            // Act
            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Assert
            Assert.AreEqual(azureFaceApiParameters.FaceEndpoint, azureFace.FaceEndpoint);
            Assert.AreEqual(new Uri(azureFaceApiParameters.FaceEndpoint), azureFace.FaceEndpointUri);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, azureFace.DefaultRecognitionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultDetectionModel, azureFace.DefaultDetectionModel);
            Assert.AreEqual(azureFaceApiParameters.DefaultPersonGroupSize, azureFace.DefaultPersonGroupSize);
            Assert.AreEqual(expectedMinFaceIdentificationConfidenceRate, azureFace.MinFaceIdentificationConfidence);

        }

        [TestMethod]
        public void Vision_AzureFace_GetAllFaceAttributes_DetectionModel1_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedAllFaceAttributesForDetectionModel1 = new List<FaceAttributeType>();
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Age);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Emotion);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.FacialHair);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Gender);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Glasses);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Hair);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.HeadPose);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Makeup);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Smile);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Occlusion);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Noise);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Exposure);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Blur);
            expectedAllFaceAttributesForDetectionModel1.Add(FaceAttributeType.Accessories);


            // Act
            List<FaceAttributeType> actualAllFaceAttributesForDetectionModel1 = AzureFace.GetAllFaceAttributes(DetectionModel.Detection01).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedAllFaceAttributesForDetectionModel1, actualAllFaceAttributesForDetectionModel1);

        }

        [TestMethod]
        public void Vision_AzureFace_GetAllFaceAttributes_DetectionModel2_Success()
        {
            // Arrange
            
            // Act
            IList<FaceAttributeType> actualAllFaceAttributesForDetectionModel2 = AzureFace.GetAllFaceAttributes(DetectionModel.Detection02);

            // Assert
            Assert.IsNull(actualAllFaceAttributesForDetectionModel2);
        }

        [TestMethod]
        public void Vision_AzureFace_GetAllFaceAttributes_DetectionModel3_Success()
        {
            // Arrange
            List<FaceAttributeType> expectedAllFaceAttributesForDetectionModel3 = new List<FaceAttributeType>();
            expectedAllFaceAttributesForDetectionModel3.Add(FaceAttributeType.Mask);
            expectedAllFaceAttributesForDetectionModel3.Add(FaceAttributeType.HeadPose);


            // Act
            List<FaceAttributeType> actualAllFaceAttributesForDetectionModel3 = AzureFace.GetAllFaceAttributes(DetectionModel.Detection03).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedAllFaceAttributesForDetectionModel3, actualAllFaceAttributesForDetectionModel3);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_GetAllFaceAttributes_UnsupportedDetectionModel_Failure()
        {
            // Arrange


            // Act
            IList<FaceAttributeType> actualAllFaceAttributesForDetectionModel3 = AzureFace.GetAllFaceAttributes("Unsupported");

            // Assert
            Assert.Fail("The method is supposed to throw an exception as we passed an unsupported DetectionModel! Test Failed!");

        }


    }
}
