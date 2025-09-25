using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetodeExtensao
{
    public class Endereco
    {
        private string rua;
        private int numero;
        private string bairro;
        private string cidade;
        private string cep;
        private string complemento;

        public Endereco(string rua, int numero, string bairro, string cidade, string cep, string complemento)
        {
            this.rua = rua;
            this.numero = numero;
            this.bairro = bairro;
            this.cidade = cidade;
            this.cep = cep;
            this.complemento = complemento;
        }

        public string GetRua() { return rua; }
        public int GetNumero() { return numero; }
        public string GetBairro() { return bairro; }
        public string GetCidade() { return cidade; }
        public string GetCEP() { return cep; }
        public string GetComplemento() { return complemento; }
        public void SetRua(string rua) { this.rua = rua; }
        public void SetNumero(int numero) { this.numero = numero; }
        public void SetBairro(string bairro) { this.bairro = bairro; }
        public void SetCidade(string cidade) { this.cidade = cidade; }
        public void SetCEP(string cep) { this.cep = cep; }
        public void SetComplemento(string complemento) { this.complemento = complemento; }

    }
}
