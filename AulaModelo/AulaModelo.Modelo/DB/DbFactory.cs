using AulaModelo.Modelo.DB.Model;
using AulaModelo.Modelo.DB.Repository;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AulaModelo.Modelo.DB
{
    public class DbFactory
    {
        //Singletton serve para garantir que a instância sempre estará ativa.

        private static DbFactory _instance = null;

        private ISessionFactory _sessionFactory;

        public PessoaRepository PessoaRepository { get; set; }

        private DbFactory()
        {
            Conexao();

            PessoaRepository = new PessoaRepository(Session);
        }

        public static DbFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DbFactory();
                }

                return _instance;
            }
        }

        //Tratamento de exceção...
        private void Conexao()
        {
            try
            {
                var server = "localhost";
                var port = "3306";
                var dbName = "db_agenda";
                var user = "root";
                var psw = "root";


                var stringConexao = "Persist Security Info=False;" + "server =" + server + ";port=" + port + ";database=" + dbName + ";uid=" + user + ";pwd=" + psw;

                try
                {
                    var mysql = new MySqlConnection(stringConexao);
                    mysql.Open();

                    if(mysql.State == System.Data.ConnectionState.Open)
                    {
                        mysql.Close();
                    }
                }
                catch
                {
                    CriarSchema(server, port, dbName, psw, user);
                }

                ConfigurarNHibernate(stringConexao);
            }catch(Exception ex)
            {
                throw new Exception("Não foi possível conectar", ex);
            }
        }

        private void CriarSchema(string server, string port, string dbName, string psw, string user)
        {
            try
            {
                var stringConexao = "server=" + server +
                    ";user=" + user +
                    ";port=" + port +
                    ";password=" + psw + ";";


                var mySql = new MySqlConnection(stringConexao);
                var cmd = mySql.CreateCommand();
                mySql.Open();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";
                cmd.ExecuteNonQuery();
                mySql.Close();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";

            }
            catch (Exception ex)
            {
                throw new Exception("Não deu para criar o Schema", ex);
            }
        }

        private void ConfigurarNHibernate(String stringConexao)
        {
            try
            {

                var config = new Configuration();

                //Configuração do NH com o MySQL
                config.DataBaseIntegration(i =>
                {
                    // Dialeto do Banco
                    i.Dialect<NHibernate.Dialect.MySQLDialect>();

                    // Conexão string
                    i.ConnectionString = stringConexao;

                    //Drive de conexão com o banco
                    i.Driver<NHibernate.Driver.MySqlDataDriver>();

                    //Provedor de conexão do MySQL
                    i.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();

                    // Gera Log dos SQL Executados no console
                    i.LogSqlInConsole = true;

                    //Descomentar caso queira visualizar o log de SQL Formatado no console
                    i.LogFormattedSql = true;

                    // Cria o Schema do banco de dados sempre que a Configuration for utilizar
                    i.SchemaAction = SchemaAutoAction.Create;

                });

                //Realiza o mapeamento das classes

                var maps = this.Mapeamento();
                config.AddMapping(maps);

                // Verifico se a aplicação é Desktop ou Web

                if(HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }
                else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                this._sessionFactory = config.BuildSessionFactory();


            }catch(Exception ex)
            {
                throw new Exception("Não foi possivel configurar NH", ex);
            }
        }

        private HbmMapping Mapeamento()
        {
            try
            {
                var mapper = new ModelMapper();

                mapper.AddMappings(Assembly.GetAssembly(typeof(PessoaMap)).GetTypes());

                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception("Não mapeou", ex);
            }
        }

        public ISession Session
        {
            get
            {
                try
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                        return _sessionFactory.GetCurrentSession();

                    var session = _sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Commit;

                    CurrentSessionContext.Bind(session);

                    return session;
                }catch (Exception ex)
                {
                    throw new Exception("Não foi possivel criar a Sessão.", ex);
                }
            }
        }
    }
}
