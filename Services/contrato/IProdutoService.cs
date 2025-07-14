using System.Collections.Generic;
using WpfApp.Models;

namespace WpfApp.Services.Contrato
{
    public interface IProdutoService
    {
        IEnumerable<Produto> GetAll();
        void Save(Produto produto);
        void Delete(int id);
    }
}