using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.TSeguimento
{
    public class ExportTSeguimento
    {
        public void exportDataTSeguimento(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate,MySqlConnection otherSource)
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

                String  sqlSelect = " SELECT    p.patient_id,";
                        sqlSelect += "		    e.encounter_id,";
                        sqlSelect += "		    p.nid,";
                        sqlSelect += "		    YEAR(e.encounter_datetime)-YEAR(p.datanasc) as idade,";
                        sqlSelect += "		    if(YEAR(e.encounter_datetime)-YEAR(p.datanasc)<2,PERIOD_DIFF(DATE_FORMAT(e.encounter_datetime,'%Y%m'),DATE_FORMAT(p.datanasc,'%Y%m')),null) meses,";
                        sqlSelect += "          estadohiv.estadohiv,";
                        sqlSelect += "          e.encounter_datetime as dataseguimento,";
                        sqlSelect += "          estadiooms.estadiooms,";
                        sqlSelect += "          dataproximaconsulta.dataproximaconsulta,";
                        sqlSelect += "          Gravidez.Gravidez,";
                        sqlSelect += "          outrodiagnostico.diagnostico,";
                        sqlSelect += "          gravidez1.gravida ";
                        sqlSelect += " FROM 	t_paciente p ";
                        sqlSelect += "		    inner join encounter e on e.patient_id=p.patient_id";
                        sqlSelect += "		    left join	(	SELECT 	o.person_id,e.encounter_id,";
                        sqlSelect += "								case o.value_coded";
                        sqlSelect += "								when 703 then 'Positivo'";
                        sqlSelect += "								when 664 then 'Negativo'";
                        sqlSelect += "								when 1138 then 'Indeterminado'";
                        sqlSelect += "								else 'OUTRO' end as estadohiv";
                        sqlSelect += "						FROM 	encounter e ";					
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id ";
                        sqlSelect += "						WHERE 	e.encounter_type=9 and o.concept_id=1040 and o.voided=0 and e.voided=0	";	   
                        sqlSelect += "					) estadohiv on estadohiv.encounter_id=e.encounter_id";
                        sqlSelect += "		    left join	(	SELECT  o.person_id,e.encounter_id,";
                        sqlSelect += "								case o.value_coded";
                        sqlSelect += "								when 1204 then 'I'";
                        sqlSelect += "								when 1205 then 'II'";
                        sqlSelect += "								when 1206 then 'III'";
                        sqlSelect += "								when 1207 then 'IV'";
                        sqlSelect += "								else 'OUTRO' end as estadiooms";
                        sqlSelect += "						FROM 	encounter e 			";		
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type in (6,9) and o.concept_id=5356 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) estadiooms on estadiooms.encounter_id=e.encounter_id";
                        sqlSelect += "		    left join	(	SELECT 	o.person_id,";
                        sqlSelect += "								e.encounter_id,";
                        sqlSelect += "								o.value_datetime as dataproximaconsulta";
                        sqlSelect += "						FROM 	encounter e ";
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type in (6,9) and o.concept_id=1410 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) dataproximaconsulta on dataproximaconsulta.encounter_id=e.encounter_id";
                        sqlSelect += "		    left join	(	SELECT 	o.person_id,";
                        sqlSelect += "								o.encounter_id,";
                        sqlSelect += "								o.value_numeric as Gravidez";
                        sqlSelect += "						FROM 	encounter e ";
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type=6 and o.concept_id= 5992 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) Gravidez on Gravidez.encounter_id=e.encounter_id";
                        sqlSelect += "          left join	(	SELECT 	o.person_id, ";
				        sqlSelect += "				                    o.encounter_id, ";
				        sqlSelect += "				                    o.value_text as diagnostico ";
				        sqlSelect += "		                     FROM 	encounter e ";
				        sqlSelect += "				                    inner join obs o on e.encounter_id=o.encounter_id ";
				        sqlSelect += "		                     WHERE 	e.encounter_type in (6,9) and o.concept_id= 1649 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "	                      ) outrodiagnostico on outrodiagnostico.encounter_id=e.encounter_id ";
                        sqlSelect += "          left join	(	SELECT 	o.person_id, ";
                        sqlSelect += "				                    o.encounter_id, ";
                        sqlSelect += "				                    'SIM' as gravida ";
                        sqlSelect += "		                     FROM 	encounter e ";
                        sqlSelect += "				                    inner join obs o on e.encounter_id=o.encounter_id ";
                        sqlSelect += "		                     WHERE 	e.encounter_type in (6,9) and o.concept_id= 1982 and o.voided=0 and e.voided=0 and o.value_coded=44";
                        sqlSelect += "	                      ) gravidez1 on gravidez1.encounter_id=e.encounter_id ";

                        sqlSelect += " WHERE	e.encounter_type in (6,9) and  ";
                        sqlSelect += "		e.voided=0 and ";
                        sqlSelect += "		p.nid is not null and ";
                        sqlSelect += "		p.dataabertura is not null and ";
                        sqlSelect += "		p.datanasc is not null and ";
                        sqlSelect += "		p.dataabertura between '"+startDateMySQL+"' and '"+endDateMySQL+"' and ";
                        sqlSelect += "		e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        sqlInsert = "Insert into t_seguimento(nid,idade,dataseguimento,tiposeguimento) values(";
                        sqlInsert += "'" + readerSource.GetString(2) + "'," + readerSource.GetString(3) + ",cdate('" + readerSource.GetMySqlDateTime(6) + "'),'Seguinte')";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 idSeguimento = insertUtil.getMaxID(target, "t_seguimento", "idseguimento");

                        insertUtil.updateNumericValue("t_seguimento", "meses", commandTarge, readerSource, 4, "idseguimento", idSeguimento);
                        insertUtil.updateStringValue("t_seguimento", "estadohiv", commandTarge, readerSource, 5, "idseguimento", idSeguimento);
                        insertUtil.updateStringValue("t_seguimento", "estadiooms", commandTarge, readerSource, 7, "idseguimento", idSeguimento);
                        insertUtil.updateDateValue("t_seguimento", "dataproximaconsulta", commandTarge, readerSource, 8, "idseguimento", idSeguimento);
                        insertUtil.updateNumericValue("t_seguimento", "Gravidez", commandTarge, readerSource, 9, "idseguimento", idSeguimento);
                        insertUtil.updateStringValue("t_seguimento", "Observacao", commandTarge, readerSource, 10, "idseguimento", idSeguimento);
                        insertUtil.updateStringValue("t_seguimento", "gravida", commandTarge, readerSource, 11, "idseguimento", idSeguimento);

                        

                        exportInfeccoesOportunistasSeguimento(otherSource, target, readerSource.GetString(1), idSeguimento);
                        exportTratamentoSeguimento(otherSource, target, readerSource.GetString(1), idSeguimento);
                        exportDiagnosticoSeguimento(otherSource, target, readerSource.GetString(1), idSeguimento);
                    }

                }

                readerSource.Close();
                UpdateScreenTB(source, target);

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_SEGUIMENTO (MODULO EXPORTTSEGUIMENTO.CS): " + e.Message);

            }
        }


        private void exportInfeccoesOportunistasSeguimento(MySqlConnection source, OleDbConnection target, String encounterId, Int32 idSeguimento)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

               

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();               

                String  sqlSelect = " SELECT	case	o.concept_id ";
                        sqlSelect += "				when 1564 then 'I' ";
                        sqlSelect += "				when 1565 then 'II' ";
                        sqlSelect += "				when 1566 then 'III' ";
                        sqlSelect += "				when 1569 then 'IV' ";
                        sqlSelect += "				when 1558 then 'I' ";
                        sqlSelect += "				when 1561 then 'II' ";
                        sqlSelect += "				when 1562 then 'III' ";
                        sqlSelect += "				when 2066 then 'IV' ";
                        sqlSelect += "				end as estadioms, ";
                        sqlSelect += "		if(length(cn.name)>50,concat(left(cn.name,45),' ...'),cn.name) as codigoio, ";
                        sqlSelect += "		p.nid, ";
                        sqlSelect += "		o.obs_datetime as data ";
                        sqlSelect += " FROM	t_paciente p ";
                        sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
                        sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                        sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and ";
                        sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";          
                        sqlSelect += " WHERE	e.encounter_type in (6,9) and o.concept_id in (1564,1565,1566,1569,1558,1561,1562,2066) and ";
                        sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and e.encounter_id=" + encounterId;
                
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

                        sqlInsert = "Insert into t_infeccoesoportunisticaseguimento(idseguimento,estadiooms,codigoio,Nid,Data) values(";
                        sqlInsert += ""+idSeguimento+",'" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "','" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();                      

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_INFECCOESOPORTUNISTASSEGUIMENTO (MODULO EXPORTTSEGUIMENTO.CS): " + e.Message);

            }
        }


        private void exportTratamentoSeguimento(MySqlConnection source, OleDbConnection target, String encounterId, Int32 idSeguimento)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;



                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String  sqlSelect = " SELECT ";                        
                        sqlSelect += "		cn.name as codtratamento, ";
		                sqlSelect += "          p.nid,";
		                sqlSelect += "          o.obs_datetime as data";
                        sqlSelect += " FROM	    t_paciente p";
                        sqlSelect += "		    inner join	encounter e on p.patient_id=e.patient_id";
                        sqlSelect += "		    inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id";
                        sqlSelect += "		    inner join	concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and ";
                        sqlSelect += "					    cn.concept_name_type='FULLY_SPECIFIED'";           
                        sqlSelect += " WHERE	e.encounter_type in (6,9) and o.concept_id=1719 and ";
                        sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and e.encounter_id=" + encounterId;

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {

                    //Int32 idSeguimento = insertUtil.getMaxID(target, "t_seguimento", "idseguimento");

                    while (readerSource.Read())
                    {

                        sqlInsert = "Insert into t_tratamentoseguimento(idseguimento,codtratamento,nid,data) values(";
                        sqlInsert += "" + idSeguimento + ",'" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "',cdate('" + readerSource.GetMySqlDateTime(2) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_TRATAMENTOSEGUIMENTO (MODULO EXPORTTSEGUIMENTO.CS): " + e.Message);

            }
        }

        private void exportDiagnosticoSeguimento(MySqlConnection source, OleDbConnection target, String encounterId, Int32 idSeguimento)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;



                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String sqlSelect = " SELECT	";
                sqlSelect += "		case cn.concept_id ";
                sqlSelect += "			when 123 then 'MALARIA' ";
                sqlSelect += "			when 68 then 'DESNUTRIÇÃO AGUDA MODERADA (DAM)' ";
                sqlSelect += "			when 3 then 'ANEMIA' ";
                sqlSelect += "		else cn.name end as diagnostico, ";
                sqlSelect += "              p.nid,";
                sqlSelect += "              o.obs_datetime as data";
                sqlSelect += " FROM	    t_paciente p";
                sqlSelect += "		    inner join	encounter e on p.patient_id=e.patient_id";
                sqlSelect += "		    inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id";
                sqlSelect += "		    inner join	concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and ";
                sqlSelect += "					    cn.concept_name_type='FULLY_SPECIFIED'";
                sqlSelect += " WHERE	e.encounter_type in (6,9) and o.concept_id=1406 and ";
                sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and e.encounter_id=" + encounterId;

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {

                    //Int32 idSeguimento = insertUtil.getMaxID(target, "t_seguimento", "idseguimento");

                    while (readerSource.Read())
                    {
                        String aux = readerSource.GetString(0);
                        if(aux.Length>50 ){
                            aux =aux.Substring(0,45);
                            aux +=" ...";
                        }
                        sqlInsert = "Insert into t_diagnosticoseguimento(idseguimento,coddiagnostico,Nid,Data) values(";
                        sqlInsert += "" + idSeguimento + ",'" + aux + "','" + readerSource.GetString(1) + "',cdate('" + readerSource.GetMySqlDateTime(2) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_DIAGNOSTICOSEGUIMENTO (MODULO EXPORTTSEGUIMENTO.CS): " + e.Message);

            }
        }

        private void UpdateScreenTB(MySqlConnection source, OleDbConnection target)
        {

            try
            {
            MySqlCommand commandSource = new MySqlCommand();
            OleDbCommand commandTarge = new OleDbCommand();
            MySqlDataReader readerSource;



            String sqlInsert;

            InsertUtils insertUtil = new InsertUtils();

            String sqlSelect = " SELECT	e.encounter_datetime,";
            sqlSelect += "              p.nid ";
            sqlSelect += " FROM	    t_paciente p";
            sqlSelect += "		    inner join	encounter e on p.patient_id=e.patient_id ";
            sqlSelect += " WHERE	e.encounter_type=20 and ";
            sqlSelect += "		                e.voided=0 and p.nid is not null ";

            commandTarge.Connection = target;
            commandTarge.CommandType = CommandType.Text;

            commandSource.Connection = source;
            commandSource.CommandType = CommandType.Text;
            commandSource.CommandText = sqlSelect;
            readerSource = commandSource.ExecuteReader();

            if (readerSource.HasRows)
            {

                //Int32 idSeguimento = insertUtil.getMaxID(target, "t_seguimento", "idseguimento");

                while (readerSource.Read())
                {

                    sqlInsert = "update t_seguimento set screeningtb='SIM' where nid='" + readerSource.GetString(1) + "' and dataseguimento=cdate('" + readerSource.GetString(0) + "')";

                    commandTarge.CommandText = sqlInsert;

                    commandTarge.ExecuteNonQuery();

                }
            }
            readerSource.Close();
            //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_DIAGNOSTICOSEGUIMENTO (MODULO EXPORTTSEGUIMENTO.CS): " + e.Message);

            }
        }


    }
}
