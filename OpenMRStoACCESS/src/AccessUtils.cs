using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm
{
    public class AccessUtils
    {
        private OleDbConnection conexao;
        private OleDbConnection otherConnection;
        private String baseDados;

        public AccessUtils(String baseDados)
        {
            this.baseDados = baseDados;
            if(conexao ==null)
                conexao = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + baseDados + "'");
            if(otherConnection==null)
                otherConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + baseDados + "'");
        }

        //Introduzido por Eurico - 16/01/2012 para obter a conexao
        public OleDbConnection getConexao { 
            get{
                if (conexao.State == ConnectionState.Closed)
                    conexao.Open();
                return conexao; } 
        }

        public OleDbConnection getOtherConexao
        {
            get
            {
                if (otherConnection.State == ConnectionState.Closed)
                    otherConnection.Open();
                return otherConnection;
            }
        }


        public bool abreConexao()
        {

            try
            {
                conexao.Open();
                MessageBox.Show("Abri Conexao");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Nao Abri Conexao: " + e.Message);
                return false;
            }
        }

        public bool fechaConexao()
        {

            try
            {
                conexao.Close();
                MessageBox.Show("Fechei Conexao");
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Nao Fechei conexao: " + e.Message);
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
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Connection Failed: " + e.Message);
                return false;
            }
        }
    }
}
