using System;
using System.IO;
using System.Data.SqlClient;
using System.Linq;

class Program
{
    static void Main()
    {
        string connectionString = "Server=tcp:cafe-senador.database.windows.net,1433;" +
                                 "Initial Catalog=MenuDB;" +
                                 "Persist Security Info=False;" +
                                 "User ID=CoffeeAdmin;" +
                                 "Password=CoffeeCeola@123;" +
                                 "MultipleActiveResultSets=False;" +
                                 "Encrypt=True;" +
                                 "TrustServerCertificate=False;" +
                                 "Connection Timeout=30;";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("✅ Conectado ao Azure SQL Database com sucesso!");

                // Ler script SQL
                string scriptPath = "seed-data.sql";
                if (!File.Exists(scriptPath))
                {
                    Console.WriteLine($"❌ Arquivo {scriptPath} não encontrado!");
                    return;
                }

                string sqlScript = File.ReadAllText(scriptPath);
                
                // Dividir por GO
                var statements = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                int executedCount = 0;
                foreach (var statement in statements)
                {
                    string cleanStatement = statement.Trim();
                    if (string.IsNullOrEmpty(cleanStatement) || cleanStatement.StartsWith("--"))
                        continue;

                    try
                    {
                        using (SqlCommand command = new SqlCommand(cleanStatement, connection))
                        {
                            command.CommandTimeout = 60;
                            command.ExecuteNonQuery();
                            executedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Erro ao executar statement: {ex.Message.Substring(0, Math.Min(100, ex.Message.Length))}");
                    }
                }

                Console.WriteLine($"✅ Banco de dados populado com sucesso! ({executedCount} statements executados)");

                // Verificar dados
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Estabelecimentos", connection))
                {
                    int estCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"\n📊 Resumo dos dados inseridos:");
                    Console.WriteLine($"   Estabelecimentos: {estCount}");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Produtos", connection))
                {
                    int prodCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"   Produtos: {prodCount}");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Clientes", connection))
                {
                    int cliCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"   Clientes: {cliCount}");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pedidos", connection))
                {
                    int pedCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"   Pedidos: {pedCount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao conectar: {ex.Message}");
        }
    }
}
