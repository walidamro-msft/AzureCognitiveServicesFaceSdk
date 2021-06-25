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

namespace AzureCognitiveServices
{
    /// <summary>
    /// This partial class deals with Azure Face API - PersonGroupPerson feature
    /// Notes:
    /// A person group is a container holding the uploaded person data, including face recognition features.
    ///     After creation, use PersonGroup Person - Create to add persons into the group, and then call PersonGroup - Train to get this group ready for Face - Identify.
    ///     No image will be stored.Only the person's extracted face feature(s) and userData will be stored on server until PersonGroup Person - Delete or PersonGroup - Delete is called.
    /// 
    /// 'recognitionModel' should be specified to associate with this person group. The default value for 'recognitionModel' is 'recognition_01', if the latest model needed, please explicitly specify the model you need in this parameter.New faces that are added to an existing person group will use the recognition model that's already associated with the collection. Existing face feature(s) in a person group can't be updated to features extracted by another version of recognition model.
    /// 
    ///     'recognition_01': The default recognition model for PersonGroup - Create.All those person groups created before 2019 March are bonded with this recognition model.
    ///     'recognition_02': Recognition model released in 2019 March.
    ///     'recognition_03': Recognition model released in 2020 May.
    ///     'recognition_04': Recognition model released in 2021 February. 'recognition_04' is recommended since its accuracy is improved on faces wearing masks compared with 'recognition_03', and its overall accuracy is improved compared with 'recognition_01' and 'recognition_02'.
    /// 
    ///     Person group quota:
    ///         - Free-tier subscription quota: 1,000 person groups. Each holds up to 1,000 persons.
    ///         - S0-tier subscription quota: 1,000,000 person groups. Each holds up to 10,000 persons.
    ///         - to handle larger scale face identification problem, please consider using LargePersonGroup.

    /// </summary>
    public partial class AzureFace
    {
        public const int MaxPersonRecordsToRetrieve = 10000;

        /// <summary>
        /// Create a new Person under a specific PersonGroup in Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to create a new Person under.</param>
        /// <param name="personName">The new Person Name</param>
        /// <param name="personNotes">New Person Notes</param>
        /// <returns></returns>
        public Person PersonCreate(string personGroupId,
                                   string personName,
                                   string personNotes = null)
        {
            return PersonCreate(personGroupId, personName, personNotes, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Create a new Person under a specific PersonGroup in Azure.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to create a new Person under.</param>
        /// <param name="personName">The new Person Name</param>
        /// <param name="personNotes">New Person Notes</param>
        /// <param name="personGroupSize">PersonGroup Size</param>
        /// <returns></returns>
        private Person PersonCreate(string personGroupId, 
                                   string personName, 
                                   string personNotes = null,
                                   PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to add new Person to the Person Group ID [{personGroupId}]...");

                Person person;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Create large PersonGroup
                        person = faceClient.LargePersonGroupPerson.CreateAsync(personGroupId, personName, personNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] created new Person [{personName}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to created a new Person [{personName}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // Create default PersonGroup
                        person = faceClient.PersonGroupPerson.CreateAsync(personGroupId, personName, personNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] created new Person [{personName}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] failed to created a new Person [{personName}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return person;
            }

        }


        /// <summary>
        /// Delete an existing Person from a specific PersonGroup in Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to delete the Person from.</param>
        /// <param name="personId">Person GUID to delete from PersonGroup</param>
        /// <returns>Boolean</returns>
        public bool PersonDelete(string personGroupId,
                                 Guid personId)
        {
            return PersonDelete(personGroupId, personId, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Delete an existing Person from a specific PersonGroup in Azure.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to delete the Person from.</param>
        /// <param name="personId">Person GUID to delete from PersonGroup</param>
        /// <param name="personGroupSize">PersonGroup Size</param>
        /// <returns>Boolean</returns>
        private bool PersonDelete(string personGroupId,
                                 Guid personId,
                                 PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to delete existing Person [{personId}] from the Person Group ID [{personGroupId}]...");

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Delete a Person in a Large PersonGroup
                        faceClient.LargePersonGroupPerson.DeleteAsync(personGroupId, personId).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] deleted existing Person ID [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to delete existing Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        // Delete a Person in a default PersonGroup
                        faceClient.PersonGroupPerson.DeleteAsync(personGroupId, personId).Wait();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] deleted existing Person ID [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] failed to delete existing Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return false;
                    }

                }

                return true;
            }

        }

        /// <summary>
        /// Retrieves an existing Person from a specific PersonGroup in Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to retrieve the Person from.</param>
        /// <param name="personId">Person GUID to retrieve from PersonGroup</param>
        /// <returns>Boolean</returns>
        public Person PersonGetSingle(string personGroupId,
                                      Guid personId)
        {
            return PersonGetSingle(personGroupId, personId, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Retrieves an existing Person from a specific PersonGroup in Azure.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to retrieve the Person from.</param>
        /// <param name="personId">Person GUID to retrieve from PersonGroup</param>
        /// <param name="personGroupSize">PersonGroup Size</param>
        /// <returns>Boolean</returns>
        private Person PersonGetSingle(string personGroupId,
                                      Guid personId,
                                      PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to retrieve existing Person [{personId}] from the Person Group ID [{personGroupId}]...");

                Person person;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Retrieve a Person from a Large PersonGroup
                        person = faceClient.LargePersonGroupPerson.GetAsync(personGroupId, personId).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] retrieved existing Person ID [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to retrieve existing Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // Retrieve a Person from a default PersonGroup
                        person = faceClient.PersonGroupPerson.GetAsync(personGroupId, personId).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] retrieved existing Person ID [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] failed to retrieve existing Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return person;
            }

        }

        /// <summary>
        /// Returns Persons from a specific PersonGroup from Azure using the initialized default PersonGroup Size.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to retrieve list of Persons from.</param>
        /// <param name="start">Starting person id to return (used to list a range of persons).</param>
        /// <param name="top">Number of persons to return starting with the person id indicated by the 'start' parameter.</param>
        /// <returns>IList<Person></returns>
        public IList<Person> PersonGetList(string personGroupId,
                                    string start = null,
                                    int? top = null)
        {
            return PersonGetList(personGroupId, start, top, DefaultPersonGroupSize);
        }


        /// <summary>
        // Returns Persons from a specific PersonGroup from Azure.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to retrieve list of Persons from.</param>
        /// <param name="start">Starting person id to return (used to list a range of persons).</param>
        /// <param name="top">Number of persons to return starting with the person id indicated by the 'start' parameter.</param>
        /// <param name="personGroupSize">PersonGroup Size</param>
        /// <returns>IList<Person></returns>
        private IList<Person> PersonGetList(string personGroupId,
                                           string start = null,
                                           int? top = null,
                                           PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {

            if (top != null)
            {
                if (top > MaxPersonRecordsToRetrieve || top < 1)
                {
                    top = MaxPersonRecordsToRetrieve;
                }
            }

            IList<Person> personList;

            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to retrieve list of Persons from the Person Group ID [{personGroupId}]...");

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Retrieve a Person from a Large PersonGroup
                        personList = faceClient.LargePersonGroupPerson.ListAsync(personGroupId, start, top).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] retrieved {personList.Count} Person(s) successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to retrieve Persons list in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // Retrieve a Person from a default PersonGroup
                        personList = faceClient.PersonGroupPerson.ListAsync(personGroupId, start, top).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] retrieved {personList.Count} Person(s) successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] failed to retrieve Persons list in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return personList;
            }

        }

    }
}
