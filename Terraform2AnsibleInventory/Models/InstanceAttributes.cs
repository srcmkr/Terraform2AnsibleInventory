using System.Collections.Generic;

namespace Terraform2AnsibleInventory.Models
{
    public class InstanceAttributes
    {
        public string Ipv4_address { get; set; }
        public Dictionary<string, string> Labels { get; set; }
    }
}
