using AulaModelo.Modelo.DB.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AulaModelo.Modelo.DB.Repository
{
    public class PessoaRepository : RepositoryBase<Pessoa>
    {
        public PessoaRepository(ISession session) : base(session) { }

        public IList<Pessoa> GetAllByName(String nome)
        {
            try
            {
                return Session.Query<Pessoa>()
                    .Where(w => w.Nome.ToLower().Contains(nome.Trim().ToLower()))
                    .ToList();

            }catch (Exception ex)
            {
                throw new Exception("Achei as pessoas pelo nome não", ex);
            }
        }
    }
}
