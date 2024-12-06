using Microsoft.Data.SqlClient;

namespace MiniPOS.RestApi.Shared;

public class AppSettings
{
    private static readonly SqlConnectionStringBuilder _SqlConnectionStringBuillder = new()
    {
        DataSource = ".",
        InitialCatalog = "MyMiniPOSDB",
        UserID = "sa",
        Password = "Aa145156167!",
        TrustServerCertificate = true
    };

    public static readonly string ConnectionString = _SqlConnectionStringBuillder.ConnectionString;
}
