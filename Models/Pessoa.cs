using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class Pessoa : INotifyPropertyChanged
    {
        private string _nome;
        private string _cpf;

        public int Id { get; set; }
        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged();
            }
        }
        public string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                OnPropertyChanged();

            }
        }
        public string Endereco { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
