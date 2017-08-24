using System;
using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
    public class OctopusProject
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public List<OctopusEnvironment> OctopusEnvironments { get; set; }
        public OctopusProject()
        {
             OctopusEnvironments = new List<OctopusEnvironment>();
        }
    }
}