using MauiMYSQL.Models;

namespace MauiMYSQL;

public partial class PersonagensPage : ContentPage
{
    Personagem personagem = new();

    public PersonagensPage()
    {
        InitializeComponent();

        if (personagem.Conexao())
        {
            lblStatus.Text = "Conectado ao banco de dados!";
            AtualizarLista();
        }
        else
        {
            lblStatus.Text = personagem.conexao_status;
        }
    }

    private void AtualizarLista()
    {
        if (personagem.ConsultaLista())
        {
            lstPersonagens.ItemsSource = null;
            lstPersonagens.ItemsSource = personagem.ListaPersonagens;
        }
    }

    private void btnAdicionarPersonagem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNomePersonagem.Text) ||
            string.IsNullOrWhiteSpace(txtRaca.Text) ||
            string.IsNullOrWhiteSpace(txtIdade.Text))
        {
            DisplayAlert("Erro", "Preencha os campos obrigatórios!", "OK");
            return;
        }

        personagem.Nome = txtNomePersonagem.Text;
        personagem.Raca = txtRaca.Text;
        personagem.LocalMoradia = txtMoradia.Text;
        personagem.Idade = int.TryParse(txtIdade.Text, out int idade) ? idade : 0;
        personagem.Descricao = txtDescricaoPersonagem.Text;
        personagem.Url = txtUrl.Text;

        if (personagem.Salvar(true))
        {
            DisplayAlert("Sucesso", "Personagem salvo!", "OK");
            AtualizarLista();

            txtNomePersonagem.Text = "";
            txtRaca.Text = "";
            txtMoradia.Text = "";
            txtIdade.Text = "";
            txtDescricaoPersonagem.Text = "";
            txtUrl.Text = "";
        }
        else
        {
            DisplayAlert("Erro", "Não foi possível salvar", "OK");
        }
    }
}
