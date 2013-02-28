using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.TPaciente
{
    public class ExportTPaciente
    {
        public static void ExportData(MySqlConnection source, OleDbConnection target,DateTime startDate,DateTime endDate)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                //String sqlSelect="Select patient_id 0,hdd 1,dataabertura 2,nid 3,sexo 4,datanasc 5,idade 6,meses 7,";
                //sqlSelect+="coddistrito 8,codproveniencia 9,designacaoprov 10,designacaoprov 11,emtarv 12,datainiciotarv 13,";
                //sqlSelect +="codestado 14,destinopaciente 15,datasaidatarv 16,datadiagnostico 17,aconselhado 18,referidohdd 19,";
                //sqlSelect +="datareferidohdd 20,aceitabuscaactiva 21,dataaceitabuscaactiva 22,referidobuscaactiva 23,";
                //sqlSelect +="datareferenciabuscaactiva 24,situacaohiv 25 from t_paciente where dataabertura between "+startDate+ " and " + endDate;

                String sqlSelect = " Select patient_id,hdd,dataabertura,nid,sexo,datanasc,idade,meses,";
                sqlSelect += "      coddistrito,codproveniencia,designacaoprov,Codigoproveniencia,emtarv,datainiciotarv,";
                sqlSelect += "      codestado,destinopaciente,datasaidatarv,datadiagnostico,aconselhado,referidohdd,";
                sqlSelect += "      datareferidohdd,aceitabuscaactiva,dataaceitabuscaactiva,referidobuscaactiva,";
                sqlSelect += "      datareferenciabuscaactiva,situacaohiv,nome,identificacao,codbairro,celula,avenida, ";
                sqlSelect += "      codregime,apelido,provider_id,tipopaciente,cirurgias,transfusao,estadiooms,emtratamentotb from t_paciente ";
                sqlSelect += " where datanasc is not null and nid is not null and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {
                    while (readerSource.Read())
                    {
                        sqlInsert = "Insert into t_paciente(hdd,dataabertura,nid,sexo,datanasc,idade,coddistrito) values(";
                        sqlInsert += "'" + checkNull(readerSource, 1) + "',cdate('" + checkNull(readerSource, 2) + "'),'" + checkNull(readerSource, 3) + "','" + checkNull(readerSource, 4) + "',cdate('" + checkNull(readerSource, 5) + "')," + checkNull(readerSource, 6) + ",'" + checkNull(readerSource, 8) + "')";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateNumericValue("t_paciente", "meses", commandTarge, readerSource, 7, "nid", readerSource.GetString(3));
                        insertUtil.updateStringValue("t_paciente", "codproveniencia", commandTarge, readerSource, 9, "nid", readerSource.GetString(3));
                        insertUtil.updateStringValue("t_paciente", "designacaoprov", commandTarge, readerSource, 10, "nid", readerSource.GetString(3));
                        insertUtil.updateStringValue("t_paciente", "Codigoproveniencia", commandTarge, readerSource, 11, "nid", readerSource.GetString(3));

                        insertUtil.updateBooleanValue("t_paciente", "emtarv", commandTarge, readerSource, 12, "nid", readerSource.GetString(3));
                        insertUtil.updateDateValue("t_paciente", "datainiciotarv", commandTarge, readerSource, 13, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "codestado", commandTarge, readerSource, 14, "nid", readerSource.GetString(3));
                        insertUtil.updateStringValue("t_paciente", "destinopaciente", commandTarge, readerSource, 15, "nid", readerSource.GetString(3));

                        insertUtil.updateDateValue("t_paciente", "datasaidatarv", commandTarge, readerSource, 16, "nid", readerSource.GetString(3));
                        insertUtil.updateDateValue("t_paciente", "datadiagnostico", commandTarge, readerSource, 17, "nid", readerSource.GetString(3));

                        insertUtil.updateBooleanValue("t_paciente", "aconselhado", commandTarge, readerSource, 18, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "referidohdd", commandTarge, readerSource, 19, "nid", readerSource.GetString(3));

                        insertUtil.updateDateValue("t_paciente", "datareferidohdd", commandTarge, readerSource, 20, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "aceitabuscaactiva", commandTarge, readerSource, 21, "nid", readerSource.GetString(3));

                        insertUtil.updateDateValue("t_paciente", "dataaceitabuscaactiva", commandTarge, readerSource, 22, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "referidobuscaactiva", commandTarge, readerSource, 23, "nid", readerSource.GetString(3));

                        insertUtil.updateDateValue("t_paciente", "datareferenciabuscaactiva", commandTarge, readerSource, 24, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "situacaohiv", commandTarge, readerSource, 25, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "nome", commandTarge, readerSource, 26, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "identificacao", commandTarge, readerSource, 27, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "codbairro", commandTarge, readerSource, 28, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "celula", commandTarge, readerSource, 29, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "avenida", commandTarge, readerSource, 30, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "codregime", commandTarge, readerSource, 31, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "apelido", commandTarge, readerSource, 32, "nid", readerSource.GetString(3));

                        insertUtil.updateNumericValue("t_paciente", "codfuncionario", commandTarge, readerSource, 33, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "tipopaciente", commandTarge, readerSource, 34, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "cirurgias", commandTarge, readerSource, 35, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "transfusao", commandTarge, readerSource, 36, "nid", readerSource.GetString(3));

                        insertUtil.updateStringValue("t_paciente", "estadiooms", commandTarge, readerSource, 37, "nid", readerSource.GetString(3));

                        insertUtil.updateBooleanValue("t_paciente", "emtratamentotb", commandTarge, readerSource, 38, "nid", readerSource.GetString(3));

                    }
                }
                
                readerSource.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("Houve erro ao Exportar tabela T_PACIENTE:" + e.Message);
            }
            
            

        }

        private static String checkNull(MySqlDataReader reader, Int16 index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetValue(index).ToString();
        }
    }
   
}
