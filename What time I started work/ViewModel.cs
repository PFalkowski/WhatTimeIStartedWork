using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace What_time_I_started_work
{
    public class ViewModel : BindableBase
    {
        public string Label
        {
            get
            {
                return "Time You first opened this machine today: ";
            }
        }
        public string Content1
        {
            get
            {
                return TodaysFirstSecurityLogon();
            }
        }
        private string windowsIdentityName = null;
        public string WindowsIdentityName
        {
            get
            {
                if (windowsIdentityName == null)
                {
                    windowsIdentityName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                }
                return windowsIdentityName;
            }
        }
        public string TodaysFirstSecurityLogon()
        {
            EventLog log = EventLog.GetEventLogs().First(o => o.Log == "Security");
            var splited = WindowsIdentityName.Split('\\');
            var nameToCheckOn = splited.Last();
            var logon = log.Entries.Cast<EventLogEntry>()
                .Where(entry => entry.TimeWritten.Date == DateTime.Now.Date && entry.InstanceId == 4624 && entry.Message.Contains(nameToCheckOn))
                .OrderBy(i => i.TimeWritten)
                .FirstOrDefault();
            return logon?.TimeWritten.ToShortTimeString();
        }
    }
}
