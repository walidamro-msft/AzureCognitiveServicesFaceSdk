﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaceIdentificationWpfExample {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class AzureCognitiveServiceSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AzureCognitiveServiceSettings defaultInstance = ((AzureCognitiveServiceSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AzureCognitiveServiceSettings())));
        
        public static AzureCognitiveServiceSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://???.cognitiveservices.azure.com")]
        public string AzureVisionFaceEndPoint {
            get {
                return ((string)(this["AzureVisionFaceEndPoint"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("???")]
        public string AzureVisionFaceApiKey {
            get {
                return ((string)(this["AzureVisionFaceApiKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("recognition_04")]
        public string AzureVisionFaceRecognitionModel {
            get {
                return ((string)(this["AzureVisionFaceRecognitionModel"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.85")]
        public double AzureVisionFaceMinFaceIdentificationConfidenceRate {
            get {
                return ((double)(this["AzureVisionFaceMinFaceIdentificationConfidenceRate"]));
            }
        }
    }
}
