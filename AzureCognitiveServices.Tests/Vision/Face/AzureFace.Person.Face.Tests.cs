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
using System;
using System.Diagnostics;

namespace AzureCognitiveServices.Tests.Vision.Face
{
    [TestClass]
    [TestCategory("Vision.Face.Person.Face")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AzureFacePersonGroupPersonFaceTests
    {
        private static readonly AzureCognitiveServicesTestsSettings settings = new AzureCognitiveServices.Tests.AzureCognitiveServicesTestsSettings();

        [ClassCleanup()]
        public static void ClassCleanup()
        {

            Trace.WriteLine($"Cleanup started...");
            Debug.Print("Cleanup started...");
            Console.WriteLine($"Cleanup started...");

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

            IList<FacePersonGroup> personGroups = azureFace.PersonGroupGetList();

            foreach (FacePersonGroup pg in personGroups)
            {
                if (pg.Name.StartsWith("TestPersonGroup"))
                {
                    Assert.IsTrue(azureFace.PersonGroupDelete(pg.Id), $"Failed to delete Test PersonGroup [{pg.Id}] during cleanup! Please delete manually!");
                    Trace.WriteLine($"PersonGroup [{pg.Id}] was deleted Successfully during cleanup...");
                    Console.WriteLine($"PersonGroup [{pg.Id}] was deleted Successfully during cleanup...");
                }
            }

            Trace.WriteLine("Cleanup ended...");
            Debug.Print("Cleanup ended...");
            Console.WriteLine("Cleanup ended...");
        }

        [TestMethod]
        public void Vision_AzureFace_PersonGroupPerson_Face_AddFaceToPerson_FromFile_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "John Doe";
            string newPersonNotes = "Please delete this person. This is a test person.";
            
            string newPersonFaceImagePath = @"Vision\Face\Resources\1Woman.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";


            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default
            };


            IList<FaceAttributeType> faceAttributes = AzureFace.GetAllFaceAttributes(azureFaceApiParameters.DefaultDetectionModel);


            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Act 
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            // Assert
            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);


        }
                

        [TestMethod]
        public void Vision_AzureFace_PersonGroupPerson_Face_AddFaceToPerson_FromUri_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "John Doe";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImageUri = "https://docs.microsoft.com/en-us/azure/cognitive-services/face/images/facefindsimilar.queryface.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";


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

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Act 
            PersistedFace face = azureFace.PersonFaceAddFromUri(newPersonGroupGuid, person.PersonId, newPersonFaceImageUri, newPersonFaceNotes);

            // Assert
            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);


        }

        [TestMethod]
        [TestCategory("Vision.Face.Person.Face.Identify")]
        public void Vision_AzureFace_PersonGroupPerson_Face_IdentifyBillGates_FromFile_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "Bill Gates";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImagePath = @"Vision\Face\Resources\BillGates1.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";

            string personToIdentifyImagePath = @"Vision\Face\Resources\BillGates2.jpg";
            string expectedPersonToIdentifyName = newPersonName;

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.85
            };

            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Train PersonGroup
            bool trained = azureFace.PersonGroupTrain(newPersonGroupGuid);

            Assert.IsTrue(trained);

            // Act
            Person actual = azureFace.PersonFaceIdentifyFromFile(newPersonGroupGuid, personToIdentifyImagePath);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedPersonToIdentifyName, actual.Name, "Failed to identify face!");

        }

        [TestMethod]
        [TestCategory("Vision.Face.Person.Face.Identify")]
        public void Vision_AzureFace_PersonGroupPerson_Face_IdentifySatyaNadella_FromFile_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "Satya Nadella";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImagePath = @"Vision\Face\Resources\SatyaNadella1.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";

            string personToIdentifyImagePath = @"Vision\Face\Resources\SatyaNadella2.jpg";
            string expectedPersonToIdentifyName = newPersonName;

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.85
            };

            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Train PersonGroup
            bool trained = azureFace.PersonGroupTrain(newPersonGroupGuid);

            Assert.IsTrue(trained);

            // Act
            Person actual = azureFace.PersonFaceIdentifyFromFile(newPersonGroupGuid, personToIdentifyImagePath);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedPersonToIdentifyName, actual.Name, "Failed to identify face!");

        }

        [TestMethod]
        [TestCategory("Vision.Face.Person.Face.Identify")]
        public void Vision_AzureFace_PersonGroupPerson_Face_IdentifySatyaNadella2_FromFile_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "Satya Nadella";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImagePath = @"Vision\Face\Resources\SatyaNadella1.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";

            string personToIdentifyImagePath = @"Vision\Face\Resources\SatyaNadella3.jpg";
            string expectedPersonToIdentifyName = newPersonName;

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.85
            };

            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Train PersonGroup
            bool trained = azureFace.PersonGroupTrain(newPersonGroupGuid);

            Assert.IsTrue(trained);

            // Act
            Person actual = azureFace.PersonFaceIdentifyFromFile(newPersonGroupGuid, personToIdentifyImagePath);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedPersonToIdentifyName, actual.Name, "Failed to identify face!");

        }

        [TestMethod]
        [TestCategory("Vision.Face.Person.Face.Identify")]
        public void Vision_AzureFace_PersonGroupPerson_Face_IdentifySatyaNadella_SatyaFaceIsNotInPersonGroup_FromFile_Failure()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "Bill Gates";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImagePath = @"Vision\Face\Resources\BillGates1.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";

            string personToIdentifyImagePath = @"Vision\Face\Resources\SatyaNadella2.jpg";

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.85
            };

            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Train PersonGroup
            bool trained = azureFace.PersonGroupTrain(newPersonGroupGuid);

            Assert.IsTrue(trained);

            // Act
            Person actual = azureFace.PersonFaceIdentifyFromFile(newPersonGroupGuid, personToIdentifyImagePath);

            // Assert
            Assert.IsNull(actual, "Face was identified! Test Failed!");

        }

        [TestMethod]
        [TestCategory("Vision.Face.Person.Face.Identify")]
        [ExpectedException(typeof(Exception))]
        public void Vision_AzureFace_PersonGroupPerson_Face_IdentifyTwoFaces_TwoFacesNotSupported_FromFile_Failure()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "Bill Gates";
            string newPersonNotes = "Please delete this person. This is a test person.";

            string newPersonFaceImagePath = @"Vision\Face\Resources\BillGates1.jpg";
            string newPersonFaceNotes = $"My Face notes for [{newPersonName}]. Please delete if you see this face.";

            string personToIdentifyImagePath = @"Vision\Face\Resources\1Woman1Man.jpg";

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01,
                DefaultPersonGroupSize = PersonGroupSize.Default,
                MinFaceIdentificationConfidenceRate = 0.85
            };

            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Create PersonGroup
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), "Failed to create PersonGroup [{newPersonGroupGuid}]! Test Failed!");

            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Train PersonGroup
            bool trained = azureFace.PersonGroupTrain(newPersonGroupGuid);

            Assert.IsTrue(trained);

            // Act
            Person actual = azureFace.PersonFaceIdentifyFromFile(newPersonGroupGuid, personToIdentifyImagePath);

            // Assert
            Assert.Fail("the system should not identify a person if the image contains more than 1 person! Test Failed!");

        }

    }
}
