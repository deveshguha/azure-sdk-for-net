// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.StorSimple1200Series.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for SupportedDeviceCapabilities.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SupportedDeviceCapabilities
    {
        [EnumMember(Value = "Invalid")]
        Invalid,
        [EnumMember(Value = "FileServer")]
        FileServer,
        [EnumMember(Value = "IscsiServer")]
        IscsiServer
    }
    internal static class SupportedDeviceCapabilitiesEnumExtension
    {
        internal static string ToSerializedValue(this SupportedDeviceCapabilities? value)
        {
            return value == null ? null : ((SupportedDeviceCapabilities)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this SupportedDeviceCapabilities value)
        {
            switch( value )
            {
                case SupportedDeviceCapabilities.Invalid:
                    return "Invalid";
                case SupportedDeviceCapabilities.FileServer:
                    return "FileServer";
                case SupportedDeviceCapabilities.IscsiServer:
                    return "IscsiServer";
            }
            return null;
        }

        internal static SupportedDeviceCapabilities? ParseSupportedDeviceCapabilities(this string value)
        {
            switch( value )
            {
                case "Invalid":
                    return SupportedDeviceCapabilities.Invalid;
                case "FileServer":
                    return SupportedDeviceCapabilities.FileServer;
                case "IscsiServer":
                    return SupportedDeviceCapabilities.IscsiServer;
            }
            return null;
        }
    }
}
