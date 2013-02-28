using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using log4net;
using System.Data;

namespace ImportacaoOpenmrsForm
{
    public class OpenMRSUtils
    {
        private MySqlConnection conexao;
        private MySqlConnection otherConnection;
        private String servidor;
        private String baseDados = "openmrs";
        //private String porta = "3306";
        private String usuario;
        private String senha;

        private static readonly ILog log = LogManager.GetLogger(typeof(OpenMRSUtils));

        public OpenMRSUtils()
        {
        }

        public String  Servidor 
        {
            get { return servidor;}
            set { servidor =value;} 
        }
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public String Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public String BaseDados
        {
            get { return baseDados; }
            set { baseDados = value; }
        }

       
        public OpenMRSUtils(String servidor, String usuario, String senha)
        {
            this.servidor = servidor;
            this.usuario = usuario;
            this.senha = senha;
            //Introduzido por Eurico
            if(conexao ==null )
                conexao = new MySqlConnection("Persist Security Info=False;default command timeout=9999;Connection Timeout=9999;" + "SERVER=" + servidor + "; DATABASE=" + baseDados + "; UID=" + usuario + "; PASSWORD=" + senha);
            if (otherConnection == null)
                otherConnection = new MySqlConnection("Persist Security Info=False;default command timeout=9999;Connection Timeout=9999;" + "SERVER=" + servidor + "; DATABASE=" + baseDados + "; UID=" + usuario + "; PASSWORD=" + senha);

        }

        //Introduzido por Eurico - 16/01/2012 para obter a conexao
        public MySqlConnection getConexao
        {
            get
            {
                if (conexao.State == ConnectionState.Closed)
                    conexao.Open();
                return conexao;
            }
        }

        public MySqlConnection getOtherConnection
        {
            get
            {
                if (otherConnection.State == ConnectionState.Closed)
                    otherConnection.Open();
                return otherConnection;
            }
        }
        


        public MySqlConnection mysqlConexao()
        {
            if (conexao == null)
                conexao = new MySqlConnection("Persist Security Info=False;default command timeout=9999;Connection Timeout=9999;" + "SERVER=" + servidor + "; DATABASE=" + baseDados + "; UID=" + usuario + "; PASSWORD=" + senha);
            if (conexao.State == ConnectionState.Closed)
                conexao.Open();
            return conexao;
        }

        public bool abreConexao()
        {
            
                try
                {
                    mysqlConexao().Open();
                    //MessageBox.Show("Abri Conexao");
                    log.Info("Abrir Conexao na base de dados openmrs");
                    return true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show("Nao Abri Conexao: "+e.Message);
                    return false;
                }
            }

        public bool fechaConexao()
        {
            
                try
                {
                    mysqlConexao().Close();
                    MessageBox.Show("Fechei Conexao");
                    return true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show("Nao Fechei conexao: "+e.Message);
                    return false;
                }
            
        }

        public bool testaConexao()
        {
            try
            {
                //conexao.Open();
                abreConexao();
                MessageBox.Show("Successfully Conneted");
                //conexao.Close();
                fechaConexao();
                log.Info("Abrir Conexao na base de dados openmrs");
                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Connection Failed: "+ e.Message);
                return false;
            }
        }
    }
}
