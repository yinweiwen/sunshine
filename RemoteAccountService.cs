using System;
using System.Configuration;
using System.Data;
using Npgsql;

public class RemoteAccountService
{
    private static string connectionString = ConfigurationManager.AppSettings["db"];

    public static DataTable GetRemoteAccounts(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            var query = @"SELECT * FROM remote_accounts 
                          WHERE name ILIKE @searchTerm 
                          OR remark ILIKE @searchTerm
                          ORDER BY id
                          LIMIT @pageSize OFFSET @offset";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("searchTerm", "%" + searchTerm + "%");
                cmd.Parameters.AddWithValue("pageSize", pageSize);
                cmd.Parameters.AddWithValue("offset", (page - 1) * pageSize);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }

    public static void DeleteRemoteAccount(string id)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand("DELETE FROM remote_accounts WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void AddRemoteAccount(string name, string remark, string code, string password, string type)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand("INSERT INTO remote_accounts (name, remark, code, password, type) VALUES (@name, @remark, @code, @password, @type)", conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("remark", remark);
            cmd.Parameters.AddWithValue("code", code);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("type", type);
            cmd.ExecuteNonQuery();
        }
    }

    // Add methods for Insert, Update, Delete operations here...
}