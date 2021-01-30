namespace Terraform2AnsibleInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Terraform to Ansible Inventory by label converter");
            System.Console.WriteLine($"https://github.com/srcmkr/Terraform2AnsibleInventory\r\n");
            var t2ai = new T2AI();
            t2ai.PreflightCheck(args);
            t2ai.LoadJson();
            t2ai.SaveToFile();
            System.Console.WriteLine($"tfstate converted successfully and saved to 'ansible_hosts'\r\n");
        }
    }
}
