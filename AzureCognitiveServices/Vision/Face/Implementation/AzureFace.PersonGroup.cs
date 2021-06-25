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
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace AzureCognitiveServices
{
    /// <summary>
    /// This partial class deals with Azure Face API - PersonGroup feature
    /// SDK Examples: https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/client-libraries?tabs=visual-studio&pivots=programming-language-csharp#create-a-persongroup
    /// Notes:
    /// 
    ///     - Free-tier subscription quota:
    ///             1,000 persons in all large person groups.
    ///     - S0-tier subscription quota:
    ///             1,000,000 persons per large person group.
    ///             1,000,000 large person groups.
    ///             1,000,000,000 persons in all large person groups.
    /// </summary>
    public partial class AzureFace
    {
        public const int MaxPersonGroupRecordsToRetrieve = 1000;

        /// <summary>
        /// Creates a new PersonGroup in Azure using the initialized default Recognition Model and default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupUniqueId">Id referencing a particular person group.</param>
        /// <param name="personGroupName">User defined name, maximum length is 128.</param>
        /// <param name="personGroupNotes">User specified data. Length should not exceed 16KB.</param>
        /// <returns>Boolean</returns>
        public bool PersonGroupCreate(string personGroupUniqueId, string personGroupName, string personGroupNotes = null)
        {
            return PersonGroupCreate(personGroupUniqueId, personGroupName, personGroupNotes, DefaultRecognitionModel, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Creates a new PersonGroup in Azure.
        /// </summary>
        /// <param name="personGroupUniqueId">Id referencing a particular person group.</param>
        /// <param name="personGroupName">User defined name, maximum length is 128.</param>
        /// <param name="personGroupNotes">User specified data. Length should not exceed 16KB.</param>
        /// <param name="recognitionModel">Possible values include: 'recognition_01', 'recognition_02'</param>
        /// <param name="personGroupSize">Default PersonGroup size (Free Tier Group Size 1,000 Persons, Paid Tier 1,000,000 Persons). LargePersonGroup can hold up to 30,000,000 Persons in one group</param>
        /// <returns>Boolean</returns>
        private bool PersonGroupCreate(string personGroupUniqueId, string personGroupName, string personGroupNotes = null, 
                                        string recognitionModel = RecognitionModel.Recognition04, PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to add new Person Group ID [{personGroupUniqueId}]...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Create large PersonGroup
                        faceClient.LargePersonGroup.CreateAsync(personGroupUniqueId, personGroupName, personGroupNotes, recognitionModel).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] created successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch(Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] failed to be created in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }
                    
                }
                else
                {
                    try
                    {
                        // Create default size PersonGroup
                        faceClient.PersonGroup.CreateAsync(personGroupUniqueId, personGroupName, personGroupNotes, recognitionModel).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] created successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] failed to be created in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }

                return true; 
            }
                        
        }

        /// <summary>
        /// Updates an existing Person Group in Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to update</param>
        /// <param name="personGroupName">New PersonGroup Name to Update</param>
        /// <param name="personGroupNotes">New PersonGroup Notes to Update</param>
        /// <returns>Boolean</returns>
        public bool PersonGroupUpdate(string personGroupUniqueId,
                                      string personGroupName,
                                      string personGroupNotes = null)
        {
            return PersonGroupUpdate(personGroupUniqueId, personGroupName, personGroupNotes, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Updates an existing Person Group in Azure.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to update</param>
        /// <param name="personGroupName">New PersonGroup Name to Update</param>
        /// <param name="personGroupNotes">New PersonGroup Notes to Update</param>
        /// <param name="personGroupSize">PersonGroup Size</param>
        /// <returns>Boolean</returns>
        public bool PersonGroupUpdate(string personGroupUniqueId, 
                                      string personGroupName, 
                                      string personGroupNotes = null,
                                      PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print("Attempting to update existing Person Group ID [{personGroupUniqueId}]...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Update large PersonGroup
                        faceClient.LargePersonGroup.UpdateAsync(personGroupUniqueId, personGroupName, personGroupNotes).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] updated successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] failed to be updated in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        // Update default size PersonGroup
                        faceClient.PersonGroup.UpdateAsync(personGroupUniqueId, personGroupName, personGroupNotes).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] updated successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] failed to be updated in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }

                return true;
            }

        }


        /// <summary>
        /// Delete a Person Group from Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to delete</param>
        /// <returns>Boolean</returns>
        public bool PersonGroupDelete(string personGroupUniqueId)
        {
            return PersonGroupDelete(personGroupUniqueId, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Delete a Person Group from Azure.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to delete</param>
        /// <param name="personGroupSize">PersonGroup Size to delete</param>
        /// <returns>Boolean</returns>
        private bool PersonGroupDelete(string personGroupUniqueId, PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {

            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to delete Person Group ID [{personGroupUniqueId}]...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Delete large PersonGroup
                        faceClient.LargePersonGroup.DeleteAsync(personGroupUniqueId).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] deleted successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] failed to be deleted in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        // Delete default size PersonGroup
                        faceClient.PersonGroup.DeleteAsync(personGroupUniqueId).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] deleted successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] failed to be deleted in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }

                return true;
            }

        }

        /// <summary>
        /// Returns maximum 1000 Person Groups from Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="top">Maximum number of groups to retrieve. Maximum 1000 PersonGroups</param>
        /// <returns></returns>
        public IList<FacePersonGroup> PersonGroupGetList(string start = null, int? top = MaxPersonGroupRecordsToRetrieve)
        {
            return PersonGroupGetList(DefaultPersonGroupSize, start, top);
        }


        /// <summary>
        /// Returns maximum 1000 Person Groups from Azure.
        /// </summary>
        /// <param name="personGroupSize">PersonGroup Size to list</param>
        /// <param name="top">Maximum number of groups to retrieve. Maximum 1000 PersonGroups</param>
        /// <returns></returns>
        private IList<FacePersonGroup> PersonGroupGetList(PersonGroupSize personGroupSize = PersonGroupSize.Default, string start = null, int? top = MaxPersonGroupRecordsToRetrieve)
        {

            if (top > MaxPersonGroupRecordsToRetrieve || top < 1)
            {
                top = MaxPersonGroupRecordsToRetrieve; // Force maximum returned list size to 1000 PersonGroups. This restriction is enforced by the Face API.
            }

            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to list all Person Groups...");
                List<FacePersonGroup> personGroupList = new List<FacePersonGroup>();

                IList<PersonGroup> defaultPersonGroupList = null;
                IList<LargePersonGroup> largePersonGroupList = null;
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // list large PersonGroup
                        largePersonGroupList = faceClient.LargePersonGroup.ListAsync(start, top, true).GetAwaiter().GetResult();

                        foreach (LargePersonGroup pg in largePersonGroupList)
                        {
                            personGroupList.Add(new FacePersonGroup { Id = pg.LargePersonGroupId, Name = pg.Name, Notes = pg.UserData, RecognitionModel = pg.RecognitionModel });
                        }
                        

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"{largePersonGroupList.Count} Large Person Groups returned successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Failed to retrieve Large Person Groups in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // list default PersonGroup
                        defaultPersonGroupList = faceClient.PersonGroup.ListAsync(null, top, true).GetAwaiter().GetResult();

                        foreach (PersonGroup pg in defaultPersonGroupList)
                        {
                            personGroupList.Add(new FacePersonGroup { Id = pg.PersonGroupId, Name = pg.Name, Notes = pg.UserData, RecognitionModel = pg.RecognitionModel });
                        }

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"{defaultPersonGroupList.Count} Default Person Groups returned successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Failed to retrieve Default Person Groups in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return personGroupList;
            }

        }


        /// <summary>
        /// Returns a single Person Group from Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to retrieve.</param>
        /// <param name="personGroupSize">PersonGroup Size to retrieve.</param>
        /// <returns></returns>
        public FacePersonGroup PersonGroupGetSingle(string personGroupUniqueId)
        {
            return PersonGroupGetSingle(personGroupUniqueId, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Returns a single Person Group from Azure.
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to retrieve.</param>
        /// <param name="personGroupSize">PersonGroup Size to retrieve.</param>
        /// <returns></returns>
        private FacePersonGroup PersonGroupGetSingle(string personGroupUniqueId, PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {

            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to list all Person Groups...");
                FacePersonGroup personGroup = null;

                PersonGroup defaultPersonGroup = null;
                LargePersonGroup largePersonGroup = null;
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // list large PersonGroup
                        largePersonGroup = faceClient.LargePersonGroup.GetAsync(personGroupUniqueId, true).GetAwaiter().GetResult();

                        personGroup = new FacePersonGroup { Id = largePersonGroup.LargePersonGroupId, 
                                                            Name = largePersonGroup.Name, 
                                                            Notes = largePersonGroup.UserData, 
                                                            RecognitionModel = largePersonGroup.RecognitionModel };

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] returned successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Failed to retrieve Large Person Group [{personGroupUniqueId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // list large PersonGroup
                        defaultPersonGroup = faceClient.PersonGroup.GetAsync(personGroupUniqueId, true).GetAwaiter().GetResult();

                        personGroup = new FacePersonGroup
                        {
                            Id = defaultPersonGroup.PersonGroupId,
                            Name = defaultPersonGroup.Name,
                            Notes = defaultPersonGroup.UserData,
                            RecognitionModel = defaultPersonGroup.RecognitionModel
                        };

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupUniqueId}] returned successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Failed to retrieve Default Person Group [{personGroupUniqueId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return personGroup;
            }

        }

        /// <summary>
        /// Submit a person group training task. Training is a crucial step that only a trained person group can be used by Face - Identify. 
        /// The training task is an asynchronous task. Training time depends on the number of person entries, and their faces in a person group. 
        /// It could be several seconds to minutes. To check training status, please use PersonGroup - Get Training Status. 
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to retrieve.</param>
        /// <returns></returns>
        public bool PersonGroupTrain(string personGroupUniqueId)
        {
            return PersonGroupTrain(personGroupUniqueId, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Submit a person group training task. Training is a crucial step that only a trained person group can be used by Face - Identify. 
        /// The training task is an asynchronous task. Training time depends on the number of person entries, and their faces in a person group. 
        /// It could be several seconds to minutes. To check training status, please use PersonGroup - Get Training Status. 
        /// </summary>
        /// <param name="personGroupUniqueId">PersonGroup ID to retrieve.</param>
        /// <param name="personGroupSize">PersonGroup Size to retrieve.</param>
        /// <returns></returns>
        private bool PersonGroupTrain(string personGroupUniqueId, PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {               

                TrainingStatus trainingStatus;
                int trainingWaitTimeoutInMillisecondsCounter = 0;

                Debug.Print($"Attempting to add new Person Group ID [{personGroupUniqueId}]...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Train large PersonGroup
                        faceClient.LargePersonGroup.TrainAsync(personGroupUniqueId).Wait();

                        // Wait until the training is completed.
                        while (true)
                        {
                            if (trainingWaitTimeoutInMillisecondsCounter >= DefaultMaximumTrainingWaitTimeoutInMilliseconds)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Timed out!");
                                return false;
                            }
                                                        
                            trainingStatus = faceClient.LargePersonGroup.GetTrainingStatusAsync(personGroupUniqueId).GetAwaiter().GetResult();

                            if (trainingStatus.Status == TrainingStatusType.Succeeded)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Succeeded!");
                                break;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Nonstarted)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Did not start yet!");
                                continue;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Running)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Still running!");
                                continue;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Failed)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Failed!");
                                return false;
                            }

                            trainingWaitTimeoutInMillisecondsCounter += 1000;
                            Thread.Sleep(1000);
                        }

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] created trained in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupUniqueId}] failed to be trained in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        // Train default size PersonGroup
                        faceClient.PersonGroup.TrainAsync(personGroupUniqueId).Wait();

                        // Wait until the training is completed.
                        while (true)
                        {
                            if (trainingWaitTimeoutInMillisecondsCounter >= DefaultMaximumTrainingWaitTimeoutInMilliseconds)
                            {
                                Debug.Print($"Current LargePersonGroup [{personGroupUniqueId}] Training Status: Timed out!");
                                return false;
                            }
                                                        
                            trainingStatus = faceClient.PersonGroup.GetTrainingStatusAsync(personGroupUniqueId).GetAwaiter().GetResult();

                            if (trainingStatus.Status == TrainingStatusType.Succeeded)
                            {
                                Debug.Print($"Current PersonGroup [{personGroupUniqueId}] Training Status: Succeeded!");
                                break;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Nonstarted)
                            {
                                Debug.Print($"Current PersonGroup [{personGroupUniqueId}] Training Status: Did not start yet!");
                                continue;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Running)
                            {
                                Debug.Print($"Current PersonGroup [{personGroupUniqueId}] Training Status: Still running!");
                                continue;
                            }

                            if (trainingStatus.Status == TrainingStatusType.Failed)
                            {
                                Debug.Print($"Current PersonGroup [{personGroupUniqueId}] Training Status: Failed!");
                                return false;
                            }

                            trainingWaitTimeoutInMillisecondsCounter += 1000;
                            Thread.Sleep(1000);
                        }

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] trained successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Person Group [{personGroupUniqueId}] failed to be trained in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }

                return true;
                
            }

        }

    }
}
