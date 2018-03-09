using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AulaModelo.Modelo.DB.Repository
{
    public class RepositoryBase<T> where T : class
    {
        protected ISession Session { get; set; }

        public RepositoryBase(ISession session)
        {
            Session = session;
        }

        public void Delete(T entity)
        {
            try
            {
                Session.Clear();

                var transacao = Session.BeginTransaction();

                Session.Delete(entity);

                transacao.Commit();




            }catch (Exception ex)
            {
                throw new Exception("Não deu para excluir.", ex);
            }
        }

        public T SaveOrUpdate(T entity)
        {
            try
            {
                Session.Clear();

                var transacao = Session.BeginTransaction();

                Session.SaveOrUpdate(entity);

                transacao.Commit();

                return entity;


            }
            catch (Exception ex)
            {
                throw new Exception("Não deu para inserir.", ex);
            }
        }

        public IList<T> FindAll()
        {
            try
            {
                return Session.CreateCriteria(typeof(T)).List<T>();
            }
            catch (Exception ex)
            {
                throw new Exception("Não achei todos.", ex);
            }
        }

        public T FindById(Guid id)
        {
            try
            {
                return Session.Get<T>(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Não achei esse cara.", ex);
            }
        }
    }
}
