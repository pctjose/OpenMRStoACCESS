using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Data;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TExposicaoTarv
{
    public class ExportExposicaoTarv
    {
        public void exportDataExposicaoTARV(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            exportDataExposicaoTarvNascenca(source, target, startDate, endDate);
            exportDataEsposicaoTarvMae(source, target, startDate, endDate);
        }
        private void exportDataExposicaoTarvNascenca(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            //try
            //{
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String sqlSelect = "Select distinct p.nid,tarv.tarv";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "      inner join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                  case o.value_coded";
                sqlSelect += "                   when 631 then 'NVP'";
                sqlSelect += "                   when 797 then 'AZT'";
                sqlSelect += "                   when 792 then 'D4T+3TC+NVP'";
                sqlSelect += "                   when 1800 then 'TARV'";
                sqlSelect += "                   when 1801 then 'AZT+NVP'";
                sqlSelect += "                   when 630 then 'AZT+3TC'";
                sqlSelect += "                   when 628 then '3TC'";
                sqlSelect += "                   else '' end as tarv";
                sqlSelect += "      FROM 	encounter e";
                sqlSelect += "              inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "      WHERE 	e.encounter_type=7 and o.concept_id=1503 and o.voided=0 and e.voided=0";
                sqlSelect += "               ) tarv on tarv.encounter_id=e.encounter_id";
                sqlSelect += "      where e.encounter_type=7 and p.nid is not null and  e.voided=0 and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "';";
                
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
                        sqlInsert = "Insert into t_esposicaotarvnascenca(nid,tarv) values('" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                     }
                }
                
                readerSource.Close();

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show("Houve erro ao Exportar tabela T_EXPOSICAOTARVNASCENCA:"+e.Message);
            //}
          }

        private void exportDataEsposicaoTarvMae(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            //try
            //{
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String sqlSelect = "Select distinct p.nid,tarv.tarv";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "      inner join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                  case o.value_coded";
                sqlSelect += "                   when 631 then 'NVP'";
                sqlSelect += "                   when 797 then 'AZT'";
                sqlSelect += "                   when 792 then 'D4T+3TC+NVP'";
                sqlSelect += "                   when 1800 then 'TARV'";
                sqlSelect += "                   when 1801 then 'AZT+NVP'";
                sqlSelect += "                   when 630 then 'AZT+3TC'";
                sqlSelect += "                   when 628 then '3TC'";
                sqlSelect += "                   else '' end as tarv";
                sqlSelect += "      FROM 	encounter e";
                sqlSelect += "              inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "      WHERE 	e.encounter_type=7 and o.concept_id=1504 and o.voided=0 and e.voided=0";
                sqlSelect += "               ) tarv on tarv.encounter_id=e.encounter_id";
                sqlSelect += "      where e.encounter_type=7 and p.nid is not null and  e.voided=0 and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' group by p.nid;";

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
                        sqlInsert = "Insert into t_esposicaotarvmae(nid,tarv) values('" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }

                readerSource.Close();

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show("Houve erro ao Exportar tabela T_EXPOSICAOTARVMAE:" + e.Message);
            //}
        }

    }
}
