using System.Collections.Generic;

namespace Terraform2AnsibleInventory.Models
{
    public class RootObject
    {
        public List<Resource> Resources { get; set; }
    }
}
