using System;
using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
    public class OctopusMonitorViewModel
    {
        public List<OctopusProject> OctopusProjects { get; set; }

        public OctopusMonitorViewModel()
        {
            OctopusProjects = new List<OctopusProject>();
        }
    }
}