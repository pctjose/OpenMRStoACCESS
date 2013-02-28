using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.TAconselhamento
{
    public class ExportTAconselhamento
    {

        public void exportAconselhamento(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate, MySqlConnection otherSource)
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

                String sqlSelect = " SELECT	p.patient_id,";
                sqlSelect += "		e.encounter_id,";
                sqlSelect += "		p.nid,";
                sqlSelect += "		criteriosmedicos.criteriosmedicos,";
                sqlSelect += "		conceitos.conceitos,";
                sqlSelect += "		interessado.interessado,";
                sqlSelect += "		confidente.confidente,";
                sqlSelect += "		apareceregularmente.apareceregularmente,";
                sqlSelect += "		riscopobreaderencia.riscopobreaderencia,";
                sqlSelect += "		regimetratamento.regimetratamento,";
                sqlSelect += "		prontotarv.prontotarv,";
                sqlSelect += "		prontotarv.datapronto,";
                sqlSelect += "		obs.obs";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then 'SIM'";
                sqlSelect += "								when 1066 then 'NAO'";
                sqlSelect += "							end as criteriosmedicos";
                sqlSelect += "					FROM 	encounter e 	";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1248 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) criteriosmedicos on criteriosmedicos.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as conceitos";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1729 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) conceitos on conceitos.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as interessado";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1736 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) interessado on interessado.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as confidente";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1739 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) confidente on confidente.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as apareceregularmente";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1743 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) apareceregularmente on apareceregularmente.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as riscopobreaderencia";
                sqlSelect += "					FROM 	encounter e 	";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1749 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) riscopobreaderencia on riscopobreaderencia.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as regimetratamento";
                sqlSelect += "					FROM 	encounter e 	";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1752 and o.voided=0 and e.voided=0	";
                sqlSelect += "				  ) regimetratamento on regimetratamento.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then true";
                sqlSelect += "								when 1066 then false";
                sqlSelect += "							end as prontotarv,";
                sqlSelect += "							case o.value_coded";
                sqlSelect += "								when 1065 then o.obs_datetime";
                sqlSelect += "							else null end as datapronto";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1756 and o.voided=0 and e.voided=0";
                sqlSelect += "				  ) prontotarv on prontotarv.encounter_id=e.encounter_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							o.value_text as obs";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1757 and o.voided=0 and e.voided=0";
                sqlSelect += "				  ) obs on obs.encounter_id=e.encounter_id  ";
                sqlSelect += " WHERE	e.encounter_type in (19,29) and  e.voided=0 and";
                sqlSelect += "			p.nid is not null and ";
                sqlSelect += "			p.datanasc is not null and ";
                sqlSelect += "			p.dataabertura is not null and ";
                sqlSelect += "		    p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and ";
                sqlSelect += "		    e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        sqlInsert = "Insert into t_aconselhamento(nid) values(";
                        sqlInsert += "'" + readerSource.GetString(2) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 idAconselhamento = insertUtil.getMaxID(target, "t_aconselhamento", "idaconselhamento");


                        insertUtil.updateStringValue("t_aconselhamento", "criteriosmedicos", commandTarge, readerSource, 3, "idaconselhamento", idAconselhamento);

                        insertUtil.updateBooleanValue("t_aconselhamento", "conceitos", commandTarge, readerSource, 4, "idaconselhamento", idAconselhamento);
                        insertUtil.updateBooleanValue("t_aconselhamento", "interessado", commandTarge, readerSource, 5, "idaconselhamento", idAconselhamento);
                        insertUtil.updateBooleanValue("t_aconselhamento", "confidente", commandTarge, readerSource, 6, "idaconselhamento", idAconselhamento);

                        insertUtil.updateBooleanValue("t_aconselhamento", "apareceregularmente", commandTarge, readerSource, 7, "idaconselhamento", idAconselhamento);
                        insertUtil.updateBooleanValue("t_aconselhamento", "riscopobreaderencia", commandTarge, readerSource, 8, "idaconselhamento", idAconselhamento);

                        insertUtil.updateBooleanValue("t_aconselhamento", "regimetratamento", commandTarge, readerSource, 9, "idaconselhamento", idAconselhamento);
                        insertUtil.updateBooleanValue("t_aconselhamento", "prontotarv", commandTarge, readerSource, 10, "idaconselhamento", idAconselhamento);

                        insertUtil.updateDateValue("t_aconselhamento", "datapronto", commandTarge, readerSource, 11, "idaconselhamento", idAconselhamento);

                        insertUtil.updateStringValue("t_aconselhamento", "obs", commandTarge, readerSource, 12, "idaconselhamento", idAconselhamento);

                        exportActividadeAconselhamento(otherSource, target, readerSource.GetString(1), idAconselhamento);
                        
                    }

                }

                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_ACONSELHAMENTO (MODULO EXPORTTACONSELHAMENTO.CS): " + e.Message);

            }
        }


        private void exportActividadeAconselhamento(MySqlConnection source, OleDbConnection target, String encounterId, Int32 idAconselhamento)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String  sqlSelect = " SELECT	p.patient_id,";
                        sqlSelect += "		    e.encounter_id,";
                        sqlSelect += "		    p.nid,";
                        sqlSelect += "		    e.encounter_datetime as data,";
                        sqlSelect += "		    sessao.nrsessao,";
                        sqlSelect += "		    tipo.tipoactividade,";
                        sqlSelect += "		    confidente.apresentouconfidente";
                        sqlSelect += " FROM	t_paciente p";
                        sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
                        sqlSelect += "		left join	(	SELECT 	o.person_id,";
                        sqlSelect += "								o.encounter_id,";
                        sqlSelect += "								o.value_numeric as nrsessao";
                        sqlSelect += "						FROM 	encounter e ";
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1724 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) sessao on sessao.encounter_id=e.encounter_id";
                        sqlSelect += "		left join	(	SELECT 	o.person_id,";
                        sqlSelect += "								o.encounter_id,";
                        sqlSelect += "								case o.value_coded";
                        sqlSelect += "									when 1725 then 'GRUPO'";
                        sqlSelect += "									when 1726 then 'INDIVIDUAL'";
                        sqlSelect += "								end as tipoactividade";
                        sqlSelect += "						FROM 	encounter e ";
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1727 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) tipo on tipo.encounter_id=e.encounter_id";
                        sqlSelect += "		left join	(	SELECT 	o.person_id,";
                        sqlSelect += "								o.encounter_id,";
                        sqlSelect += "								case o.value_coded";
                        sqlSelect += "									when 1065 then true";
                        sqlSelect += "									when 1066 then false";
                        sqlSelect += "								end as apresentouconfidente";
                        sqlSelect += "						FROM 	encounter e ";
                        sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                        sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1728 and o.voided=0 and e.voided=0";
                        sqlSelect += "					) confidente on confidente.encounter_id=e.encounter_id";
                        sqlSelect += " WHERE	p.nid is not null and ";
                        sqlSelect += "			p.datanasc is not null and ";
                        sqlSelect += "			e.voided=0 and e.encounter_type in (19,29) and ";
                        sqlSelect += "			p.dataabertura is not null and ";
                        sqlSelect += "		    e.encounter_id=" + encounterId;

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {

                    //Int32 idAconselhamento = insertUtil.getMaxID(target, "t_aconselhamento", "idaconselhamento");

                    while (readerSource.Read())
                    {

                        sqlInsert = "Insert into t_actividadeaconselhamento(idaconselhamento,nid,data) values(";
                        sqlInsert += "" + idAconselhamento + ",'" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateNumericValue("t_actividadeaconselhamento", "nrsessao", commandTarge, readerSource, 4, "idaconselhamento",idAconselhamento);
                        insertUtil.updateStringValue("t_actividadeaconselhamento", "tipoactividade", commandTarge, readerSource, 5, "idaconselhamento", idAconselhamento);
                        insertUtil.updateBooleanValue("t_actividadeaconselhamento", "apresentouconfidente", commandTarge, readerSource, 6, "idaconselhamento",idAconselhamento);
                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_ACTIVIDADEACONSELHAMENTO (MODULO EXPORTTACONSELHAMENTO.CS): " + e.Message);

            }
        }



        public void exportAconselhamentoSeguimento(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate, MySqlConnection otherSource)
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

                String sqlSelect = " SELECT	p.patient_id,";
                sqlSelect += "		e.encounter_id,";
                sqlSelect += "		e.encounter_datetime,";
                sqlSelect += "		p.nid,";
                sqlSelect += "		resumo.obs";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "		left join (	SELECT 	o.person_id,";
                sqlSelect += "							e.encounter_id,";
                sqlSelect += "							o.value_text as obs";
                sqlSelect += "					FROM 	encounter e ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "					WHERE 	e.encounter_type=24 and o.concept_id=1553 and o.voided=0 and e.voided=0";
                sqlSelect += "				  ) resumo on resumo.encounter_id=e.encounter_id  ";               
                sqlSelect += " WHERE	e.encounter_type=24 and  e.voided=0 and";
                sqlSelect += "			p.nid is not null and ";
                sqlSelect += "			p.datanasc is not null and ";
                sqlSelect += "			p.dataabertura is not null and ";
                sqlSelect += "		    p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and ";
                sqlSelect += "		    e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        sqlInsert = "Insert into t_aconselhamento(nid) values(";
                        sqlInsert += "'" + readerSource.GetString(3) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 idAconselhamento = insertUtil.getMaxID(target, "t_aconselhamento", "idaconselhamento");


                       
                        insertUtil.updateStringValue("t_aconselhamento", "obs", commandTarge, readerSource, 4, "idaconselhamento", idAconselhamento);

                        exportActividadeAconselhamentoSeguimento(otherSource, target, readerSource.GetString(1), idAconselhamento);

                    }

                }

                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_ACONSELHAMENTOSEGUIMENTO (MODULO EXPORTTACONSELHAMENTO.CS): " + e.Message);

            }
        }


        private void exportActividadeAconselhamentoSeguimento(MySqlConnection source, OleDbConnection target, String encounterId, Int32 idAconselhamento)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String sqlSelect = " SELECT	p.patient_id,";
                sqlSelect += "		    e.encounter_id,";
                sqlSelect += "		    p.nid,";
                sqlSelect += "		    e.encounter_datetime as data,";                
                sqlSelect += "		    tipo.tipoactividade ";               
                sqlSelect += " FROM	t_paciente p";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "		left join	(	SELECT 	o.person_id,";
                sqlSelect += "								o.encounter_id,";
                sqlSelect += "								case o.value_coded";
                sqlSelect += "									when 2044 then 'Apoio Psicologico Individual'";
                sqlSelect += "									when 2045 then 'Apoio Social e Familiar'";
                sqlSelect += "									when 2046 then 'Informacao e Educacao Sobre Prevencao'";
                sqlSelect += "								end as tipoactividade";
                sqlSelect += "						FROM 	encounter e ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						WHERE 	e.encounter_type=24 and o.concept_id=2047 and o.voided=0 and e.voided=0";
                sqlSelect += "					) tipo on tipo.encounter_id=e.encounter_id";
                sqlSelect += " WHERE	p.nid is not null and ";
                sqlSelect += "			p.datanasc is not null and ";
                sqlSelect += "			e.voided=0 and e.encounter_type=24 and ";
                sqlSelect += "			p.dataabertura is not null and ";
                sqlSelect += "		    e.encounter_id=" + encounterId;

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {

                    //Int32 idAconselhamento = insertUtil.getMaxID(target, "t_aconselhamento", "idaconselhamento");

                    while (readerSource.Read())
                    {

                        sqlInsert = "Insert into t_actividadeaconselhamento(idaconselhamento,nid,data) values(";
                        sqlInsert += "" + idAconselhamento + ",'" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();
                        
                        insertUtil.updateStringValue("t_actividadeaconselhamento", "tipoactividade", commandTarge, readerSource, 4, "idaconselhamento", idAconselhamento);
                        
                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_ACTIVIDADEACONSELHAMENTOSEGUIMENTO (MODULO EXPORTTACONSELHAMENTO.CS): " + e.Message);

            }
        }
    }
}
