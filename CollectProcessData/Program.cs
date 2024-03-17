using System.Management;
using System.Runtime.Versioning;

namespace ProcessData;

[SupportedOSPlatform("windows")]
class Program
{
    // https://www.sysnet.pe.kr/2/0/1850
    // https://medium.com/oldbeedev/c-how-to-monitor-cpu-memory-disk-usage-in-windows-a06fc2f05ad5
    
    // Momory Usage
    // https://stackoverflow.com/questions/29391872/how-to-calculate-memory-usage-as-task-manager-does
    // https://devblogs.microsoft.com/scripting/why-does-my-performance-monitoring-script-keep-returning-the-same-incorrect-values/
    // https://stackoverflow.com/questions/1476018/how-to-know-the-cpu-and-memory-usage-of-a-process-with-wmi
    // https://stackoverflow.com/questions/48432100/cpu-usage-notifyicon-using-wmi
    
    // https://stackoverflow.com/questions/11523150/how-do-you-monitor-the-cpu-utilization-of-a-process-using-powershell
    
    static void Main(string[] args)
    {
        Program program = new Program();
        ProcessData processData = program.GetProcessData(544, true);
        program.PrintProcessData(processData, true);
    }

    private ProcessData? GetProcessData(UInt32 processId, bool recursive = false)
    {
        string queryText = $"select * from win32_process where processid = {processId}";

        ProcessData? processData = null;

        using (var searcher = new ManagementObjectSearcher(queryText))
        {
            foreach (ManagementBaseObject? wmiData in searcher.Get())
            {
                if (wmiData != null)
                {
                    processData = new ProcessData(wmiData);
                    if (recursive)
                    {
                        CollectChildProcessData(processData, recursive);
                    }
                }
            }
        }

        return processData;
    }

    private ProcessData? CollectChildProcessData(ProcessData processData, bool recursive = false)
    {
        string queryText = $"select * from win32_process where parentprocessid = {processData.ProcessId}";

        using (var searcher = new ManagementObjectSearcher(queryText))
        {
            foreach (ManagementBaseObject? wmiData in searcher.Get())
            {
                if (wmiData != null)
                {
                    ProcessData chidProcessData = new ProcessData(wmiData);
                    processData.AddChild(chidProcessData);
                    if (recursive)
                    {
                        CollectChildProcessData(chidProcessData, recursive);
                    }
                }
            }
        }

        return processData;
    }

    private void PrintProcessData(ProcessData processData, bool recursive = false)
    {
        Console.WriteLine("[{0}] {1}", processData.Level, processData.ToString());
        if (recursive)
        {
            foreach (ProcessData childProcessData in processData.GetChildren())
            {
                PrintProcessData(childProcessData, recursive);
            }
        }
    }
}