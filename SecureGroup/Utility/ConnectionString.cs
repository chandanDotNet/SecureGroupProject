namespace SecureGroup.Utility
{
    public class ConnectionString
    {

        private static string cName = "Data Source=LT-721-HO; Initial Catalog=SecureGroup;User ID=sa;Password=Shyam@2023;Encrypt=false;TrustServerCertificate=true";
        public static string CName
        {
            get => cName;
        }
    }
}
