using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFormsGeoBackground.Interfaces
{
    public interface IGPSBackgroundService
    {
        void Start();
        void Stop();
        event EventHandler ServiceStartedEvent;
    }
}
