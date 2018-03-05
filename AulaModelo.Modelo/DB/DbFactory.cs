using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AulaModelo.Modelo.DB
{
    public class DbFactory
    {
        //Singletton serve para garantir que a instância sempre estará ativa.

        private static DbFactory _instance = null;

        private DbFactory()
        {
            Conexao();
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
                var stringConexao = "";


                ConfigurarNHibernate(stringConexao);
            }catch(Exception ex)
            {
                throw new Exception("Não foi possível conectar", ex);
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
                    i.Dialect<NHibernate.Dialect.MsSql7Dialect>();

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
                    i.SchemaAction = SchemaAutoAction.Update;

                });


            }catch(Exception ex)
            {
                throw new Exception("Não foi possivel configurar NH", ex);
            }
        }
    }
}
