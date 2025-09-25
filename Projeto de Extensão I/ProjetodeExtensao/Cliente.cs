using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetodeExtensao
{
    public class Cliente
    {
        private string nome;
        private string cpf;
        private Endereco endereco;
        private string email;
        private string sexo;

        public Cliente(string nome, string cpf, Endereco endereco, string email, string sexo)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.endereco = endereco;
            this.email = email;
            this.sexo = sexo;
        }

        public string GetNome() { return nome; }
        public string GetCPF() { return cpf; }
        public Endereco GetEndereco() { return endereco; }
        public string GetEmail() { return email; }
        public string GetSexo() { return sexo; }

        public void SetNome(string nome) { this.nome = nome; }
        public void SetCPF(string cpf) { this.cpf = cpf; }
        public void SetEndereco(Endereco endereco) { this.endereco = endereco; }
        public void SetEmail(string email) { this.email = email; }
        public void SetSexo(string sexo) { this.sexo = sexo; }
    }

}
