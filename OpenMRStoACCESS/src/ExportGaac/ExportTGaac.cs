using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.ExportGaac
{
    public class ExportTGaac
    {
        private void exportTActividade(MySqlConnection source, OleDbConnection target, Int32 idGaac)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;



                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String sqlSelect = " SELECT	p.nid,gm.start_date,gm.end_date,rl.name as reason,g.gaac_identifier, ";
                sqlSelect += "		gm.description ";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join	gaac_member gm on p.patient_id=gm.member_id ";
                sqlSelect += "		inner join	gaac g on g.gaac_id=gm.gaac_id ";
                sqlSelect += "		left join	gaac_reason_leaving_type rl on rl.gaac_reason_leaving_type_id=gm.reason_leaving_type ";
                sqlSelect += " WHERE	g.voided=0 and gm.voided=0 and gm.gaac_id=" + idGaac;

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {

                    // Int32 idSeguimento = insertUtil.getMaxID(target, "t_seguimento", "idseguimento");

                    while (readerSource.Read())
                    {

                        sqlInsert = "Insert into t_gaac_actividades(nid,dataInscricao,numGAAC) values(";
                        sqlInsert += "'" + readerSource.GetString(0) + "','" + readerSource.GetMySqlDateTime(1) + "'," + Convert.ToInt32( readerSource.GetString(4)) + ")";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 maxActividade = insertUtil.getMaxID(target, "t_gaac_actividades", "ID");
                        insertUtil.updateStringValue("t_gaac_actividades", "motivo", commandTarge, readerSource, 3, "ID", maxActividade);
                        insertUtil.updateStringValue("t_gaac_actividades", "observacao", commandTarge, readerSource, 5, "ID", maxActividade);
                        insertUtil.updateDateValue("t_gaac_actividades", "dataSaida", commandTarge, readerSource, 2, "ID", maxActividade);

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_GAAC_ACTIVIDADES (Modulo ExportTGaac.cs " + e.Message);

            }
        }


        public void exportTGAAC(MySqlConnection source, OleDbConnection target, MySqlConnection otherSource)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();                

                String sqlSelect = " SELECT l.name as hdd, ";
                sqlSelect += "          g.gaac_identifier as numGaac, ";
                sqlSelect += "		    g.start_date as datainicio, ";
                sqlSelect += "		    at.name as afinidade, ";
                sqlSelect += "          g.date_crumbled as dataDesintegracao, ";
                sqlSelect += "		    p.nid as nidPontoFocal, ";
                sqlSelect += "          g.description as observacao, ";
                sqlSelect += "          g.gaac_id ";
                sqlSelect += " FROM 	gaac g  ";
                sqlSelect += "		    left join t_paciente p on g.focal_patient_id=p.patient_id ";
                sqlSelect += "		    left join gaac_affinity_type at on g.affinity_type=at.gaac_affinity_type_id";
                sqlSelect += "		    left join location l on g.location_id=l.location_id ";
                sqlSelect += " WHERE	g.voided=0  ";

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

                        Int32 numGaac = Convert.ToInt32(readerSource.GetString(1));
                        sqlInsert = "Insert into t_gaac(hdd,numGAAC,datainicio,afinidade) values(";
                        sqlInsert += "'" + readerSource.GetString(0) + "'," + numGaac + ",cdate('" + readerSource.GetMySqlDateTime(2) + "'),'" + readerSource.GetString(3) + "')";

                        

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateDateValue("t_gaac", "dataDesintegracao", commandTarge, readerSource, 4, "numGAAC", numGaac);
                        insertUtil.updateDateValue("t_gaac", "nidPontoFocal", commandTarge, readerSource, 5, "numGAAC", numGaac);
                        insertUtil.updateDateValue("t_gaac", "observacao", commandTarge, readerSource, 6, "numGAAC", numGaac);

                        
                        this.exportTActividade(otherSource,target,readerSource.GetInt32(7));
                        
                    }

                }                

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_GAAC (MODULO EXPORTTGAAC.CS): " + e.Message);

            }
        }

    }
}
