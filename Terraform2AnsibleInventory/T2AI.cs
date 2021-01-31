using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraform2AnsibleInventory.Models;

namespace Terraform2AnsibleInventory
{
    public class T2AI
    {
        private string tfstateFile = "terraform.tfstate";
        private RootObject RootObject { get; set; }
        public StringBuilder sb {get;set; }

        public bool PreflightCheck(string inputFile)
        {
            if (System.IO.File.Exists(inputFile))
            {
                tfstateFile = inputFile;
                return true;
            }
            else
            {
                Console.WriteLine("Please set full path incl. terraform.tfstate as parameter if not in current work folder.");
                return false;
            }
        }

        public bool LoadJson()
        {
            try
            {
                RootObject = JsonConvert.DeserializeObject<RootObject>(System.IO.File.ReadAllText(tfstateFile));
                return true;
            }
            catch
            {
                Console.WriteLine("terraform.tfstate is invalid or could not be read.");
                return false;
            }
        }

        public bool SaveToFile(string fileName)
        {
            var tags = new List<Tag>();
            sb = new StringBuilder();
            sb.Clear();

            foreach (var resource in RootObject.Resources.Where(c => c.Type.ToLower() == "hcloud_server"))
            {
                foreach (var instance in resource.Instances)
                {
                    var attribute = instance.Attributes;

                    string ip = attribute.Ipv4_address;
                    if (attribute?.Labels != null)
                    {
                        foreach (var key in attribute.Labels)
                        {
                            string tag = key.Key;

                            if (!tags.Any(c => c.TagName == tag))
                            {
                                tags.Add(new Tag
                                {
                                    TagName = tag,
                                    IpAddress = new List<string> { ip }
                                });
                            }
                            else
                            {
                                var tagObj = tags.Single(c => c.TagName == tag);
                                if (!tagObj.IpAddress.Contains(ip))
                                {
                                    tagObj.IpAddress.Add(ip);
                                }
                            }
                        }
                    }
                }
            }

            
            foreach (var tag in tags)
            {
                var cleanLabel = new string(tag.TagName.Where(char.IsLetter).ToArray());
                sb.AppendLine($"[{cleanLabel}]");
                foreach (var ip in tag.IpAddress)
                {
                    sb.AppendLine(ip);
                }

                sb.AppendLine("");
            }
            try
            {
                System.IO.File.WriteAllText(fileName, sb.ToString());
                return true;
            }
            catch
            {
                Console.WriteLine("Cannot write ansible_hosts to current directory. Aborted.");
                return false;
            }

        }
    }
}
