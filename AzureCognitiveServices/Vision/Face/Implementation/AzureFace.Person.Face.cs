﻿/*
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
using System.IO;
using System.Globalization;

namespace AzureCognitiveServices
{
    /// <summary>
    /// This partial class deals with Azure Face API - PersonGroupPerson Face feature
    /// Notes:
    /// Add a face to a person into a person group for face identification or verification. To deal with an image containing multiple faces, input face can be specified as an image with a targetFace rectangle. It returns a persistedFaceId representing the added face. No image will be stored. Only the extracted face feature(s) will be stored on server until PersonGroup PersonFace - Delete, PersonGroup Person - Delete or PersonGroup - Delete is called.
    ///     Note persistedFaceId is different from faceId generated by Face - Detect.
    /// 
    ///     Higher face image quality means better recognition precision.Please consider high-quality faces: frontal, clear, and face size is 200x200 pixels (100 pixels between eyes) or bigger.
    ///     Each person entry can hold up to 248 faces.
    ///        JPEG, PNG, GIF (the first frame), and BMP format are supported.The allowed image file size is from 1KB to 6MB.
    ///     "targetFace" rectangle should contain one face.Zero or multiple faces will be regarded as an error.If the provided "targetFace" rectangle is not returned from Face - Detect, there’s no guarantee to detect and add the face successfully.
    /// 
    ///     Out of detectable face size (36x36 - 4096x4096 pixels), large head-pose, or large occlusions will cause failures.
    ///    Adding/deleting faces to/from a same person will be processed sequentially. Adding/deleting faces to/from different persons are processed in parallel.
    ///     The minimum detectable face size is 36x36 pixels in an image no larger than 1920x1080 pixels. Images with dimensions higher than 1920x1080 pixels will need a proportionally larger minimum face size.
    /// 
    ///    Different 'detectionModel' values can be provided. To use and compare different detection models, please refer to How to specify a detection model
    ///         'detection_01': The default detection model for PersonGroup Person - Add Face. Recommend for near frontal face detection. For scenarios with exceptionally large angle (head-pose) faces, occluded faces or wrong image orientation, the faces in such cases may not be detected.
    ///         'detection_02': Detection model released in 2019 May with improved accuracy especially on small, side and blurry faces.
    ///         'detection_03': Detection model released in 2021 February with improved accuracy especially on small faces.

    /// </summary>
    public partial class AzureFace
    {
        /// <summary>
        /// Add Face to Person in PersonGroup using an image file for the face.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to add Face to</param>
        /// <param name="personId">Person ID to add Face to</param>
        /// <param name="personFaceImageFile">Face Image File</param>
        /// <param name="personFaceNotes">Face Notes</param>
        /// <returns>PersistedFace</returns>
        public PersistedFace PersonFaceAddFromFile(string personGroupId,
                                            Guid personId,
                                            string personFaceImageFile,
                                            string personFaceNotes = null)
        {
            using (Stream stream = ConvertByteArrayToStream(ConvertImageToByteArray(personFaceImageFile)))
            {
                return PersonFaceAddFromStream(personGroupId, personId, stream, personFaceNotes, DefaultPersonGroupSize);
            }
        }

        /// <summary>
        /// Add Face to Person in PersonGroup using an image stream for the face.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to add Face to</param>
        /// <param name="personId">Person ID to add Face to</param>
        /// <param name="personFaceImageStream">Face Image Stream Object</param>
        /// <param name="personFaceNotes">Face Notes</param>
        /// <returns>PersistedFace</returns>
        public PersistedFace PersonFaceAddFromStream(string personGroupId,
                                            Guid personId,
                                            Stream personFaceImageStream,
                                            string personFaceNotes = null)
        {
            return PersonFaceAddFromStream(personGroupId, personId, personFaceImageStream, personFaceNotes, DefaultPersonGroupSize);
        }


        /// <summary>
        /// Add Face to Person in PersonGroup using an image stream for the face.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to add Face to</param>
        /// <param name="personId">Person ID to add Face to</param>
        /// <param name="personFaceImageFileStream">Face Image Stream Object</param>
        /// <param name="personFaceNotes">Face Notes</param>
        /// <returns>PersistedFace</returns>
        private PersistedFace PersonFaceAddFromStream(string personGroupId,
                                                      Guid personId,
                                                      Stream personFaceImageFileStream,
                                                      string personFaceNotes = null,
                                                      PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to add new Face to Person [{personId}] in the Person Group ID [{personGroupId}] from image stream...");

                PersistedFace personFace;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Add Person Face to large PersonGroup
                        personFace = faceClient.LargePersonGroupPerson.AddFaceFromStreamAsync(personGroupId, personId, personFaceImageFileStream, personFaceNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] added new Face [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to add a new Person Face to Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }
                else
                {
                    try
                    {
                        // Add Person Face to default PersonGroup
                        personFace = faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId, personId, personFaceImageFileStream, personFaceNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] added new Face [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to add a new Person Face to Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        return null;
                    }

                }

                return personFace;
            }
        }


        /// <summary>
        /// Add Face to Person in PersonGroup using an image URI for the face.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to add Face to</param>
        /// <param name="personId">Person ID to add Face to</param>
        /// <param name="personFaceImageFileUri">Face Image URI</param>
        /// <param name="personFaceNotes">Face Notes</param>
        /// <returns>PersistedFace</returns>
        public PersistedFace PersonFaceAddFromUri(string personGroupId,
                                                  Guid personId,
                                                  string personFaceImageFileUri,
                                                  string personFaceNotes = null)
        {
            return PersonFaceAddFromUri(personGroupId, personId, personFaceImageFileUri, personFaceNotes, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Add Face to Person in PersonGroup using an image URI for the face.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to add Face to</param>
        /// <param name="personId">Person ID to add Face to</param>
        /// <param name="personFaceImageFileUri">Face Image URI</param>
        /// <param name="personFaceNotes">Face Notes</param>
        /// <returns>PersistedFace</returns>
        private PersistedFace PersonFaceAddFromUri(string personGroupId,
                                                   Guid personId,
                                                   string personFaceImageFileUri,
                                                   string personFaceNotes = null,
                                                   PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to add new Face to Person [{personId}] in the Person Group ID [{personGroupId}] from image URI [{personFaceImageFileUri}]...");

                PersistedFace personFace;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Add Person Face to large PersonGroup
                        personFace = faceClient.LargePersonGroupPerson.AddFaceFromUrlAsync(personGroupId, personId, personFaceImageFileUri, personFaceNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] added new Face [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to add a new Person Face to Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }
                else
                {
                    try
                    {
                        // Add Person Face to default PersonGroup
                        personFace = faceClient.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, personId, personFaceImageFileUri, personFaceNotes).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] added new Face [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to add a new Person Face to Person ID [{personId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }

                return personFace;
            }
        }

        /// <summary>
        /// Returns a PresistedFace from a PersonGroup based on the PersonGroup ID, Person ID, and Face ID.
        /// </summary>
        /// <param name="personGroupId">Person Group ID</param>
        /// <param name="personId">Person ID</param>
        /// <param name="faceId">Face ID</param>
        /// <returns>PersistedFace</returns>
        public PersistedFace PersonFaceGetSingle(string personGroupId,
                                                   Guid personId,
                                                   Guid faceId)
        {
            return PersonFaceGetSingle(personGroupId, personId, faceId, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Returns a PresistedFace from a PersonGroup based on the PersonGroup ID, Person ID, and Face ID.
        /// </summary>
        /// <param name="personGroupId">Person Group ID</param>
        /// <param name="personId">Person ID</param>
        /// <param name="faceId">Face ID</param>
        /// <param name="personGroupSize">Person Group Size</param>
        /// <returns>PersistedFace</returns>
        private PersistedFace PersonFaceGetSingle(string personGroupId,
                                                   Guid personId,
                                                   Guid faceId,
                                                   PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting to retrieve a single Face ID [{faceId}] from Person [{personId}] from the Person Group ID [{personGroupId}]...");

                PersistedFace personFace;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Retrieve single Person Face from large PersonGroup
                        personFace = faceClient.LargePersonGroupPerson.GetFaceAsync(personGroupId, personId, faceId).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] retrieve single Face ID [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Large Person Group [{personGroupId}] failed to retrieve single Person Face [{faceId}] from Person ID [{personId}] from Person Group ID [{personGroupId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }
                else
                {
                    try
                    {
                        // Retrieve single Person Face from default PersonGroup
                        personFace = faceClient.PersonGroupPerson.GetFaceAsync(personGroupId, personId, faceId).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] retrieve single Face ID [{personFace.PersistedFaceId}] to Person [{personId}] successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Default Person Group [{personGroupId}] failed to retrieve single Person Face [{faceId}] from Person ID [{personId}] from Person Group ID [{personGroupId}] in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }

                return personFace;
            }
        }

        /// <summary>
        /// Identify a single face from a PersonGroup.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to try to identify the Face from</param>
        /// <param name="imagePath">The image path to detect the faces</param>
        /// <returns>Person</returns>
        public Person PersonFaceIdentifyFromFile(string personGroupId,
                                                 string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new Exception("Image file does not exist!");
            }

            using (Stream stream = ConvertByteArrayToStream(ConvertImageToByteArray(imagePath)))
            {
                return PersonFaceIdentifyFromStream(personGroupId, stream);
            }
        }

        /// <summary>
        /// Identify a single face from a PersonGroup.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to try to identify the Face from</param>
        /// <param name="personFaceImageFileStream">Single Face Stream. If you pass stream with multiple faces, the method will throw an exception.</param>
        /// <returns>Person</returns>
        public Person PersonFaceIdentifyFromStream(string personGroupId,
                                                   Stream personFaceImageFileStream)
        {
            return PersonFaceIdentifyFromStreamWithPersonGroupSize(personGroupId, personFaceImageFileStream, DefaultPersonGroupSize);
        }

        /// <summary>
        /// Identify a single face from a PersonGroup.
        /// </summary>
        /// <param name="personGroupId">PersonGroup ID to try to identify the Face from</param>
        /// <param name="personFaceImageFileStream">Single Face Stream. If you pass stream with multiple faces, the method will throw an exception.</param>
        /// <param name="personGroupSize">Person Group Size</param>
        /// <returns>Person</returns>
        private Person PersonFaceIdentifyFromStreamWithPersonGroupSize(string personGroupId,
                                                           Stream personFaceImageFileStream,
                                                           PersonGroupSize personGroupSize = PersonGroupSize.Default)
        {
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                Debug.Print($"Attempting Detect Person's Face from image stream...");

                IList<DetectedFace> detectedFaces = DetectFromStream(personFaceImageFileStream);
                

                if (detectedFaces == null)
                {
                    Debug.Print("No faces detected in stream!");
                    return null;
                }

                if (detectedFaces.Count > 1)
                {
                    Debug.Print("More than 1 face detected in stream! This is not allowed in Single Person Identification.");
                    throw new Exception("More than 1 face detected in stream! This is not allowed in Single Person Identification.");
                }

                List<Guid> sourceFaceIds = new List<Guid>();
                sourceFaceIds.Add(detectedFaces[0].FaceId.Value);

                IList<IdentifyResult> identifyResults;

                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                if (personGroupSize == PersonGroupSize.LargeGroup)
                {
                    try
                    {
                        // Add Person Face to large PersonGroup
                        identifyResults = faceClient.Face.IdentifyAsync(sourceFaceIds, null, personGroupId).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Identify Face in Large Person Group [{personGroupId}] finished successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Identify Face in Large Person Group [{personGroupId}] failed in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }
                else
                {
                    try
                    {
                        // Add Person Face to large PersonGroup
                        identifyResults = faceClient.Face.IdentifyAsync(sourceFaceIds, personGroupId, null).GetAwaiter().GetResult();

                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Identify Face in Person Group [{personGroupId}] finished successfully in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop(); // Stop Stopwatch
                        Debug.Print($"Identify Face in Person Group [{personGroupId}] failed in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}. Returned Error: {ex.Message}.");
                        throw ex;
                    }

                }

                //TODO: Rethink the return value as when we return null, the called does not know if the face was identified with low confidence or it was not identified at all!
                if (identifyResults == null)
                {
                    Debug.Print($"Could not identify face in stream from PersonGroup ID: [{personGroupId}].");
                    return null;
                }

                if (identifyResults.Count > 1)
                {
                    Debug.Print($"Multiple faces identified in stream from PersonGroup ID: [{personGroupId}]. This is not allowed in this method.");
                    throw new Exception($"Multiple faces identified in stream from PersonGroup ID: [{personGroupId}]. This is not allowed in this method.");
                }

                if (identifyResults[0].Candidates == null || identifyResults[0].Candidates.Count == 0)
                {
                    Debug.Print($"Could not identify face in stream from PersonGroup ID: [{personGroupId}].");
                    return null;
                }

                if (identifyResults[0].Candidates[0].Confidence >= MinFaceIdentificationConfidence)
                {
                    Debug.Print($"Face identified successfully from image stream from PersonGroup ID: [{personGroupId}] as Person ID: [{identifyResults[0].Candidates[0].PersonId}] with {identifyResults[0].Candidates[0].Confidence.ToString("P", CultureInfo.InvariantCulture)} confidence rate.");
                    return PersonGetSingle(personGroupId, identifyResults[0].Candidates[0].PersonId); // Return the first candidate as the first one is the highest confidence candidate.
                }
                else
                {
                    Debug.Print($"Face was identified with low confidence from PersonGroup ID: [{personGroupId}] as Person ID: [{identifyResults[0].Candidates[0].PersonId}] with {identifyResults[0].Candidates[0].Confidence.ToString("P", CultureInfo.InvariantCulture)} confidence rate. The AI is not sure this is the right person. Returning NULL.");
                    return null;
                }

            }
        }

    }
}
