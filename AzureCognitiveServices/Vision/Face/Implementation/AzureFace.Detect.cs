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
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Diagnostics;

namespace AzureCognitiveServices
{
    /// <summary>
    /// This partial class deals with Azure Face API - Detect feature
    /// </summary>
    public partial class AzureFace
    {
        /// <summary>
        /// Detects all the faces in an image from a URI and returns all the faces metadata.
        /// </summary>
        /// <param name="imageUri">The image URI to detect the faces</param>
        /// <param name="faceAttributes">List of face attributes to return</param>
        /// <returns>IList<DetectedFace> List of detected faces metadata</returns>
        public IList<DetectedFace> Detect(Uri imageUri, IList<FaceAttributeType> faceAttributes = null)
        {
            
            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                // List of detected faces
                IList<DetectedFace> faceList;

                Debug.Print("Attempting to detect faces using URL...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                faceList = faceClient.Face.DetectWithUrlAsync(imageUri.ToString(), true, true, faceAttributes, DefaultRecognitionModel, true).Result;

                sw.Stop(); // Stop Stopwatch
                Debug.Print($"Face(s) detected from an URL image in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");

                return faceList;
            }

        }

        /// <summary>
        /// Detects all the faces in an image from a file path and returns all the faces metadata.
        /// </summary>
        /// <param name="imagePath">The image path to detect the faces</param>
        /// <param name="faceAttributes">List of face attributes to return</param>
        /// <returns>IList<DetectedFace> List of detected faces metadata</returns>
        public IList<DetectedFace> Detect(string imagePath, IList<FaceAttributeType> faceAttributes = null)
        {
            if (!File.Exists(imagePath))
            {
                throw new Exception("Image file does not exist!");
            }

            using (Stream stream = ConvertByteArrayToStream(ConvertImageToByteArray(imagePath)))
            {
                return DetectFromStream(stream, faceAttributes);
            }

        }

        /// <summary>
        /// Detects all the faces in an image stream and returns all the faces metadata.
        /// </summary>
        /// <param name="imageStream">The image stream to detect the faces</param>
        /// <param name="faceAttributes">List of face attributes to return</param>
        /// <returns>IList<DetectedFace> List of detected faces metadata</returns>
        private IList<DetectedFace> DetectFromStream(Stream imageStream, IList<FaceAttributeType> faceAttributes = null)
        {
            if (imageStream == null)
            {
                throw new Exception("Empty Image Stream!");
            }

            using (IFaceClient faceClient = CreateFaceClient(FaceEndpoint, FaceApiKey))
            {
                // List of detected faces
                IList<DetectedFace> faceList;

                Debug.Print("Attempting to detect faces by image stream...");
                Stopwatch sw = new Stopwatch(); // Create Stopwatch
                sw.Restart();   // Start Stopwatch

                faceList = faceClient.Face.DetectWithStreamAsync(imageStream, true, true, faceAttributes, DefaultRecognitionModel, true).Result;

                sw.Stop(); // Stop Stopwatch
                Debug.Print($"Face(s) detected from an URL image in {ReadableElapsedMilliseconds(sw.ElapsedMilliseconds)}...");

                return faceList;
            }



        }
    }
}
