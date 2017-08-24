using System;
using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
    public class OctopusEnvironment
    {
        public String Name { get; set; }
        public String Id { get; set; }
        public OctopusItem OctopusItem { get; set; }
    }
}