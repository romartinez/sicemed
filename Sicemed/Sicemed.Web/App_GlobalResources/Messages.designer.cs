//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Messages", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} no es válido..
        /// </summary>
        internal static string CustomValidationAttribute_ValidationError {
            get {
                return ResourceManager.GetString("CustomValidationAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} debe ser una dirección de email válida..
        /// </summary>
        internal static string EmailAttribute_ValidationError {
            get {
                return ResourceManager.GetString("EmailAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {1} es inválido..
        /// </summary>
        internal static string PropertyValueInvalid {
            get {
                return ResourceManager.GetString("PropertyValueInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} es requerido..
        /// </summary>
        internal static string PropertyValueRequired {
            get {
                return ResourceManager.GetString("PropertyValueRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} debe encontrarse entre los valores {1} y {2}..
        /// </summary>
        internal static string RangeAttribute_ValidationError {
            get {
                return ResourceManager.GetString("RangeAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} debe validar la siguiente expresión regular &apos;{1}&apos;..
        /// </summary>
        internal static string RegexAttribute_ValidationError {
            get {
                return ResourceManager.GetString("RegexAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {1} es requerido..
        /// </summary>
        internal static string RequiredAttribute_ValidationError {
            get {
                return ResourceManager.GetString("RequiredAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} debe ser una cadena de texto con una longitud máxima de {1}..
        /// </summary>
        internal static string StringLengthAttribute_ValidationError {
            get {
                return ResourceManager.GetString("StringLengthAttribute_ValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} debe ser una cadena de texto con una longitud mínima de {2} y máxima de {1}..
        /// </summary>
        internal static string StringLengthAttribute_ValidationErrorIncludingMinimum {
            get {
                return ResourceManager.GetString("StringLengthAttribute_ValidationErrorIncludingMinimum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo {0} no es válido..
        /// </summary>
        internal static string ValidationAttribute_ValidationError {
            get {
                return ResourceManager.GetString("ValidationAttribute_ValidationError", resourceCulture);
            }
        }
    }
}