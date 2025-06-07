using System;
using MySqlConnector;

namespace MauiMYSQL.Models
{
    public class Conecta
    {
        public string conexao_status { get; set; }
        public string StrQuery { get; set; }
        public MySqlDataReader Dr;
        public MySqlCommand Cmd;
        public MySqlConnection Conn;

        public bool Conexao()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "172.200.213.108",
                Port = 3306,
                UserID = "paulaabib",
                Password = "FatecFranca123#",
                Database = "personagens_lotr"
            };

            Conn = new MySqlConnection(builder.ToString());
            try
            {
                Conn.Open();
                conexao_status = "Conexão realizada com sucesso!";
                return true;
            }
            catch (Exception ex)
            {
                conexao_status = $"Erro: {ex.Message}";
                return false;
            }
        }
    }
}
