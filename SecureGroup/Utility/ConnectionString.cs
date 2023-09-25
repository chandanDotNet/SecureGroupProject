namespace SecureGroup.Utility
{
    public class ConnectionString
    {

        //private static string cName = "Data Source=EC2AMAZ-F2LBLTP\\SQLEXPRESS; Initial Catalog=SecureGroupTesting;User ID=sa;Password=SG@2023;Encrypt=false;TrustServerCertificate=true";
       //private static string cName = "Data Source=CRM\\SQLEXPRESS; Initial Catalog=SecureGroup;User ID=sa;Password=SG@2023;Encrypt=false;TrustServerCertificate=true";
       private static string cName = "Data Source=10.200.109.160; Initial Catalog=SecureGroup;User ID=sa;Password=Shyam@2023;Encrypt=false;TrustServerCertificate=true";
       // private static string cName = "Data Source=REDMINE-SRV; Initial Catalog=SecureGroup;User ID=sa;Password=Shyam@2023;Encrypt=false;TrustServerCertificate=true";
        public static string CName
        {
            get => cName;
        }
    }
}
