using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AulaModelo.Modelo.DB.Model
{
    public class Pessoa
    {
        public static List<Pessoa> Pessoas = new List<Pessoa>();

        public Guid Id { get; set; }
        public String Nome { get; set; }
        public DateTime DtNascimento { get; set; }
        public String Email { get; set; }
        public String Telefone { get; set; }


    }
}
