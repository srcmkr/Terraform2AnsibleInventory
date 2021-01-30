using System.Collections.Generic;

namespace Terraform2AnsibleInventory.Models
{
    public class Resource
    {
        public string Type { get; set; }
        public string Name {get;set;}
        public List<Instance> Instances {get;set;}
    }
}
