using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;


namespace ImportacaoOpenmrsForm.TObservacaoPaciente
{
    public class ExportTObservacaoPaciente
    {
        public void exportDataTObservacaoPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            exportNumericObsPaciente(source, target, startDate, endDate);
            exportCodedObsPaciente(source, target, startDate, endDate);
            exportTextObsPaciente(source, target, startDate, endDate);

            exportObsDataPaciente(source, target, startDate, endDate);
        }
        
        private void exportNumericObsPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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


            String  sqlSelect = " SELECT	p.patient_id, ";
                    sqlSelect += "		    e.encounter_id, ";
                    sqlSelect += "		    c.concept_id, ";
                    sqlSelect += "		    case c.concept_id ";
                    sqlSelect += "			    when 730 then 'CD4' ";
                    sqlSelect += "			    when 5497 then 'CD4' ";
                    sqlSelect += "              when 5085 then 'Tensão Arterial' ";
                    sqlSelect += "			    when 5086 then 'Tensão Arterial' ";
                    sqlSelect += "			    when 653 then 'AST' ";
                    sqlSelect += "			    when 654 then 'ALT' ";
                    sqlSelect += "			    when 1021 then 'LINFOCITO' ";
                    sqlSelect += "			    when 952 then 'LINFOCITO' ";
                    sqlSelect += "			    when 1022 then 'NEUTROFILO' ";
                    sqlSelect += "			    when 1330 then 'NEUTROFILO' ";
                    sqlSelect += "			    when 1024 then 'EOSINOFILO' ";
                    sqlSelect += "			    when 1332 then 'EOSINOFILO' ";
                    sqlSelect += "			    when 1025 then 'BASOFILO' ";
                    sqlSelect += "			    when 1333 then 'BASOFILO' ";
                    sqlSelect += "			    when 1023 then 'MONOCITO' ";
                    sqlSelect += "			    when 1331 then 'MONOCITO' ";
                    sqlSelect += "			    when 1017 then 'CMHC' ";
                    sqlSelect += "			    when 851 then 'VCM' ";
                    sqlSelect += "			    when 21 then 'Hemoglobina' ";
                    sqlSelect += "			    when 1018 then 'HGM' ";
                    sqlSelect += "			    when 678 then 'WBC' ";
                    sqlSelect += "			    when 679 then 'RBC' ";
                    sqlSelect += "			    when 5283 then 'INDICE DE KARNOFSKY' ";
                    sqlSelect += "			    when 5314 then 'Períneo' ";
                    sqlSelect += "			    when 1342 then 'IMC' ";
                    sqlSelect += "			    when 5088 then 'Temperatura' ";
                    sqlSelect += "			    when 5089 then 'Peso' ";
                    sqlSelect += "			    when 5090 then 'Altura' ";
                    sqlSelect += "			    when 5087 then 'Pulmonar - Auscultação' ";
                    sqlSelect += "			    when 5242 then 'Pulmonar - Respiracao' ";
                    sqlSelect += "			    when 729 then 'Plaquetas' ";
                    sqlSelect += "			    when 1015 then 'HTC' ";
                    sqlSelect += "			    when 5195 then 'Esplenomegáglia' ";
                    sqlSelect += "		    else cn.name end as codobservacao, ";
                    sqlSelect += "		    case c.concept_id ";
                    sqlSelect += "			    when 730 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 5497 then 'ABSOLUTO' ";
                    sqlSelect += "			    when 5085 then 'SUPERIOR' ";
                    sqlSelect += "			    when 5086 then 'INFERIOR' ";
                    sqlSelect += "			    when 1021 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 952 then 'ABSOLUTO' "; 
                    sqlSelect += "			    when 1022 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 1330 then 'ABSOLUTO' ";
                    sqlSelect += "			    when 1024 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 1332 then 'ABSOLUTO' ";
                    sqlSelect += "			    when 1025 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 1333 then 'ABSOLUTO' ";
                    sqlSelect += "			    when 1023 then 'PERCENTUAL' ";
                    sqlSelect += "			    when 1331 then 'ABSOLUTO' ";
                    sqlSelect += "		    else null end as codestado, ";
                    sqlSelect += "		    o.value_numeric as valor,	 ";
                    sqlSelect += "		    p.nid,	 ";
                    sqlSelect += "		    o.obs_datetime as data	 ";	
                    sqlSelect += " FROM	    t_paciente p ";
                    sqlSelect += "		    inner join	encounter e on p.patient_id=e.patient_id ";
                    sqlSelect += "		    inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                    sqlSelect += "		    inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and  cn.concept_name_type='FULLY_SPECIFIED'";                    
                    sqlSelect += "		    inner join	concept c on c.concept_id=o.concept_id      ";     
                    sqlSelect += " WHERE	e.encounter_type in (1,3) and   ";
                    sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and  ";
                    sqlSelect += "		    c.datatype_id=1 and c.is_set=0 and p.datanasc is not null and ";
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

                    sqlInsert = "Insert into t_observacaopaciente(codobservacao,valor,nid,data) values(";
                    sqlInsert += "'" + readerSource.GetString(3) + "','" + readerSource.GetString(5) + "','" + readerSource.GetString(6) + "',cdate('" + readerSource.GetMySqlDateTime(7) + "'))";
                    commandTarge.CommandText = sqlInsert;
                    commandTarge.ExecuteNonQuery();
                    if (!readerSource.IsDBNull(4))
                    {
                        Int32 idObs = insertUtil.getMaxID(target, "t_observacaopaciente", "idobservacao");
                        commandTarge.CommandText = "Update t_observacaopaciente set codestado='" + readerSource.GetString(4) + "' where idobservacao=" + idObs;
                        commandTarge.ExecuteNonQuery();
                    }

                }
            }
            readerSource.Close();
            
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_OBSERVACAOPACIENTE, NUMERICO (MODULO EXPORTTOBSERVACAOPACIENTE.CS): " + e.Message);

            }
        }

        private void exportCodedObsPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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


                String  sqlSelect = " SELECT	p.patient_id, ";
                        sqlSelect += "		    e.encounter_id, ";
                        sqlSelect += "		    c.concept_id, ";
                        sqlSelect += "		    case c.concept_id ";
                        sqlSelect += "			    when 300 then 'TIPAGEM SANGUINEA' ";
                        sqlSelect += "			    when 1655 then 'RPR' ";
                        sqlSelect += "			    when 299 then 'VDRL' ";
                        sqlSelect += "			    when 307 then 'BACILOSCOPIA' ";
                        sqlSelect += "			    when 1030 then 'PCR' ";
                        sqlSelect += "			    when 1120 then 'Pele' ";
                        sqlSelect += "			    when 1127 then 'Aparelho Articular' ";
                        sqlSelect += "			    when 1126 then 'Genitais' ";
                        sqlSelect += "			    when 1415 then 'Mucosas' ";
                        sqlSelect += "			    when 1129 then 'Neurológico' ";
                        sqlSelect += "			    when 509 then 'Inguinais aumentadados' ";
                        sqlSelect += "			    when 1124 then 'Pulmonar - Respiracao' ";
                        sqlSelect += "			    when 1427 then 'Pulmonar - Respiracao' ";
                        sqlSelect += "			    when 1125 then 'Abdómen' ";
                        sqlSelect += "			    when 5112 then 'Axilas aumentados' ";
                        sqlSelect += "			    when 643 then 'Cervicais aumentados' ";
                        sqlSelect += "			    when 1425 then 'Estado Hidratação' ";
                        sqlSelect += "			    when 1239 then 'Local do Corpo' ";
                        sqlSelect += "			    when 1629 then 'Cardiológico - Auscultação' ";
                        sqlSelect += "			    when 1550 then 'Ap: Sopro Tubárico' ";
                        sqlSelect += "			    when 1540 then 'Parótidas aumentadas' ";
                        sqlSelect += "			    when 5945 then 'Febre' ";
                        sqlSelect += "			    when 1552 then 'Pulmonar - Auscultação' ";
                        sqlSelect += "			    when 1548 then 'ap - Fervores' ";
                        sqlSelect += "			    when 5334 then 'Candidíase da orofarínge' ";
                        sqlSelect += "			    when 1545 then 'Ap - Mv' ";
                        sqlSelect += "			    when 1551 then 'Ap: Sopro anfórico' ";
                        sqlSelect += "			    when 562 then 'Sopro Cardiaco' ";
                        sqlSelect += "			    when 1419 then 'Icterícia' ";
                        sqlSelect += "			    when 1549 then 'ap: Roncos' ";
                        sqlSelect += "			    when 1621 then 'Antibioticos' ";
                        sqlSelect += "			    when 161 then 'Linfadenopatia Generalizada' ";
                        sqlSelect += "			    when 6121 then 'Uso de Cotrimoxazol' ";
                        sqlSelect += "			    when 1533 then 'Antibioticos' ";
                        sqlSelect += "			    when 1535 then 'Vitamina A' ";
                        sqlSelect += "			    when 12 then 'Raio X: Torax' ";
                        sqlSelect += "			    when 1192 then 'Uso de Antiretroviral' ";
                        sqlSelect += "			    when 1538 then 'Uso de Anitfungal' ";
                        sqlSelect += "			    when 1088 then 'Regime' ";
                        sqlSelect += "			    when 1532 then 'LCR Patologico' ";                   
                        sqlSelect += "		    else cn.name end as codobservacao, ";
                        sqlSelect += "		    cnc.name as codestado, ";
                        sqlSelect += "		    p.nid,	 ";
                        sqlSelect += "		    o.obs_datetime as data	 ";
                        sqlSelect += " FROM	    t_paciente p  ";
                        sqlSelect += "		    inner join	encounter e on p.patient_id=e.patient_id ";
                        sqlSelect += "		    inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                        sqlSelect += "		    inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and  ";
                        sqlSelect += "					    cn.concept_name_type='FULLY_SPECIFIED'  ";
                        sqlSelect += "		    inner join	concept_name cnc on cnc.concept_id=o.value_coded and cnc.locale='pt' and  ";
                        sqlSelect += "					    cnc.concept_name_type='FULLY_SPECIFIED'  ";
                        sqlSelect += "		    inner join	concept c on c.concept_id=o.concept_id	 ";
                        sqlSelect += " WHERE	e.encounter_type in (1,3) and   ";
                        sqlSelect += "		    o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and  ";
                        sqlSelect += "		    c.datatype_id=2 and c.is_set=0  and p.datanasc is not null and ";	
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

                        sqlInsert = "Insert into t_observacaopaciente(codobservacao,codestado,nid,data) values(";
                        sqlInsert += "'" + readerSource.GetString(3) + "','" + readerSource.GetString(4) + "','" + readerSource.GetString(5) + "',cdate('" + readerSource.GetMySqlDateTime(6) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }
                readerSource.Close();
               
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_OBSERVACAOPACIENTE, CODED (MODULO EXPORTTOBSERVACAOPACIENTE.CS): " + e.Message);

            }
        }


        private void exportTextObsPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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
                sqlSelect += "		    case c.concept_id ";
                sqlSelect += "			    when 1671 then 'Historia Actual' ";
                sqlSelect += "			    when 1474 then 'Terapeutica Outro' ";
                sqlSelect += "			    when 1649 then 'Outros Diagnosticos' ";
                sqlSelect += "			    when 1424 then 'Tipo Lesao de Cavidade Orofaringea' ";
                sqlSelect += "			    when 1543 then 'Exame Pulmonar Auscultacao, Outro' ";
                sqlSelect += "			    when 1423 then 'Tipo de Lesao da Pele' ";
                sqlSelect += "			    when 1396 then 'Outro Exame Abdomen' ";
                sqlSelect += "			    when 1407 then 'Outro Exame Neurologico' ";
                sqlSelect += "			    when 1678 then 'Outos exames Genitais' ";
                sqlSelect += "			    when 1542 then 'Exame Oral' ";
                sqlSelect += "			    when 1556 then 'Membros Inferiores' ";
                sqlSelect += "			    when 1555 then 'Membros Superiores' ";
                sqlSelect += "			    when 1554 then 'Perineo' ";
                sqlSelect += "			    when 1541 then 'Outros Tratamentos' ";
                sqlSelect += "			    when 1553 then 'Outras Massas' ";
                sqlSelect += "			    when 1536 then 'Uso de Cotrimoxazol, Especificamente' ";
                sqlSelect += "			    when 1642 then 'RX: Torax, Outro' ";
                sqlSelect += "		    else cn.name end as codobservacao, ";
                sqlSelect += "		o.value_text as valor, ";
                sqlSelect += "		p.nid,	 ";
                sqlSelect += "		o.obs_datetime as dataresultado ";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
                sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and  ";
                sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";
                sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id	 ";
                sqlSelect += " WHERE	e.encounter_type in (1,3) and  ";
                sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and  ";
                sqlSelect += "		c.datatype_id=3 and c.is_set=0 and p.datanasc is not null and ";
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

                        sqlInsert = "Insert into t_observacaopaciente(codobservacao,Observacao,nid,data) values(";
                        sqlInsert += "'" + readerSource.GetString(3) + "','" + readerSource.GetString(4) + "','" + readerSource.GetString(5) + "',cdate('" + readerSource.GetMySqlDateTime(6) + "'))";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }
                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_OBSERVACAOPACIENTE, CODED (MODULO EXPORTTOBSERVACAOPACIENTE.CS): " + e.Message);

            }
        }


        private void exportObsDataPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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


                String sqlSelect = " SELECT	p.nid,e.encounter_datetime,e.provider_id ";     
                sqlSelect += "       FROM	t_paciente p ";
                sqlSelect += "		        inner join	encounter e on p.patient_id=e.patient_id ";                
                sqlSelect += "       WHERE	e.encounter_type in (1,3) and e.voided=0 and ";                
                sqlSelect += "		        p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and ";
                sqlSelect += "		        e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

                

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

                        sqlInsert = "Insert into t_observacaodata(nid,data,medico) values(";
                        sqlInsert += "'" + readerSource.GetString(0) + "',cdate('" + readerSource.GetString(1) + "')," + readerSource.GetInt32(2) + ")";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                    }
                }
                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_OBSERVACAOPACIENTE, CODED (MODULO EXPORTTOBSERVACAOPACIENTE.CS): " + e.Message);

            }
        }
    }
}
