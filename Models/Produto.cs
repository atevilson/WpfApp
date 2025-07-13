using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class Produto : INotifyPropertyChanged
    {
        private string _nome;
        private int _codigo;
        private decimal _valor;

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

        public int Codigo
        {
            get => _codigo;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Código é obrigatório");
                }
                _codigo = value;
                OnPropertyChanged();
            }
        }
        public decimal Valor
        {
            get => _valor;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Valor é obrigatório");
                }
                _valor = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
