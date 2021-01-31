# Terraform2AnsibleInventory
Transforms terraform.tfstate to ini-styled ansible hosts-file

**Available parameters**  
-i, --in Required. Input file to be processed.  
-o, --out Required. Output file to be processed.  
-v, --verbose (Default: false) Verbose all messages to standard output.  
--help Display this help screen.  
--version Display version information.  

**Usage**  
t2ai --in /path/to/terraform.tfstate --out /path/to/ansible_hosts [--verbose]  
