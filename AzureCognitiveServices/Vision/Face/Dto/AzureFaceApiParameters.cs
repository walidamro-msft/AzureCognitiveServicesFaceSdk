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

namespace AzureCognitiveServices
{
    public class AzureFaceApiParameters : IAzureFaceApiParameters
    {
        /// <summary>
        /// Azure Face API Endpoint (you get it from the Azure Portal)
        /// </summary>
        public string FaceEndpoint { get; set; }

        /// <summary>
        /// Azure Face API Key (you get it from the Azure Portal)
        /// </summary>
        public string FaceApiKey { get; set; }

        /// <summary>
        /// Default Azure Face Recognition Model. 
        /// Recognition model 4 was released in 2021 February. It is recommended since its accuracy is improved on faces wearing masks compared with model 3, and its overall accuracy is improved compared with models 1 and 2.
        /// Use Microsoft.Azure.CognitiveServices.Vision.Face.Models.RecognitionModel Enumerator for all available recognition models.
        /// </summary>
        public string DefaultRecognitionModel { get; set; } = RecognitionModel.Recognition04;

        /// <summary>
        /// Default Azure Face Detection Model. By default model 3 is the selected detection model. Use Microsoft.Azure.CognitiveServices.Vision.Face.Models.DetectionModel Enumerator for all available detection models.
        /// </summary>
        public string DefaultDetectionModel { get; set; } = null;

        /// <summary>
        /// Default PersonGroupSize. By default it is a regular size and not large.
        /// </summary>
        public PersonGroupSize DefaultPersonGroupSize { get; set; } = PersonGroupSize.Default;

        /// <summary>
        /// Minimum Face Identification Confidence Rate
        /// </summary>
        public double MinFaceIdentificationConfidenceRate { get; set; } = 0.90;  // Default 90% Confidence Rate
    }
}
