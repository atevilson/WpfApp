using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class Pessoa : INotifyPropertyChanged
    {
        private string _nome;
        private string _cpf;

        public int Id { get; }
        public string Nome
        {
            get => _nome;
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Nome é obrigatório");
                }
                _nome = value;
                OnPropertyChanged();
            }
        }
        public string Cpf
        {
            get => _cpf;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("CPF é obrigatório");
                }
                _cpf = value;
                OnPropertyChanged();

            }
        }
        public string Endereco;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
