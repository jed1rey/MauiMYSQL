using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Java.Util;
using MauiMYSQL.Models;
using MySqlConnector;

namespace MauiMYSQL.Models
{
    public class Times : Conecta
    {

        public int id { get; set; }
        public string nome { get; set; }
        public string sigla { get; set; }
        public string escudo_url { get; set; }

        public List<Times> listaTimes = new List<Times>();

        public Times() { }

        public bool Times_Add(string nome, string sigla, string escudo_url)
        {

            if (!Conexao())
            {
                return false;
            }


            StrQuery = "INSERT INTO times (nome, sigla, escudo_url) VALUES (@nome, @sigla, @esculdo_url)";
            Cmd = new MySqlCommand(StrQuery, Conn);
            Cmd.Parameters.AddWithValue("@nome", nome);
            Cmd.Parameters.AddWithValue("@sigla", sigla);
            Cmd.Parameters.AddWithValue("@esculdo_url", escudo_url);
            try
            {
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // retornar os times cadastrados no banco de dados em uma lista

        public bool Times_Consulta()
        {
            if (!Conexao())
            {
                return false;
            }

            StrQuery = "SELECT * FROM times order by nome";
            MySqlCommand cmd = new MySqlCommand(StrQuery, Conn);
            cmd.CommandType = System.Data.CommandType.Text;
            Dr = cmd.ExecuteReader();
            listaTimes.Clear();
            while (Dr.Read())
            {
                    listaTimes.Add(new Times { id = int.Parse(Dr["id"].ToString()), 
                                               nome = Dr["nome"].ToString(), 
                                               sigla = Dr["sigla"].ToString(), 
                                               escudo_url = Dr["escudo_url"].ToString()
                                             }
                                  );
            }

            return true;

        }










    }
}
