using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data .MySqlClient;
using System.Data;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TTarv
{
    public class ExportTTarv
    {
        public void exportData(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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

                String sqlSelect = "Select	p.patient_id,";
                sqlSelect += "e.encounter_id,";
                sqlSelect += "p.nid,";
                sqlSelect += "e.encounter_datetime as datatarv,";
                sqlSelect += "regime.codRegime,";
                sqlSelect += "case o.value_coded";
                sqlSelect += "	when 1256 then 'Inicia'";
                sqlSelect += "	when 1257 then 'Manter'";
                sqlSelect += "	when 1259 then 'Alterar'";
                sqlSelect += "	when 1369 then 'Transfer de'";
                sqlSelect += "	when 1705 then 'Reiniciar'";
                sqlSelect += "	when 1708 then 'Saida'";
                sqlSelect += "	else 'OUTRO' end as tipotarv,";
                sqlSelect += "	proxima.dataproxima,";
                sqlSelect += "	aviada.qtdComp,";
                sqlSelect += "	saldo.qtdSaldo,";
                sqlSelect += "	outro.dataoutro";
                sqlSelect += " from	t_paciente p ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "		left join	(";
                sqlSelect += "						select e.encounter_id,";
                sqlSelect += "								case o.value_coded";
                sqlSelect += "								when 792 then 'D4T+3TC+NVP'";
                sqlSelect += "								when 6110 then 'D4T20+3TC+NVP'";
                sqlSelect += "								when 1827 then 'D4T30+3TC+EFV'";
                sqlSelect += "								when 6103 then 'D4T+3TC+LPV'";
                sqlSelect += "								when 1651 then 'AZT+3TC+NVP'";
                sqlSelect += "								when 1703 then 'AZT+3TC+EFV'";
                sqlSelect += "								when 1702 then 'AZT+3TC+NFV'";
                sqlSelect += "								when 6100 then 'AZT+3TC+LPV'";
                sqlSelect += "								when 817 then 'AZT+3TC+ABC'";
                sqlSelect += "								when 6104 then 'ABC+3TC+EFV'";
                sqlSelect += "								when 6106 then 'ABC+3TC+LPV/r'";
                sqlSelect += "								when 6105 then 'ABC+3TC+NVP'";
                sqlSelect += "								when 6243 then 'TDF+3TC+NVP'";
                sqlSelect += "								when 6244 then 'AZT+3TC+RTV'";
                sqlSelect += "								when 1700 then 'AZT+DDl+NFV'";
                sqlSelect += "								when 633 then 'EFV'";
                sqlSelect += "								when 625 then 'D4T'";
                sqlSelect += "								when 631 then 'NVP'";
                sqlSelect += "								when 628 then '3TC'";
                sqlSelect += "								when 6107 then 'TDF+AZT+3TC+LPV/r'";
                sqlSelect += "								when 6236 then 'D4T+DDI+RTV-IP'";
                sqlSelect += "								when 1701 then 'ABC+DDI+NFV'";
                sqlSelect += "								else 'OUTROS' end as codRegime";
                sqlSelect += "						from	encounter e inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and o.concept_id=1088 and e.encounter_type=18";
                sqlSelect += "					) regime on regime.encounter_id=e.encounter_id";
                sqlSelect += "		left join	(";
                sqlSelect += "						select	e.encounter_id,";
                sqlSelect += "								o.value_datetime as dataproxima";
                sqlSelect += "						from	encounter e ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=5096";
                sqlSelect += "					) proxima on e.encounter_id=proxima.encounter_id";
                sqlSelect += "		left join	(";
                sqlSelect += "						select	e.encounter_id,";
                sqlSelect += "								o.value_numeric as qtdComp";
                sqlSelect += "						from	encounter e ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1715";
                sqlSelect += "					) aviada on e.encounter_id=aviada.encounter_id";
                sqlSelect += "		left join	(";
                sqlSelect += "						select	e.encounter_id,";
                sqlSelect += "								o.value_numeric as qtdSaldo";
                sqlSelect += "						from	encounter e ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1713";
                sqlSelect += "					) saldo on e.encounter_id=saldo.encounter_id";
                sqlSelect += "		left join	(";
                sqlSelect += "						select	e.encounter_id,";
                sqlSelect += "								o.value_datetime as dataoutro";
                sqlSelect += "						from	encounter e ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1190";
                sqlSelect += "					) outro on e.encounter_id=outro.encounter_id";
                sqlSelect += " where e.voided=0 and e.encounter_type=18 and o.voided=0 and o.concept_id=1255 and o.value_coded<>1708 and e.encounter_datetime between  '" + startDateMySQL + "' and '" + endDateMySQL + "'";
                sqlSelect += " and p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and p.nid is not null and regime.codregime is not null";

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

                        sqlInsert = "Insert into t_tarv(nid,datatarv,tipotarv,codregime) values(";
                        sqlInsert += "'" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'),'" + readerSource.GetString(5) + "','" + readerSource.GetString(4) + "')";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 idTarv = insertUtil.getMaxID(target, "t_tarv", "idtarv");

                        //insertUtil.updateStringValue("t_tarv", "codregime", commandTarge, readerSource, 4, "encounter_id", readerSource.GetString(1));

                        insertUtil.updateDateValue("t_tarv", "dataproxima", commandTarge, readerSource, 6, "idtarv", idTarv);

                        insertUtil.updateNumericValue("t_tarv", "QtdComp", commandTarge, readerSource, 7, "idtarv", idTarv);

                        insertUtil.updateNumericValue("t_tarv", "QtdSaldo", commandTarge, readerSource, 8, "idtarv", idTarv);

                        insertUtil.updateDateValue("t_tarv", "dataoutroservico", commandTarge, readerSource, 9, "idtarv", idTarv);
                    }
                    commandTarge.CommandText = "update t_tarv set dias=QtdComp/2 where QtdComp is not null";
                    commandTarge.ExecuteNonQuery();
                }
                
                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_TARV: " + e.Message);

            }
        }

        public void exportDataHistEstadoPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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

                String  sqlSelect = " select	p.patient_id,";
		                sqlSelect+="        e.encounter_id, ";
		                sqlSelect+="        p.nid, ";
		                sqlSelect+="        case    o.value_coded ";
		                sqlSelect+="                when 1707 then 'Abandono' ";
		                sqlSelect+="                when 1706 then 'Transferido para' ";
		                sqlSelect+="                when 1366 then 'Morte' ";
		                sqlSelect+="                when 1704 then 'HIV Negativo' ";
		                sqlSelect+="                when 1709 then 'Suspender Tarv' ";
		                sqlSelect+="                else 'Outro' end as codestado, ";
		                sqlSelect+="        e.encounter_datetime as dataestado, ";
		                sqlSelect+="        destino.destinopaciente ";
                        sqlSelect+=" from	t_paciente p ";
		                sqlSelect+="        inner join encounter e on p.patient_id=e.patient_id ";
		                sqlSelect+="        inner join obs o on o.encounter_id=e.encounter_id and o.person_id=e.patient_id ";
		                sqlSelect+="        left join	( ";
                        sqlSelect+="						select	e.encounter_id, ";
                        sqlSelect+="								o.value_text as destinopaciente ";
                        sqlSelect+="						from	encounter e ";
                        sqlSelect+="								inner join obs o on e.encounter_id=o.encounter_id ";
                        sqlSelect+="						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=2059 ";
                        sqlSelect+="					) destino on e.encounter_id=destino.encounter_id ";
                        sqlSelect+=" where	e.encounter_type in (18,6,9) and o.concept_id in (1708,6138) and o.voided=0 and e.voided=0 and p.nid is not null and ";
                        sqlSelect += "		p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        sqlInsert = "Insert into t_histestadopaciente(nid,codestado,dataestado) values(";
                        sqlInsert += "'" + readerSource.GetString(2) + "','" + readerSource.GetString(3) + "',cdate('" + readerSource.GetMySqlDateTime(4) + "'))";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        Int32 id = insertUtil.getMaxID(target, "t_histestadopaciente", "ID");


                        insertUtil.updateStringValue("t_histestadopaciente", "destinopaciente", commandTarge, readerSource, 5, "ID", id);

                    }
                    
                }

                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_HISTESTADOPACIENTE (MODULO EXPORTTTARV.CS): " + e.Message);

            }
        }


    }
}
