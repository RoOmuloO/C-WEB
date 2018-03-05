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

            return View(Pessoa.Pessoas);
        }

        public ActionResult InserirPessoa()
        {
            return View("EditarPessoa", new Pessoa());
        }

        public ActionResult GravarPessoa(Pessoa pessoa)
        {
            if(pessoa.Id.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                pessoa.Id = Guid.NewGuid();
                Pessoa.Pessoas.Add(pessoa);
            }
            else
            {
                var p = Pessoa.Pessoas.FirstOrDefault(f => f.Id == pessoa.Id);
                p.Nome = pessoa.Nome;
                p.DtNascimento = pessoa.DtNascimento;
                p.Email = pessoa.Email;
                p.Telefone = pessoa.Telefone;
            }

            
            return RedirectToAction("Index");
        }

        public ActionResult ApagarPessoa(Guid id)
        {
            Pessoa p = null;

            foreach (var pessoa in Pessoa.Pessoas)
            {
                if(pessoa.Id == id)
                {
                    p = pessoa;
                    break;
                }
            }

            if(p != null)
            {
                Pessoa.Pessoas.Remove(p);
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditarPessoa(Guid id)
        {
            var p = Pessoa.Pessoas.FirstOrDefault(f => f.Id == id);
            if(p != null)
            {
                return View(p);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Buscar(String edtBusca)
        {

            var pessoaAux = new List<Pessoa>();

            if (String.IsNullOrEmpty(edtBusca))
            {
                return View("Index", Pessoa.Pessoas);
            }

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
            pessoaAux = Pessoa.Pessoas.Where(x => x.Nome.Contains(edtBusca)).ToList();
            return View("Index",pessoaAux); 


        }
    }
}