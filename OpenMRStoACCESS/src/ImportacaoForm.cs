﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net.Config;
using log4net;
using ImportacaoOpenmrsForm.TPaciente;
using ImportacaoOpenmrsForm.TTarv;
using ImportacaoOpenmrsForm.TSeguimento;
using ImportacaoOpenmrsForm.TResultadoLab;
using ImportacaoOpenmrsForm.TAconselhamento;
using ImportacaoOpenmrsForm.TAntecedentesClinicos;
using ImportacaoOpenmrsForm.TObservacaoPaciente;
using ImportacaoOpenmrsForm.TContacto;
using ImportacaoOpenmrsForm.TAdulto;
using ImportacaoOpenmrsForm.TCrianca;
using ImportacaoOpenmrsForm.TMae;
using ImportacaoOpenmrsForm.TPai;
using ImportacaoOpenmrsForm.Utils;
using ImportacaoOpenmrsForm.TExposicaoTarv;
using ImportacaoOpenmrsForm.TTratamentoTB;
using ImportacaoOpenmrsForm.TBuscaActiva;
using ImportacaoOpenmrsForm.ExportGaac;
using Excel=Microsoft.Office.Interop.Excel;
using System.Reflection;
namespace ImportacaoOpenmrsForm
{
    public partial class ImportacaoForm : Form
    {
        //private ILog log;

        public ImportacaoForm()
        {
            InitializeComponent();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            //log = log4net.LogManager.GetLogger("LogInFile");
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            txtDataSource.Text = openFileDialog1.FileName;

        }

        private void btTestOpenMRS_Click(object sender, EventArgs e)
        {
            OpenMRSUtils openmrs = new OpenMRSUtils(txtHost.Text,txtUsername.Text,txtPassword.Text);
            openmrs.testaConexao();
        }

        private void btImport_Click(object sender, EventArgs e)
        {

            //Validacoes de campos
            if (String.IsNullOrEmpty(txtHost.Text))
            {
                MessageBox.Show("The field Server Host must not be empty...");
                txtHost.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtPort.Text))
            {
                MessageBox.Show("The field Port must not be empty...");
                txtPort.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("The field User Name must not be empty...");
                txtUsername.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("The field Password must not be empty...");
                txtPassword.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtDataSource.Text))
            {
                MessageBox.Show("The field Data Source must not be empty...");
                txtDataSource.Focus();
                return;
            }

            lblSMS.Text = "Preparando dados iniciais...";
            lblSMS.Refresh();

            String dataInicialMySQL = dataInicial.Value.Year + "/" + dataInicial.Value.Month + "/" + dataInicial.Value.Day;
            String dataFinalMySQL = dataFinal.Value.Year + "/" + dataFinal.Value.Month + "/" + dataFinal.Value.Day;

            //Utils para conexao MySQL - OpenMRS
            OpenMRSUtils mysqlUtil = new OpenMRSUtils(txtHost.Text, txtUsername.Text, txtPassword.Text);
            
            //Utils para conexao MS Access - Sistema HDD
            AccessUtils accessUtil = new AccessUtils(txtDataSource.Text);

            //Tem algumas funcoes auxiliares de insercao de dados
            InsertUtils insertUtils = new InsertUtils();

            //Inserir na tabela t_hdd com base no distrito seleccionado
            insertUtils.insertHDD(accessUtil.getConexao, cboDistrito);

           // //Levar o codigo da unidade sanitaria seleccionada
            //String codHDD = insertUtils.getHDDID(accessUtil.getConexao);

           // //Levar o codigo do distrito como primeiros 4 digitos do codigo da unidade sanitaria
           // String codDistrito = codHDD.Substring(0, 4);
            //Levar o codigo do distrito como primeiros 4 digitos do codigo da unidade sanitaria
            //String codDistrito = codHDD.Substring(0, 4);

            //Zerar a tabela t_paciente garantir que e nova insercao
            insertUtils.emptyTPaciente(accessUtil.getConexao);

            barraProgresso.Value = 5;

            lblSMS.Text = "A executar a chamada do Stored Procedure no OpenMRS...";
            lblSMS.Refresh();

            //Chamar a procedure MySQL para encher a tabela t_paciente no OpenMRS
            String codUS = cboDistrito.Text.Substring(0, 6);
            insertUtils.callFillTPacienteTable(mysqlUtil.getConexao, dataFinalMySQL, codUS, codUS.Substring(0,4));

            barraProgresso.Value = 10;

            lblSMS.Text = "Enchendo a tabela t_paciente no ACCESS...";
            lblSMS.Refresh();

            //Encher a tabela de t_bairro
            insertUtils.insertBairro(accessUtil.getConexao, mysqlUtil.getConexao);

            //Encher a tabela de t_funcionario
            insertUtils.insertFuncionario(accessUtil.getConexao, mysqlUtil.getConexao);

            // //Encher a tabela t_paciente MS Access
            ExportTPaciente.ExportData(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 15;
            barraProgresso.Refresh();

            //Export t_adulto 
            lblSMS.Text = "Enchendo a tabela t_adulto...";
            lblSMS.Refresh();

            ExportTAdulto eAdulto = new ExportTAdulto();
            eAdulto.exportDataAdulto(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 20;
            barraProgresso.Refresh();

            lblSMS.Text = "Enchendo a tabela t_pai, t_mae e t_crianca...";
            lblSMS.Refresh();

            ExportCrianca ec = new ExportCrianca();

            ExportTMae em = new ExportTMae();

            ExportTPai ep = new ExportTPai();

            em.exportDataTMae(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);
            ep.exportDataTPai(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);
            ec.exportDataCrianca(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 25;
            barraProgresso.Refresh();


            lblSMS.Text = "Export a tabela t_seguimento (Leva muito tempo) ...";
            lblSMS.Refresh();

            ExportTSeguimento eSeguimento = new ExportTSeguimento();

            eSeguimento.exportDataTSeguimento(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date, mysqlUtil.getOtherConnection);


            barraProgresso.Value = 55;
            barraProgresso.Refresh();



            lblSMS.Text = "Export a tabela t_tarv,t_histestadopaciente ...";
            lblSMS.Refresh();

            ExportTTarv etarv = new ExportTTarv();

            etarv.exportData(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            etarv.exportDataHistEstadoPaciente(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 70;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_resultadoLaboratorio ...";
            lblSMS.Refresh();

            ExportTResultadoLab lab = new ExportTResultadoLab();
            lab.exportDataTResultadoLaboratorio(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 75;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_aconselhamento ...";
            lblSMS.Refresh();

            ExportTAconselhamento eAconselhamento = new ExportTAconselhamento();

            eAconselhamento.exportAconselhamento(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date, mysqlUtil.getOtherConnection);

            eAconselhamento.exportAconselhamentoSeguimento(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date, mysqlUtil.getOtherConnection);

            barraProgresso.Value = 80;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_antecedentesClinicos, t_observacaopaciente,t_contacto ...";
            lblSMS.Refresh();

            ExportAntecedentesClinicosPaciente eAntecedente = new ExportAntecedentesClinicosPaciente();
            ExportTObservacaoPaciente eObs = new ExportTObservacaoPaciente();

            ExportTContacto contacto = new ExportTContacto();

            eAntecedente.exportAntecedentesClinicosPaciente(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            eObs.exportDataTObservacaoPaciente(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            contacto.exportContacto(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 85;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_exposicaotarvmae, t_exposicaotarvnascenca ...";
            lblSMS.Refresh();

            ExportExposicaoTarv expTarv = new ExportExposicaoTarv();
            expTarv.exportDataExposicaoTARV(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 90;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_tratamentoTB, t_QuestionarioTB ...";
            lblSMS.Refresh();

            ExportTTratamentoTB eTB = new ExportTTratamentoTB();
            eTB.exportDataTB(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            barraProgresso.Value = 95;
            barraProgresso.Refresh();

            lblSMS.Text = "Export da tabela t_Busaca ...";
            lblSMS.Refresh();

            ExportBuscaActiva eBusca = new ExportBuscaActiva();
            eBusca.exportDataBuscaActiva(mysqlUtil.getConexao, accessUtil.getConexao, dataInicial.Value.Date, dataFinal.Value.Date);

            if (chkTemGaac.Checked)
            {
                lblSMS.Text = "Export da tabela t_Gaac ...";
                lblSMS.Refresh();
                ExportTGaac eGaac = new ExportTGaac();
                eGaac.exportTGAAC(mysqlUtil.getConexao, accessUtil.getConexao, mysqlUtil.getOtherConnection);
            }


            barraProgresso.Value = 100;
            barraProgresso.Refresh();

           lblSMS.Text = "Exportacao Terminada ...";
           lblSMS.Refresh();

        }

        private void ImportacaoForm_Load(object sender, EventArgs e)
        {
            cboDistrito.SelectedIndex = 5;
            this.dataInicial.Value   = new DateTime(2006, 1, 1);
            this.dataFinal.Value = new DateTime(2012, 12, 31);
            this.txtHost.Text = "localhost";
            this.txtUsername.Text = "root";
            this.txtPassword.Text = "dm2007misau";
            this.txtDataSource.Text ="C:\\Users\\eurico.jose\\Desktop\\APR12\\Sistema Hdd_be.mdb";
        }

        private void btnDeidentify_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDataSource.Text))
            {
                MessageBox.Show("The field Data Source must not be empty...");
                txtDataSource.Focus();
                return;
            }
            AccessUtils accessUtil = new AccessUtils(txtDataSource.Text);
            
            InsertUtils insertUtils = new InsertUtils();
            
           System.Data.DataTable   dt = insertUtils.deidenfyDataBase(accessUtil.getConexao,accessUtil.getOtherConexao);


           Excel.Application oXL;
           Excel.Workbook oWB;
           Excel.Worksheet oSheet;
           Excel.Range oRange;

           // Start Excel and get Application object.
           oXL = new Excel.ApplicationClass();

           // Set some properties
           oXL.Visible = true;
           oXL.UserControl = true;

            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

           oXL.DisplayAlerts = false;

            

           // Get a new workbook.
           
           oWB = oXL.Workbooks.Add(Missing.Value);

           //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

           // Get the active sheet
           oSheet = (Excel.Worksheet)oWB.ActiveSheet;
           oSheet.Name = "NIDS";

           // Process the DataTable
           // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE
           //DataTable dt = Customers.RetrieveAsDataTable();

           int rowCount = 1;

           foreach (DataRow dr in dt.Rows)
           {
               rowCount += 1;
               for (int i = 1; i < dt.Columns.Count + 1; i++)
               {
                   // Add the header the first time through
                   if (rowCount == 2)
                   {
                       oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                   }
                   oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
               }
           }

           // Resize the columns
           oRange = oSheet.get_Range(oSheet.Cells[1, 1],
                         oSheet.Cells[rowCount, dt.Columns.Count]);
           oRange.EntireColumn.AutoFit();

           // Save the sheet and close
           oSheet = null;
           oRange = null;
           oWB.SaveAs("Deidentify_"+cboDistrito.Text+".xls", Excel.XlFileFormat.xlWorkbookNormal,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value,
               Excel.XlSaveAsAccessMode.xlExclusive,
               Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value);
           oWB.Close(Missing.Value, Missing.Value, Missing.Value);
           oWB = null;
           oXL.Quit();

           // Clean up
           // NOTE: When in release mode, this does the trick
           GC.WaitForPendingFinalizers();
           GC.Collect();
           GC.WaitForPendingFinalizers();
           GC.Collect();
           System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
            MessageBox.Show("Completed");
            

        }

       

        
    }
}
