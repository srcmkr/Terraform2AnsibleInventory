using System.Collections.Generic;

namespace Terraform2AnsibleInventory.Models
{
    public class Tag
    {
        public string TagName { get; set; }
        public List<string> IpAddress { get; set; }
    }
}
