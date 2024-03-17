using System.Management;
using System.Runtime.Versioning;
using System.Text;

namespace ProcessData;

[SupportedOSPlatform("windows")]
public class ProcessData
{
    public ProcessData(ManagementBaseObject? wmiData)
    {
        ProcessId = WmiUtil.GetUInt32(wmiData, "ProcessId");
        ParentProcessId = WmiUtil.GetUInt32(wmiData, "ParentProcessId");
        Name = WmiUtil.GetString(wmiData, "Name");
        ExecutablePath = WmiUtil.GetString(wmiData, "ExecutablePath");
        CommandLine = WmiUtil.GetString(wmiData, "CommandLine");
        CreationDate = WmiUtil.GetDateTime(wmiData, "CreationDate");
        TerminationDate = WmiUtil.GetDateTime(wmiData, "TerminationDate");
    }
    
    public int Level => (ParentProcessData == null) ? 1 : ParentProcessData.Level + 1;
    public UInt32? ProcessId { get; }
    public UInt32? ParentProcessId { get; }
    public string? Name { get; }
    public string? ExecutablePath { get; }
    public string? CommandLine { get; }
    public DateTime? CreationDate { get; }
    public DateTime? TerminationDate { get; }

    public ProcessData? ParentProcessData { get; private set; } = null;
    private List<ProcessData> _children = new List<ProcessData>();

    public void AddChild(ProcessData child)
    {
        child.ParentProcessData = this;
        this._children.Add(child);
    }
    
    public List<ProcessData> GetChildren()
    {
        return this._children;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('{');
        sb.Append($"\"ProcessId\":{ProcessId},");
        sb.Append($"\"ParentProcessId\":{ParentProcessId},");
        sb.Append($"\"Name\":\"{Name}\",");
        sb.Append($"\"CreationDate\":{(CreationDate == null ? "null" : ((DateTime)CreationDate).ToString("yyyy-MM-dd HH:mm:ss.fff"))},");
        sb.Append($"\"TerminationDate\":{(TerminationDate == null ? "null" : ((DateTime)TerminationDate).ToString("yyyy-MM-dd HH:mm:ss.fff"))},");
        sb.Append($"\"ExecutablePath\":\"{ExecutablePath.Replace("\\","\\\\").Replace("\"","\\\"")}\",");
        sb.Append($"\"CommandLine\":\"{CommandLine.Replace("\\","\\\\").Replace("\"","\\\"")}\"");
        sb.Append('}');
        return sb.ToString();
    }
    
}