using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using BuildMonitor.Models.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildMonitor.Helpers
{
    public class OctopusHandler : IOctopusHandler
    {
        private dynamic json;
        public string GetJson()
        {
            return HttpGet();
        }

        public OctopusMonitorViewModel GetModel()
        {
            json = JsonConvert.DeserializeObject<dynamic>(GetJson());
            var model = new OctopusMonitorViewModel();

            var projects = json.Projects;
            foreach (var project in projects)
            {
                var octopusProject = new OctopusProject
                {
                    Id = project.Id,
                    Name = project.Name
                };

                List<dynamic> environmentList = json.Environments.ToObject<List<dynamic>>();
                List<dynamic> currentEnvironmentList = project.EnvironmentIds.ToObject<List<dynamic>>();
                List<dynamic> itemList = json.Items.ToObject<List<dynamic>>();

                octopusProject.OctopusEnvironments.AddRange(environmentList
                    .Where(e => currentEnvironmentList.Contains(e.Id.ToString()))
                    .Select(e => new OctopusEnvironment()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        OctopusItem = itemList
                            .Select(i => new OctopusItem()
                            {
                                EnvironmentId = i.EnvironmentId,
                                Id = i.Id,
                                ProjectId = i.ProjectId,
                                State = i.State,
                                ReleaseVersion = i.ReleaseVersion
                            })
                            .FirstOrDefault(i => (i.ProjectId.Equals(project.Id.ToString()) && i.EnvironmentId.Equals( e.Id.ToString())))
                    }));

                model.OctopusProjects.Add(octopusProject);
            }



            return model;
        }

        public string HttpGet()
        {
            var octopusKey = ConfigurationManager.AppSettings["octopus_api_key"];
            var octopusUrl = ConfigurationManager.AppSettings["octopus_api_url"];

            WebClient client = new WebClient();
            client.Headers.Add("X-Octopus-ApiKey", octopusKey);
            Stream data = client.OpenRead(octopusUrl);
            if (data == null) return " ";
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return s;
        }
    }

    public interface IOctopusHandler
    {
        string GetJson();
        OctopusMonitorViewModel GetModel();
    }
}