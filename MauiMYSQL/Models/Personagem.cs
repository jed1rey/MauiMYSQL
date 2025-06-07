using System.Collections.Generic;
using MySqlConnector;

namespace MauiMYSQL.Models
{
    public class Personagem : Conecta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Raca { get; set; }
        public string LocalMoradia { get; set; }
        public int Idade { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }

        public List<Personagem> ListaPersonagens { get; set; } = new();

        public bool ConsultaLista(string filtro = "")
        {
            if (!Conexao()) return false;

            ListaPersonagens.Clear();

            StrQuery = string.IsNullOrWhiteSpace(filtro)
                ? "SELECT * FROM personagens ORDER BY nome"
                : "SELECT * FROM personagens WHERE nome LIKE @filtro ORDER BY nome";

            Cmd = new MySqlCommand(StrQuery, Conn);
            if (!string.IsNullOrWhiteSpace(filtro))
                Cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");

            Dr = Cmd.ExecuteReader();
            while (Dr.Read())
            {
                ListaPersonagens.Add(new Personagem
                {
                    Id = int.Parse(Dr["id"].ToString()),
                    Nome = Dr["nome"].ToString(),
                    Raca = Dr["raca"].ToString(),
                    LocalMoradia = Dr["local_moradia"].ToString(),
                    Idade = int.Parse(Dr["idade"].ToString()),
                    Descricao = Dr["descricao"]?.ToString(),
                    Url = Dr["url"]?.ToString()
                });
            }

            Dr.Close();
            Conn.Close();
            return true;
        }

        public bool Salvar(bool incluir)
        {
            if (!Conexao()) return false;

            StrQuery = incluir
                ? "INSERT INTO personagens (nome, raca, local_moradia, idade, descricao, url) VALUES (@nome, @raca, @local_moradia, @idade, @descricao, @url)"
                : "UPDATE personagens SET nome=@nome, raca=@raca, local_moradia=@local_moradia, idade=@idade, descricao=@descricao, url=@url WHERE id=@id";

            Cmd = new MySqlCommand(StrQuery, Conn);
            if (!incluir) Cmd.Parameters.AddWithValue("@id", Id);
            Cmd.Parameters.AddWithValue("@nome", Nome);
            Cmd.Parameters.AddWithValue("@raca", Raca);
            Cmd.Parameters.AddWithValue("@local_moradia", LocalMoradia);
            Cmd.Parameters.AddWithValue("@idade", Idade);
            Cmd.Parameters.AddWithValue("@descricao", Descricao);
            Cmd.Parameters.AddWithValue("@url", Url);

            try
            {
                return Cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }

        public bool Excluir()
        {
            if (!Conexao()) return false;

            StrQuery = "DELETE FROM personagens WHERE id=@id";
            Cmd = new MySqlCommand(StrQuery, Conn);
            Cmd.Parameters.AddWithValue("@id", Id);

            try
            {
                return Cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
