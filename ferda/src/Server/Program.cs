// See https://aka.ms/new-console-template for more information
using System.Data.Common;

Console.WriteLine("Hello, World!");
DbProviderFactories.RegisterFactory("System.Data.OleDb.OleDb", System.Data.OleDb.OleDbFactory.Instance);
var factory = DbProviderFactories.GetFactory("System.Data.OleDb.OleDb");
DbConnection? dbConnection = factory.CreateConnection();
if (dbConnection != null)
{
    //System.Data.IDataReader dr = System.Data.OleDb.OleDbEnumerator.GetRootEnumerator();
    //System.Data.OleDb.OleDbConnectionStringBuilder
    dbConnection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\mk185147\source\repos\Business\PrepareData\bin\Release;Extended Properties=""Text;HDR=Yes""";
    dbConnection.Open();
    Console.WriteLine("Good");
}
else
{
    Console.WriteLine("Didnt get connection");
}