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

using AzureCognitiveServices;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using System.Runtime.InteropServices.WindowsRuntime;

namespace FaceIdentificationWpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly AzureCognitiveServiceSettings settings = new AzureCognitiveServiceSettings();

        private readonly string PersonGroupId = "ddc65ef6-298d-41a7-964c-d206f26256d4";
        private readonly string PersonGroupName = "PARTechFaceApiDemo";

        private static IAzureFaceApiParameters azureFaceApiParameters = new AzureFaceApiParameters()
        {
            FaceEndpoint = settings.AzureVisionFaceEndPoint,
            FaceApiKey = settings.AzureVisionFaceApiKey,
            DefaultRecognitionModel = settings.AzureVisionFaceRecognitionModel,
            DefaultDetectionModel = DetectionModel.Detection01,
            DefaultPersonGroupSize = PersonGroupSize.Default,
            MinFaceIdentificationConfidenceRate = settings.AzureVisionFaceMinFaceIdentificationConfidenceRate
        };

        private static AzureFace azureFace = new AzureFace(azureFaceApiParameters);

        //public Collection<EncoderDevice> AudioDevices { get; set; }
        MediaCapture mediaCaptureMgr;
        bool CameraOpen = false;


        public MainWindow()
        {
            InitializeComponent();

            //azureFace.PersonGroupDelete(PersonGroupId);
            FacePersonGroup facePersonGroup = azureFace.PersonGroupGetSingle(PersonGroupId);

            if (facePersonGroup == null)
            {
                Debug.Print($"Azure Face PersonGroup ID [{PersonGroupId}] with Name [{PersonGroupName}] is not created. Will attempt to create it now...");
                if (azureFace.PersonGroupCreate(PersonGroupId, PersonGroupName))
                {
                    Debug.Print($"Azure Face PersonGroup ID [{PersonGroupId}] with Name [{PersonGroupName}] was created successfully...");
                }
            }
            else
            {
                Debug.Print($"Azure Face PersonGroup ID [{PersonGroupId}] with Name [{PersonGroupName}] was already created...");
                IList<Person> persons = azureFace.PersonGetList(PersonGroupId);

                if (persons.Count == 0)
                {
                    Debug.Print("No Persons in this PersonGroup yet.");
                }
                else
                {
                    Debug.Print($"{persons.Count} Person(s) found in the PersonGroup Name [{PersonGroupName}]:");
                    int i = 0;

                    foreach (Person person in persons)
                    {
                        Debug.Print($"{++i}. {person.Name}");
                    }
                }
                
            }
            

        }


        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CameraOpen)
            {
                await mediaCaptureMgr.StopPreviewAsync();
                mediaCaptureMgr.Dispose();
                mediaCaptureMgr = null;
                CameraOpen = false;
            }

        }

        private async void OpenCameraButton_Click(object sender, RoutedEventArgs e)
        {

            if (!CameraOpen)
            {
                mediaCaptureMgr = new MediaCapture();
                MediaCaptureInitializationSettings mediaCaptureInitializationSettings = new MediaCaptureInitializationSettings();
                KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>)MyWebCams.SelectedItem;
                mediaCaptureInitializationSettings.VideoDeviceId = selectedItem.Value;
                await mediaCaptureMgr.InitializeAsync(mediaCaptureInitializationSettings);

                ((CaptureElement)MyCaptureElement.Child).Source = mediaCaptureMgr;
                await mediaCaptureMgr.StartPreviewAsync();
                CameraOpen = true;
            }

        }

        public async Task<IList<DeviceInfo>> GetVideoProfileSupportedDevicesListAsync()
        {
            List<DeviceInfo> devicesList = new List<DeviceInfo>();

            // Finds all video capture devices
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            foreach (var device in devices)
            {
                
                // Check if the device on the requested panel supports Video Profile
                if (MediaCapture.IsVideoProfileSupported(device.Id))
                {
                    // We've located a device that supports Video Profiles on expected panel
                    devicesList.Add(new DeviceInfo { Id = device.Id, Name = device.Name });
                }
            }

            return devicesList;
        }

        public static async Task<IList<DeviceInfo>> GetVideoDevicesListAsync()
        {
            List<DeviceInfo> devicesList = new List<DeviceInfo>();

            // Finds all video capture devices
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            foreach (var device in devices)
            {
                devicesList.Add(new DeviceInfo { Id = device.Id, Name = device.Name });
            }

            return devicesList;
        }

        public static async Task<byte[]> ConvertInMemoryRandomAccessStreamToByteArray(InMemoryRandomAccessStream inMemoryRandomAccessStream)
        {
            // This is where the byteArray to be stored.
            byte[] bytes = new byte[inMemoryRandomAccessStream.Size];

            await inMemoryRandomAccessStream.ReadAsync(bytes.AsBuffer(), (uint)inMemoryRandomAccessStream.Size, InputStreamOptions.None);

            return bytes;
        }

        private async void EnrollPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraOpen)
            {
                ImageEncodingProperties imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
                string desiredName = $"~{AzureFace.GenerateNewGuidString()}.jpg";
                StorageFile storageFile = await KnownFolders.CameraRoll.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);

                await mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageEncodingProperties, storageFile);
                

                MyDialog dialog = new MyDialog();

                if (dialog.ShowDialog() == true)
                {
                    // First Make sure the Person is already enrolled or not
                    Person enrolledPerson = azureFace.PersonFaceIdentifyFromFile(PersonGroupId, System.IO.Path.Combine(KnownFolders.CameraRoll.Path, desiredName));

                    if (enrolledPerson == null)
                    {
                        Debug.Print($"This person is not enrolled yet! Attempting to enroll [{dialog.ResponseText}]");

                        Person newPerson = azureFace.PersonCreate(PersonGroupId, dialog.ResponseText);

                        if (newPerson != null)
                        {
                            Debug.Print($"Person [{dialog.ResponseText}] with of ID: [{newPerson.PersonId}] was created successfully...");

                            PersistedFace persistedFace = azureFace.PersonFaceAddFromFile(PersonGroupId, newPerson.PersonId, System.IO.Path.Combine(KnownFolders.CameraRoll.Path, desiredName));

                            if (persistedFace != null)
                            {
                                if (azureFace.PersonGroupTrain(PersonGroupId))
                                {
                                    Debug.Print($"Added new Face to Person [{dialog.ResponseText}] with Face ID: [{persistedFace.PersistedFaceId}] successfully...");
                                    MessageBox.Show($"Person [{dialog.ResponseText}] has been enrolled successfully...");
                                }
                                else
                                {
                                    Debug.Print($"Failed to Train PersonGroup [{PersonGroupName}]!");
                                    MessageBox.Show($"Failed to add new Face to Person [{dialog.ResponseText}]!");
                                }
                            }
                            else
                            {
                                Debug.Print($"Failed to add new Face to Person [{newPerson.Name}]!");

                                if (azureFace.PersonDelete(PersonGroupId, newPerson.PersonId))
                                {
                                    Debug.Print($"Deleted Person [{dialog.ResponseText}]!");
                                }

                                MessageBox.Show($"Failed to add new Face to Person [{dialog.ResponseText}]!");
                            }
                        }
                    }
                    else
                    {
                        
                        if (enrolledPerson.Name.Equals(dialog.ResponseText))
                        {
                            Debug.Print($"This person was found in the PersonGroup.");
                            Debug.Print($"Person [{enrolledPerson.Name}] already enrolled!");
                            MessageBox.Show($"Person [{enrolledPerson.Name}] already enrolled!");
                        }
                        else
                        {
                            Debug.Print($"Person [{dialog.ResponseText}] was found in the PersonGroup, but under [{enrolledPerson.Name}] Person Name!");
                            MessageBox.Show($"Person [{dialog.ResponseText}] already enrolled under a different name!");
                        }
                    }

                    
                }


            }
            else
            {
                MessageBox.Show("Please open a camera first!");
            }
         }

        private async void StopCameraButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraOpen)
            {
                await mediaCaptureMgr.StopPreviewAsync();
                mediaCaptureMgr.Dispose();
                mediaCaptureMgr = null;
                CameraOpen = false;
            }
        }

        private async void IdentifyPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraOpen)
            {
                ImageEncodingProperties imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
                string desiredName = $"~{AzureFace.GenerateNewGuidString()}.jpg";
                StorageFile storageFile = await KnownFolders.CameraRoll.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);

                await mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageEncodingProperties, storageFile);


                // First Make sure the Person is already enrolled or not
                Person enrolledPerson = azureFace.PersonFaceIdentifyFromFile(PersonGroupId, System.IO.Path.Combine(KnownFolders.CameraRoll.Path, desiredName));

                if (enrolledPerson == null)
                {
                    Debug.Print("Cannot identify this person!");
                    MessageBox.Show("Cannot identify this person!");
                }
                else
                {
                    Debug.Print($"This person was found in the PersonGroup.");
                    Debug.Print($"Person was identified as [{enrolledPerson.Name}].");
                    MessageBox.Show($"Welcome {enrolledPerson.Name}");
                }

            }
            else
            {
                MessageBox.Show("Please open a camera first!");
            }
        }

        private async void MyWebCams_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            bool populated = await PopulateWebCamsComboBoxAsync();

            if (!populated)
            {
                MessageBox.Show("No WebCam found on this computer!");
            }

        }

        private async Task<bool> PopulateWebCamsComboBoxAsync()
        {
            try
            {
                IList<DeviceInfo> devices = await GetVideoDevicesListAsync();

                if (devices.Count == 0)
                {
                    MessageBox.Show("You do not have any video streaming device on this computer! Cannot continue.");
                    return false;
                }

                //MyWebCams.ItemsSource = devices;
                KeyValuePair<string, string> keyValuePair;

                MyWebCams.SelectedValuePath = "Key";
                MyWebCams.DisplayMemberPath = "Key";

                foreach (DeviceInfo device in devices)
                {
                    MyWebCams.Items.Add(new KeyValuePair<string, string>(device.Name, device.Id));
                }

                MyWebCams.SelectedIndex = 0;
                return true;

            }
            catch (Exception ex)
            {
                Debug.Print($"Error while populating WebCams list! Error Message: {ex.Message}");
                return false;
            }
            
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            bool populated = await PopulateWebCamsComboBoxAsync();

            if (!populated)
            {
                MessageBox.Show("No WebCam found on this computer!");
            }
        }




        //BitmapImage BitmapToImageSource(Bitmap bitmap)
        //{
        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        //        memory.Position = 0;
        //        BitmapImage bitmapimage = new BitmapImage();
        //        bitmapimage.BeginInit();
        //        bitmapimage.StreamSource = memory;
        //        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapimage.EndInit();

        //        return bitmapimage;
        //    }
        //}
    }

}
