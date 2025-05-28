using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace MauiMYSQL.Models
{
    public class Alunos : Conecta
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }

        public List<Alunos> lstAlunos = new List<Alunos>();

        public Alunos()
        {

        }
        public Alunos(int id, string nome, string email, string url)
        {
            this.Id = id;
            this.Nome = nome;
            this.Email = email;
            this.Url = url;

        }

        public bool Consulta(int pId)
        {
            bool Ret = false;
            if (!Conexao())
            {
                Ret = false;
                return Ret;
            }

            StrQuery = "SELECT * FROM `Aluno` WHERE id =" + pId.ToString();
            Cmd = new MySqlCommand(StrQuery, Conn);
            Dr = Cmd.ExecuteReader();
            if (Dr.Read())
            {
                Nome = Dr["nome"].ToString();
                Endereco = Dr["Endereco"].ToString();
                Bairro = Dr["Bairro"].ToString();
                Cidade = Dr["Cidade"].ToString();
                Estado = Dr["Estado"].ToString();
                Cep = Dr["Cep"].ToString();
                Telefone = Dr["Telefone"].ToString();
                Email = Dr["Email"].ToString();
                Url = Dr["Url"].ToString();
                Ret = true;
            }
            Dr.Close();
            Conn.Close();

            return Ret;
        }

        public bool ListaClientes(string xBusca = "")
        {
            bool Ret = false;
            if (!Conexao())
            {
                Ret = false;
                return Ret;
            }
            if (xBusca.ToString() == "")
            {
                StrQuery = "SELECT * FROM `Aluno` ORDER BY `nome`";
            }
            else
            {
                StrQuery = "SELECT * FROM `Aluno` WHERE `nome` LIKE '%" + xBusca + "%' ORDER BY `nome`";
            }
            Cmd = new MySqlCommand(StrQuery, Conn);
            Dr = Cmd.ExecuteReader();
            while (Dr.Read())
            {
                //  lstAlunos.Add(Dr["nome"].ToString());
                lstAlunos.Add(
                    new Alunos
                    {
                        Id = int.Parse(Dr["id"].ToString()),
                        Nome = Dr["nome"].ToString(),
                        Email = Dr["email"].ToString(),
                        Url = Dr["url"].ToString()
                    }
                );
            }
            Dr.Close();
            Conn.Close();
            return Ret;
        }

        public bool Excluir()
        {
            bool Ret = false;
            if (!Conexao())
            {
                return Ret;
            }

            StrQuery = "DELETE FROM aluno WHERE id=" + Id.ToString();

            using (Cmd = new MySqlCommand(StrQuery, Conn))
            {
                try
                {
                    int i = Cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        Ret = true;
                        conexao_status = "OK";
                    }

                }
                catch (Exception ex)
                {
                    conexao_status = "Erro:" + ex.Message.ToString();
                    Ret = false;
                }
            }
            Dr.Close();
            Conn.Close();
            return true;
        }

        // salvar dados dos alunos

        public bool Salvar(bool pIncluir = false)
        {
            bool ret = false;
            if (!Conexao())
            {
                ret = false;
                return ret;
            }

            // VERIFICANDO DADOS NULOS
            if (string.IsNullOrEmpty(Url))
            {
                // Url = "https://icon-library.com/images/android-profile-icon/android-profile-icon-2.jpg";
                Url = "icon.png";
            }
            if (pIncluir)
            {
                // inclusao de dados

                StrQuery = "INSERT INTO aluno(id,nome,endereco,bairro,cidade,estado,cep,telefone,email,url) VALUES " +
                           "(@id,@nome,@endereco,@bairro,@cidade,@estado,@cep,@telefone,@email,@url);";

            }
            else
            {
                // alteração de dados
                // o ID devera ser passado da View para a propriedade da Classe antes de atualizar

                StrQuery = "UPDATE aluno SET id=@id,nome=@nome,endereco=@endereco," +
                           "bairro=@bairro,cidade=@cidade,estado=@estado,cep=@cep," +
                           "telefone=@telefone,email=@email,url=@url WHERE id=" + Id.ToString();

            }

            using (Cmd = new MySqlCommand(StrQuery, Conn))
            {
                if (!pIncluir)
                {
                    Cmd.Parameters.AddWithValue("@id", Id); // SE FOR ALTERAÇAO INFORMAR O CODIGO ID CORRETO

                }
                else
                {
                    Cmd.Parameters.AddWithValue("@id", 0); // SE FOR INCLUSAO MANDAR CODIGO 0
                }
                Cmd.Parameters.AddWithValue("@nome", Nome.ToString());
                Cmd.Parameters.AddWithValue("@endereco", Endereco.ToString());
                Cmd.Parameters.AddWithValue("@bairro", Bairro.ToString());
                Cmd.Parameters.AddWithValue("@cidade", Cidade.ToString());
                Cmd.Parameters.AddWithValue("@estado", Estado.ToString());
                Cmd.Parameters.AddWithValue("@cep", Cep.ToString());
                Cmd.Parameters.AddWithValue("@telefone", Telefone.ToString());
                Cmd.Parameters.AddWithValue("@email", Email.ToString());
                Cmd.Parameters.AddWithValue("@url", Url.ToString());
                try
                {
                    int i = Cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        ret = true;
                        conexao_status = "OK";
                    }

                }
                catch (Exception ex)
                {
                    conexao_status = "Erro:" + ex.Message.ToString();
                }


            }
            Dr.Close();
            Conn.Close();
            return ret;
        }





    }

}
