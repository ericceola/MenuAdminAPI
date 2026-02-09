using System;
using System.IO;
using System.Data.SqlClient;

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
                string scriptPath = "../../../../seed-correct.sql";
                if (!File.Exists(scriptPath))
                {
                    // Tentar caminho alternativo
                    scriptPath = "/home/ubuntu/MenuAdminAPI_Solution/seed-correct.sql";
                    if (!File.Exists(scriptPath))
                    {
                        // Tentar seed-data-corrected.sql
                        scriptPath = "/home/ubuntu/MenuAdminAPI_Solution/seed-data-corrected.sql";
                        if (!File.Exists(scriptPath))
                        {
                            scriptPath = "/home/ubuntu/MenuAdminAPI_Solution/seed-data.sql";
                        }
                        if (!File.Exists(scriptPath))
                        {
                            Console.WriteLine($"❌ Arquivo não encontrado!");
                            return;
                        }
                    }
                }

                string sqlScript = File.ReadAllText(scriptPath);
                
                // Executar script inteiro
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandTimeout = 120;
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine($"✅ Banco de dados populado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Erro ao executar script: {ex.Message.Substring(0, Math.Min(200, ex.Message.Length))}");
                }

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
