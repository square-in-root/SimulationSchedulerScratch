using System.Management;
using System.Runtime.Versioning;

namespace ProcessData;

[SupportedOSPlatform("windows")]
public static class WmiUtil
{
    public static string? GetString(ManagementBaseObject? wmiData, string propertyName)
    {
        return (string?)wmiData?.GetPropertyValue(propertyName);
    }
    
    public static int? GetInt(ManagementBaseObject? wmiData, string propertyName)
    {
        object value = wmiData.GetPropertyValue(propertyName);
        return (value == null) ? null : Convert.ToInt32(value);
    }
    
    public static UInt32? GetUInt32(ManagementBaseObject? wmiData, string propertyName)
    {
        object value = wmiData.GetPropertyValue(propertyName);
        return (value == null) ? null : Convert.ToUInt32(value);
    }
    
    public static UInt64? GetUInt64(ManagementBaseObject? wmiData, string propertyName)
    {
        object value = wmiData.GetPropertyValue(propertyName);
        return (value == null) ? null : Convert.ToUInt64(value);
    }

    public static DateTime? GetDateTime(ManagementBaseObject? wmiData, string propertyName)
    {
        object value = wmiData.GetPropertyValue(propertyName);
        return (value == null) ? null : ManagementDateTimeConverter.ToDateTime((string) value);
    }
    
}