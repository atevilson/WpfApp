using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WpfApp.Models;
using WpfApp.Services.Contrato;

namespace WpfApp.Services
{
    public class JsonProdutoService : IProdutoService
    {
        private readonly string _dataDir;
        private readonly string _file;
        private List<Produto> _produtos;
        private int _nextId;

        public JsonProdutoService(string dataDir)
        {
            _dataDir = dataDir;
            Directory.CreateDirectory(_dataDir);
            _file = Path.Combine(_dataDir, "produtos.json");

            _produtos = File.Exists(_file)
                ? JsonConvert.DeserializeObject<List<Produto>>(File.ReadAllText(_file))
                  ?? new List<Produto>()
                : new List<Produto>();

            _nextId = _produtos.Any() ? _produtos.Max(p => p.Id) + 1 : 1;
        }

        public IEnumerable<Produto> GetAll() => _produtos;

        public void Save(Produto produto)
        {
            if (produto.Id == 0)
            {
                produto.Id = _nextId++;
                _produtos.Add(produto);
            }
            else
            {
                var idx = _produtos.FindIndex(p => p.Id == produto.Id);
                if (idx >= 0) _produtos[idx] = produto;
            }
            Persist();
        }

        public void Delete(int id)
        {
            _produtos.RemoveAll(p => p.Id == id);
            Persist();
        }

        private void Persist()
        {
            File.WriteAllText(_file,
                JsonConvert.SerializeObject(_produtos, Formatting.Indented));
        }
    }
}
