using AulaModelo.Modelo.DB;
using AulaModelo.Modelo.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AulaModelo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Pessoa.Pessoas.Add(new Pessoa() {

            //    Id = Guid.NewGuid(),
            //    Nome = "Romulo",
            //    DtNascimento = DateTime.Now

            //});

            //Pessoa.Pessoas.Add(new Pessoa()
            //{

            //Id = Guid.NewGuid(),
            //    Nome = "Ramon",
            //    DtNascimento = DateTime.Now

            //});

            //Pessoa.Pessoas.Add(new Pessoa()
            //{

            //Id = Guid.NewGuid(),
            //    Nome = "Luiz",
            //    DtNascimento = DateTime.Now

            //});

            var pessoas = DbFactory.Instance.PessoaRepository.FindAll();

            return View(pessoas);
        }

        public ActionResult InserirPessoa()
        {
            return View("EditarPessoa", new Pessoa());
        }

        public ActionResult GravarPessoa(Pessoa pessoa)
        {
            DbFactory.Instance.PessoaRepository.SaveOrUpdate(pessoa);
            
            return RedirectToAction("Index");
        }

        public ActionResult ApagarPessoa(Guid id)
        {

            var pessoa = DbFactory.Instance.PessoaRepository.FindById(id);

            if (pessoa != null)
            {
                DbFactory.Instance.PessoaRepository.Delete(pessoa);
            }
            

            return RedirectToAction("Index");
        }

        public ActionResult EditarPessoa(Guid id)
        {
            var pessoa = DbFactory.Instance.PessoaRepository.FindById(id);

            if (pessoa != null)
            {
                return View(pessoa);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Buscar(String edtBusca)
        {

            
            if (String.IsNullOrEmpty(edtBusca))
            {
                return RedirectToAction("Index");
            }
            var pessoas = DbFactory.Instance.PessoaRepository.GetAllByName(edtBusca);

            return View("Index", pessoas);

            //1- filtro com FOR

            //for(var i = 0; i < Pessoa.Pessoas.Count; i++)
            //{
            //    if(Pessoa.Pessoas[i].Nome == edtBusca.Trim()) // Trim serve para Retirar espaços em branco
            //    {
            //        pessoaAux.Add(Pessoa.Pessoas[i]);
            //    }
            //}

            //2- Filtro com FOREACH
            //foreach(var pessoa in Pessoa.Pessoas)
            //{
            //    if (pessoa.Nome == edtBusca.Trim())
            //    {
            //        pessoaAux.Add(pessoa);
            //    }
            //}

            //3 - filtro com LINQ
            //pessoaAux = (from p in Pessoa.Pessoas
            //             where p.Nome == edtBusca.Trim()
            //             select p).ToList();



            //4 - filtro com LAMBDA
            //pessoaAux = Pessoa.Pessoas.Where(x => x.Nome.Contains(edtBusca)).ToList();



        }
    }
}