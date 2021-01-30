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

        public void PreflightCheck(string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                PreflightCheck(new string[] { tfstateFile });
                return;
            }

            if (System.IO.File.Exists(args[0]))
            {
                tfstateFile = args[0];
            }
            else
            {
                throw new Exception("Please set full path incl. terraform.tfstate as parameter if not in current work folder.");
            }
        }

        public void LoadJson()
        {
            try
            {
                RootObject = JsonConvert.DeserializeObject<RootObject>(System.IO.File.ReadAllText(tfstateFile));
            }
            catch (Exception ex)
            {
                throw new Exception("terraform.tfstate is invalid or could not be read.", ex);
            }
        }

        public void SaveToFile()
        {
            var tags = new List<Tag>();

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

            var sb = new StringBuilder();
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
                System.IO.File.WriteAllText("ansible_hosts", sb.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot write ansible_hosts to current directory. Aborted.", ex);
            }

        }
    }
}
