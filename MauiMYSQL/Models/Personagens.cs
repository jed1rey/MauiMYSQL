using System;
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

        public List<Personagem> ListaPersonagens = new List<Personagem>();

        public Personagem() { }

        // Consulta personagem pelo Id
        public bool Consulta(int pId)
        {
            bool Ret = false;
            if (!Conexao())
                return false;

            StrQuery = "SELECT * FROM personagens WHERE id = " + pId;
            Cmd = new MySqlCommand(StrQuery, Conn);
            Dr = Cmd.ExecuteReader();

            if (Dr.Read())
            {
                Id = int.Parse(Dr["id"].ToString());
                Nome = Dr["nome"].ToString();
                Raca = Dr["raca"].ToString();
                LocalMoradia = Dr["local_moradia"].ToString();
                Idade = int.Parse(Dr["idade"].ToString());
                Ret = true;
            }
            Dr.Close();
            Conn.Close();
            return Ret;
        }

        // Listar personagens com filtro opcional por nome
        public bool ListaPersonagensFiltro(string busca = "")
        {
            if (!Conexao())
                return false;

            ListaPersonagens.Clear();

            if (string.IsNullOrWhiteSpace(busca))
                StrQuery = "SELECT * FROM personagens ORDER BY nome";
            else
                StrQuery = "SELECT * FROM personagens WHERE nome LIKE @busca ORDER BY nome";

            Cmd = new MySqlCommand(StrQuery, Conn);
            if (!string.IsNullOrWhiteSpace(busca))
                Cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");

            Dr = Cmd.ExecuteReader();

            while (Dr.Read())
            {
                ListaPersonagens.Add(new Personagem
                {
                    Id = int.Parse(Dr["id"].ToString()),
                    Nome = Dr["nome"].ToString(),
                    Raca = Dr["raca"].ToString(),
                    LocalMoradia = Dr["local_moradia"].ToString(),
                    Idade = int.Parse(Dr["idade"].ToString())
                });
            }
            Dr.Close();
            Conn.Close();

            return true;
        }

        // Inserir ou atualizar personagem
        public bool Salvar(bool incluir = false)
        {
            if (!Conexao())
                return false;

            if (incluir)
            {
                StrQuery = "INSERT INTO personagens (nome, raca, local_moradia, idade) VALUES (@nome, @raca, @local_moradia, @idade)";
            }
            else
            {
                StrQuery = "UPDATE personagens SET nome = @nome, raca = @raca, local_moradia = @local_moradia, idade = @idade WHERE id = @id";
            }

            Cmd = new MySqlCommand(StrQuery, Conn);

            if (!incluir)
                Cmd.Parameters.AddWithValue("@id", Id);

            Cmd.Parameters.AddWithValue("@nome", Nome);
            Cmd.Parameters.AddWithValue("@raca", Raca);
            Cmd.Parameters.AddWithValue("@local_moradia", LocalMoradia);
            Cmd.Parameters.AddWithValue("@idade", Idade);

            try
            {
                int linhas = Cmd.ExecuteNonQuery();
                return linhas > 0;
            }
            catch (Exception ex)
            {
                conexao_status = "Erro: " + ex.Message;
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }

        // Excluir personagem
        public bool Excluir()
        {
            if (!Conexao())
                return false;

            StrQuery = "DELETE FROM personagens WHERE id = @id";
            Cmd = new MySqlCommand(StrQuery, Conn);
            Cmd.Parameters.AddWithValue("@id", Id);

            try
            {
                int linhas = Cmd.ExecuteNonQuery();
                return linhas > 0;
            }
            catch (Exception ex)
            {
                conexao_status = "Erro: " + ex.Message;
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
