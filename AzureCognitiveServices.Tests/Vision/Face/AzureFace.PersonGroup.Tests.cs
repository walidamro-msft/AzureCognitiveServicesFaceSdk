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

namespace AzureCognitiveServices.Tests.Vision.Face
{
    [TestClass]
    [TestCategory("Vision.Face.PersonGroup")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AzureFacePersonGroupTests
    {
        private static readonly AzureCognitiveServicesTestsSettings settings = new AzureCognitiveServices.Tests.AzureCognitiveServicesTestsSettings();

        [ClassCleanup()]
        public static void ClassCleanup()
        {
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
                    Console.WriteLine($"PersonGroup [{pg.Id}] was deleted Successfully during cleanup...");
                }
            }
        }


        [TestMethod]
        public void Vision_AzureFace_PersonGroup_CreateDefaultSizeNewPersonGroup_Success()
        {
            // Arrange
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My Notes!";

            bool expected = true;

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            IList<FaceAttributeType> faceAttributes = AzureFace.GetAllFaceAttributes(DetectionModel.Detection01);


            AzureFace azureFace = new AzureFace(azureFaceApiParameters);

            // Act
            bool actual = azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote);

            // Assert
            FacePersonGroup personGroup = azureFace.PersonGroupGetSingle(newPersonGroupGuid);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(personGroup);
            Assert.AreEqual(newPersonGroupGuid, personGroup.Id);
            Assert.AreEqual(newPersonGroupName, personGroup.Name);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, personGroup.RecognitionModel);
            Assert.AreEqual(newPersonGroupNote, personGroup.Notes);

            // Cleanup
            Assert.IsTrue(azureFace.PersonGroupDelete(newPersonGroupGuid), $"Failed to delete PersonGroup: [{newPersonGroupGuid}]. Please delete it manually.");
            personGroup = azureFace.PersonGroupGetSingle(newPersonGroupGuid);
            Assert.IsNull(personGroup, $"Failed to delete PersonGroup: [{newPersonGroupGuid}]. Please delete it manually."); // Make sure the PersonGroup was deleted successfully.
        }

        [TestMethod]
        public void Vision_AzureFace_PersonGroup_UpdateDefaultSizePersonGroup_Success()
        {
            // Arrange
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My Notes!";

            string updatedPersonGroupName = $"TestPersonGroupUpdated{newPersonGroupGuid}";
            string updatedPersonGroupNote = "My Updated Notes!";

            bool expected = true;

            IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
            {
                FaceEndpoint = settings.AzureVisionFaceEndPoint,
                FaceApiKey = settings.AzureVisionFaceApiKey,
                DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
                DefaultDetectionModel = DetectionModel.Detection01
            };

            IList<FaceAttributeType> faceAttributes = AzureFace.GetAllFaceAttributes(DetectionModel.Detection01);


            AzureFace azureFace = new AzureFace(azureFaceApiParameters);
            Assert.IsTrue(azureFace.PersonGroupCreate(newPersonGroupGuid, newPersonGroupName, newPersonGroupNote), $"Failed to create new PersonGroup [{newPersonGroupGuid}]!");
            FacePersonGroup personGroup = azureFace.PersonGroupGetSingle(newPersonGroupGuid);
            Assert.IsNotNull(personGroup);
            Assert.AreEqual(newPersonGroupGuid, personGroup.Id);
            Assert.AreEqual(newPersonGroupName, personGroup.Name);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, personGroup.RecognitionModel);
            Assert.AreEqual(newPersonGroupNote, personGroup.Notes);


            // Act
            bool actual = azureFace.PersonGroupUpdate(newPersonGroupGuid, updatedPersonGroupName, updatedPersonGroupNote);

            // Assert
            personGroup = azureFace.PersonGroupGetSingle(newPersonGroupGuid);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(personGroup);
            Assert.AreEqual(newPersonGroupGuid, personGroup.Id);
            Assert.AreEqual(updatedPersonGroupName, personGroup.Name);
            Assert.AreEqual(azureFaceApiParameters.DefaultRecognitionModel, personGroup.RecognitionModel);
            Assert.AreEqual(updatedPersonGroupNote, personGroup.Notes);

            // Cleanup
            Assert.IsTrue(azureFace.PersonGroupDelete(newPersonGroupGuid), $"Failed to delete PersonGroup: [{newPersonGroupGuid}]. Please delete it manually.");
            personGroup = azureFace.PersonGroupGetSingle(newPersonGroupGuid);
            Assert.IsNull(personGroup, $"Failed to delete PersonGroup: [{newPersonGroupGuid}]. Please delete it manually."); // Make sure the PersonGroup was deleted successfully.
        }

        [TestMethod]
        [TestCategory("Vision.Face.PersonGroup.Train")]
        public void Vision_AzureFace_PersonGroupPerson_Train_Success()
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

            // Add Face
            PersistedFace face = azureFace.PersonFaceAddFromFile(newPersonGroupGuid, person.PersonId, newPersonFaceImagePath, newPersonFaceNotes);

            Assert.IsNotNull(face);
            Assert.IsNotNull(face.PersistedFaceId);

            // Act
            bool actual = azureFace.PersonGroupTrain(newPersonGroupGuid);

            // Assert
            Assert.IsTrue(actual);

        }

    }
}
