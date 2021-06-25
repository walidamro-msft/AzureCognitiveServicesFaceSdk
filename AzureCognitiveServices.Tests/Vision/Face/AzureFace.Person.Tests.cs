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
    [TestCategory("Vision.Face.Person")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AzureFacePersonGroupPersonTests
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
        public void Vision_AzureFace_PersonGroupPerson_CreateNewPerson_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPersonName = "John Doe";
            string newPersonNotes = "Please delete this person. This is a test person.";

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

            // Act
            // Create Person in PersonGroup
            Person person = azureFace.PersonCreate(newPersonGroupGuid, newPersonName, newPersonNotes);

            // Assert 1
            Assert.IsNotNull(person, "Failed to create new person!");
            Assert.IsNotNull(person.PersonId);

            // Assert 2
            Person p = azureFace.PersonGetSingle(newPersonGroupGuid, person.PersonId);
            Assert.IsNotNull(p);
            Assert.AreEqual(person.PersonId, p.PersonId);
            Assert.AreEqual(newPersonName, p.Name);
            Assert.AreEqual(newPersonNotes, p.UserData);

        }

        [TestMethod]
        public void Vision_AzureFace_PersonGroupPerson_CreateMultiplePersons_Success()
        {
            // Arrange

            // Create PersonGroup
            string newPersonGroupGuid = AzureFace.GenerateNewGuidString();
            string newPersonGroupName = $"TestPersonGroup{newPersonGroupGuid}";
            string newPersonGroupNote = "My PersonGroup Test Notes! Please delete if you see this group.";

            string newPerson1Name = "John Doe";
            string newPerson1Notes = "Please delete this person. This is a test person.";
            string newPerson2Name = "Jane Smith";
            string newPerson2Notes = "Please delete this person. This is a test person.";

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

            // Act
            // Create Person 1 in PersonGroup
            Person person1 = azureFace.PersonCreate(newPersonGroupGuid, newPerson1Name, newPerson1Notes);
            // Create Person 2 in PersonGroup
            Person person2 = azureFace.PersonCreate(newPersonGroupGuid, newPerson2Name, newPerson2Notes);

            // Assert 1
            Assert.IsNotNull(person1, "Failed to create new person 1!");
            Assert.IsNotNull(person1.PersonId);
            Assert.IsNotNull(person2, "Failed to create new person 2!");
            Assert.IsNotNull(person2.PersonId);

            // Assert 2
            IList<Person> personList = azureFace.PersonGetList(newPersonGroupGuid);
            Assert.IsNotNull(personList);
            Assert.IsTrue(personList.Count == 2);

            // We don't know if the Facial Recognition algorithm is going to choose for the first and second face.
            if (person1.PersonId.Equals(personList[0].PersonId))
            {
                Assert.AreEqual(person1.PersonId, personList[0].PersonId);
                Assert.AreEqual(newPerson1Name, personList[0].Name);
                Assert.AreEqual(newPerson1Notes, personList[0].UserData);
                Assert.AreEqual(person2.PersonId, personList[1].PersonId);
                Assert.AreEqual(newPerson2Name, personList[1].Name);
                Assert.AreEqual(newPerson2Notes, personList[1].UserData);
            }
            else
            {
                Assert.AreEqual(person1.PersonId, personList[1].PersonId);
                Assert.AreEqual(newPerson1Name, personList[1].Name);
                Assert.AreEqual(newPerson1Notes, personList[1].UserData);
                Assert.AreEqual(person2.PersonId, personList[0].PersonId);
                Assert.AreEqual(newPerson2Name, personList[0].Name);
                Assert.AreEqual(newPerson2Notes, personList[0].UserData);
            }
            

        }


    }
}
