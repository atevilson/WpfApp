using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Models;
using WpfApp.Services.contrato;

namespace WpfApp.ViewModels
{
    class CadastroDePessoaViewModel : INotifyPropertyChanged
    {
        private readonly IPessoaService _pessoaService;
        private string _nomeFiltro;
        private string _cpfFiltro;
        private Pessoa _pessoaSelecionada;

        public ObservableCollection<Pessoa> Pessoas { get; }
        public ObservableCollection<Pedido> PedidosDaPessoa { get; }

        public string NomeFiltro
        {
            get => _nomeFiltro;
            set { _nomeFiltro = value; OnPropertyChanged(); }
        }

        public string CpfFiltro
        {
            get => _cpfFiltro;
            set { _cpfFiltro = value; OnPropertyChanged(); }
        }

        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                _pessoaSelecionada = value;
                OnPropertyChanged();

                (ExcluirPessoaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (IncluirPedidoCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (SalvarPessoaCommand as RelayCommand)?.RaiseCanExecuteChanged();

                CarregarPedidos();
            }
        }

        public ICommand FiltrarCommand { get; }
        public ICommand LimparFiltroCommand { get; }
        public ICommand IncluirPessoaCommand { get; }
        public ICommand SalvarPessoaCommand { get; }
        public ICommand ExcluirPessoaCommand { get; }
        public ICommand IncluirPedidoCommand { get; }
        public ICommand MarcarPagoCommand { get; }
        public ICommand MarcarEnviadoCommand { get; }
        public ICommand MarcarRecebidoCommand { get; }

        public CadastroDePessoaViewModel(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;

            Pessoas = new ObservableCollection<Pessoa>(_pessoaService.GetAll());
            PedidosDaPessoa = new ObservableCollection<Pedido>();

            FiltrarCommand = new RelayCommand(Filtrar);
            LimparFiltroCommand = new RelayCommand(LimparFiltro);
            IncluirPessoaCommand = new RelayCommand(IncluirPessoa);
            SalvarPessoaCommand = new RelayCommand(SalvarPessoa, () => PessoaSelecionada != null);
            ExcluirPessoaCommand = new RelayCommand(ExcluirPessoa, () => PessoaSelecionada != null);
            IncluirPedidoCommand = new RelayCommand(IncluirPedido, () => PessoaSelecionada != null);

            MarcarPagoCommand = new RelayCommand<Pedido>(p => AtualizarStatus(p, Status.Pago));
            MarcarEnviadoCommand = new RelayCommand<Pedido>(p => AtualizarStatus(p, Status.Enviado));
            MarcarRecebidoCommand = new RelayCommand<Pedido>(p => AtualizarStatus(p, Status.Recebido));
        }

        private void Filtrar()
        {
            var lista = _pessoaService.GetAll();

            if (!string.IsNullOrEmpty(NomeFiltro))
            {
                lista = lista.Where(p =>
                    !string.IsNullOrEmpty(p.Nome) &&
                    p.Nome.IndexOf(NomeFiltro, StringComparison.OrdinalIgnoreCase) >= 0
                ); 
            }

            if (!string.IsNullOrEmpty(CpfFiltro))
            {
                lista = lista.Where(p =>
                    !string.IsNullOrWhiteSpace(p.Cpf) &&
                    p.Cpf.Contains(CpfFiltro)
                );
            }

            Pessoas.Clear();
            foreach (var p in lista)
                Pessoas.Add(p);
        }

        private void LimparFiltro()
        {
            NomeFiltro = string.Empty;
            CpfFiltro = string.Empty;
            Filtrar();
        }

        private void IncluirPessoa()
        {
            var nova = new Pessoa();
            Pessoas.Add(nova);
            PessoaSelecionada = nova;
        }

        private void SalvarPessoa()
        {
            try
            {

                if (string.IsNullOrEmpty(PessoaSelecionada.Nome))
                    throw new ArgumentException("Nome é obrigatório.");

                if (string.IsNullOrEmpty(PessoaSelecionada.Cpf))
                    throw new ArgumentException("CPF é obrigatório.");

                _pessoaService.Save(PessoaSelecionada);

                MessageBox.Show("Salvo com sucesso!", "Sucesso",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Filtrar();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Atenção",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao salvar: " + ex.Message, "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExcluirPessoa()
        {
            var result = MessageBox.Show("Deseja remover "+ PessoaSelecionada.Nome + "?", "Sucesso", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (((byte)result).Equals(6))
            {
                _pessoaService.Delete(PessoaSelecionada.Id);
                Pessoas.Remove(PessoaSelecionada);
                PessoaSelecionada = null;
            }
        }

        private void CarregarPedidos()
        {
            PedidosDaPessoa.Clear();
            if (PessoaSelecionada == null) return;

            foreach (var pedido in _pessoaService.GetPedidos(PessoaSelecionada.Id))
                PedidosDaPessoa.Add(pedido);
        }

        private void IncluirPedido()
        {
            // preciso intetgrar ao criar a tela de pedidos (fazer nav. de tela de pedido passando a var PessoaSelecionada
        }

        private void AtualizarStatus(Pedido pedido, Status novoStatus)
        {
            if (pedido == null) return;
            pedido.Status = novoStatus;
            _pessoaService.UpdatePedido(pedido);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
