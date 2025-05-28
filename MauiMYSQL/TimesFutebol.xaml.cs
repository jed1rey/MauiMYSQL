
using MauiMYSQL.Models;

namespace MauiMYSQL;

public partial class TimesFutebol : ContentPage
{

    Conecta banco = new Conecta();
    Times times = new Times();

    public TimesFutebol()
    {
        InitializeComponent();
        if (banco.Conexao())
        {
            lblStatus.Text = "Banco conectado com Sucesso !";
            if (times.Times_Consulta())
            {
                lstTimesFutebol.ItemsSource = times.listaTimes;
            }
        }
        else
        {
            lblStatus.Text = banco.conexao_status;
        }
    }

    private void btnAdicionar(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtTimeFutebol.Text))
        {
            DisplayAlert("Atenção", "Preencha o nome do time de futebol", "OK");
            return;
        }

        times.Times_Add(txtTimeFutebol.Text, "XXX" , "https://www.example.com/esculdo.png");

        if (times.Times_Consulta()) {
            lstTimesFutebol.ItemsSource = null;
            lstTimesFutebol.ItemsSource = times.listaTimes;
           // lstTimesFutebol.IsRefreshing = true;
        }


    }
}