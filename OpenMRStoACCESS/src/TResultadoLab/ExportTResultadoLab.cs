using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.TResultadoLab
{
    public class ExportTResultadoLab
    {

        public void exportDataTResultadoLaboratorio(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            exportNumericLabResult(source, target, startDate, endDate);
            exportCodedLabResult(source, target, startDate, endDate);
        }

        private void exportNumericLabResult(MySqlConnection source, OleDbConnection target, DateTime startDate,DateTime endDate)
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


                String sqlSelect = " SELECT	p.patient_id, ";
                sqlSelect += "		e.encounter_id, ";
                sqlSelect += "		c.concept_id, ";
                sqlSelect += "		case c.concept_id ";
                sqlSelect += "			when 730 then 'CD4' ";
                sqlSelect += "			when 5497 then 'CD4' ";
                sqlSelect += "			when 653 then 'AST' ";
                sqlSelect += "			when 654 then 'ALT' ";
                sqlSelect += "			when 1021 then 'L' ";
                sqlSelect += "			when 952 then 'L' ";
                sqlSelect += "			when 1022 then 'N' ";
                sqlSelect += "			when 1330 then 'N' ";
                sqlSelect += "			when 1024 then 'E' ";
                sqlSelect += "			when 1332 then 'E' ";
                sqlSelect += "			when 1025 then 'B' ";
                sqlSelect += "			when 1333 then 'B' ";
                sqlSelect += "			when 1023 then 'M' ";
                sqlSelect += "			when 1331 then 'M' ";
                sqlSelect += "			when 1017 then 'CMHC' ";
                sqlSelect += "			when 851 then 'VGM' ";
                sqlSelect += "			when 21 then 'Hemoglobina' ";
                sqlSelect += "			when 1018 then 'HGM' ";
                sqlSelect += "			when 678 then 'WBC' ";
                sqlSelect += "			when 679 then 'RBC' ";
                sqlSelect += "			when 1015 then 'HTC' ";
                sqlSelect += "			when 729 then 'Plaquetas' ";
                sqlSelect += "			when 1016 then 'RDW' ";
                sqlSelect += "			when 1307 then 'MPV' ";
                sqlSelect += "			when 1011 then 'CK' ";
                sqlSelect += "			when 857 then 'Ureia' ";
                sqlSelect += "			when 790 then 'Creatinina' ";
                sqlSelect += "			when 848 then 'Albumina' ";
                sqlSelect += "			when 655 then 'Bilirrubina T' ";
                sqlSelect += "			when 887 then 'GLC' ";
                sqlSelect += "			when 1297 then 'Bilirrubina C' ";
                sqlSelect += "			when 1299 then 'Amilase' ";
                sqlSelect += "			when 855 then 'VS' ";
                sqlSelect += "		else cn.name end as codexame, ";
                sqlSelect += "		p.nid,	";
                sqlSelect += "		o.obs_datetime as dataresultado, ";
                sqlSelect += "		case c.concept_id ";
                sqlSelect += "			when 730 then 'PERCENTUAL' ";
                sqlSelect += "			when 5497 then 'ABSOLUTO' ";
                sqlSelect += "			when 1021 then 'PERCENTUAL' ";
                sqlSelect += "			when 952 then 'ABSOLUTO' ";
                sqlSelect += "			when 1022 then 'PERCENTUAL' ";
                sqlSelect += "			when 1330 then 'ABSOLUTO' ";
                sqlSelect += "			when 1024 then 'PERCENTUAL' ";
                sqlSelect += "			when 1332 then 'ABSOLUTO' ";
                sqlSelect += "			when 1025 then 'PERCENTUAL' ";
                sqlSelect += "			when 1333 then 'ABSOLUTO' ";
                sqlSelect += "			when 1023 then 'PERCENTUAL' ";
                sqlSelect += "			when 1331 then 'ABSOLUTO' ";
                sqlSelect += "		else null end as codparametro, ";
                sqlSelect += "		o.value_numeric as resultado, ";
                sqlSelect += "		pedido.data_pedido ";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
                sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and ";
                sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";
                sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id   ";
                sqlSelect += "      left join	( ";
                sqlSelect += "						select	e.encounter_id, ";
                sqlSelect += "								o.value_datetime as data_pedido ";
                sqlSelect += "						from	encounter e  ";
                sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=13 and o.concept_id=6246 ";
                sqlSelect += "					) pedido on e.encounter_id=pedido.encounter_id ";
                sqlSelect += " WHERE	e.encounter_type=13 and  ";
                sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and p.datanasc is not null and ";
                sqlSelect += "		    c.datatype_id=1 and c.is_set=0 and	";
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

                        sqlInsert = "Insert into t_resultadoslaboratorio(codexame,nid,dataresultado,resultado) values(";
                        sqlInsert += "'" + readerSource.GetString(3) + "','" + readerSource.GetString(4) + "',cdate('" + readerSource.GetString(5) + "'),'" + readerSource.GetDouble(7) + "')";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();
                        Int32 idLab = insertUtil.getMaxID(target, "t_resultadoslaboratorio", "idresultado");
                        if (!readerSource.IsDBNull(6))
                        {
                            
                            commandTarge.CommandText = "Update t_resultadoslaboratorio set codparametro='" + readerSource.GetString(6) + "' where idresultado=" + idLab;
                            commandTarge.ExecuteNonQuery();
                        }
                        if (!readerSource.IsDBNull(8))
                        {

                            commandTarge.CommandText = "Update t_resultadoslaboratorio set datapedido=cdate('" + readerSource.GetString(8) + "') where idresultado=" + idLab;
                            commandTarge.ExecuteNonQuery();
                        }

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_RESULTADOLABORATORIO, NUMERICO (MODULO EXPORTTRESULTADOLAB.CS): " + e.Message);

            }
        }

        private void exportCodedLabResult(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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


                String sqlSelect = " SELECT	p.patient_id, ";
                sqlSelect += "		        e.encounter_id, ";
                sqlSelect += "		        c.concept_id, ";
                sqlSelect += "		        case c.concept_id ";
                sqlSelect += "			        when 300 then 'TIPAGEM SANGUINEA' ";
                sqlSelect += "			        when 1655 then 'RPR' ";
                sqlSelect += "			        when 299 then 'VDRL' ";
                sqlSelect += "			        when 307 then 'BACILOSCOPIA' ";
                sqlSelect += "			        when 1030 then 'PCR' ";
                sqlSelect += "		        else cn.name end as codexame, ";
                sqlSelect += "		        p.nid,	";
                sqlSelect += "		        o.obs_datetime as dataresultado, ";
                sqlSelect += "		        case o.value_coded ";
                sqlSelect += "			        when 1229 then 'NAO REACTIVO' ";
                sqlSelect += "			        when 1228 then 'REACTIVO' ";
                sqlSelect += "			        when 1304 then 'MA QUALIDADE DE AMOSTRA' ";
                sqlSelect += "			        when 664 then 'NEGATIVO' ";
                sqlSelect += "			        when 703 then 'POSITIVO' ";
                sqlSelect += "			        when 1138 then 'INDETERMINADO' ";
                sqlSelect += "			        when 690 then 'A POSITIVO' ";
                sqlSelect += "			        when 692 then 'A NEGATIVO' ";
                sqlSelect += "			        when 694 then 'B POSITIVO' ";
                sqlSelect += "			        when 696 then 'B NEGATIVO' ";
                sqlSelect += "			        when 699 then 'O POSITIVO' ";
                sqlSelect += "			        when 701 then 'O NEGATIVO' ";
                sqlSelect += "			        when 1230 then 'AB POSITIVO' ";
                sqlSelect += "			        when 1231 then 'AB NEGATIVO' ";
                sqlSelect += "		        else 'OUTRO' end as codparametro ";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
                sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and ";
                sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";
                sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id  ";
                sqlSelect += " WHERE	e.encounter_type=13 and  ";
                sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and p.datanasc is not null and ";
                sqlSelect += "		c.datatype_id=2 and c.is_set=0	and ";
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

                        sqlInsert = "Insert into t_resultadoslaboratorio(codexame,nid,dataresultado,codparametro) values(";
                        sqlInsert += "'" + readerSource.GetString(3) + "','" + readerSource.GetString(4) + "',cdate('" + readerSource.GetString(5) + "'),'" + readerSource.GetString(6) + "')";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();                        

                    }
                }
                readerSource.Close();
                //otherSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_RESULTADOLABORATORIO, CODED (MODULO EXPORTTRESULTADOLAB.CS): " + e.Message);

            }
        }
    }
}
