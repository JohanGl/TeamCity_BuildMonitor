using System;
using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
    public class OctopusEnvironment
    {
        public String Name { get; set; }
        public String Id { get; set; }
        public List<OctopusItem> OctopusItems { get; set; }

        public OctopusEnvironment()
        {
            OctopusItems = new List<OctopusItem>();
        }
        
    }
}