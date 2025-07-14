using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dataDir = Path.Combine(documents, "WpfApp", "Data");

            var pessoaService = new JsonPessoaService(dataDir);

            var pessoaVm = new CadastroDePessoaViewModel(pessoaService);

            var pessoaView = new CadastroDePessoas
            {
                DataContext = pessoaVm
            };

            var window = new Window
            {
                Title = "Cadastro de Pessoas",
                Content = pessoaView,
                Width = 800,
                Height = 600
            };
            window.Show();
        }
    }
}
