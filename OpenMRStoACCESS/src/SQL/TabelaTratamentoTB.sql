Select distinct p.nid,dataInicio.dataInicio,datafim.datafim
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

left join (	 SELECT distinct o.person_id,e.encounter_id,o.value_datetime as dataInicio
			             FROM 	encounter e 
					                inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			             WHERE 	e.encounter_type in (6,9) and o.concept_id in (1113)  and o.voided=0 and e.voided=0
			               
                         UNION
                         
                         SELECT distinct o.person_id,e.encounter_id,o.obs_datetime as dataInicio
			             FROM 	encounter e 
					            inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			             WHERE 	e.encounter_type in (6,9) and o.concept_id in (1268) and o.value_coded=1256 and o.voided=0 and e.voided=0
                         
			) dataInicio on dataInicio.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datafim
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (6,9) and o.concept_id=6120 and o.voided=0 and e.voided=0
			) datafim on datafim.encounter_id=e.encounter_id

where e.encounter_type in (6,9) and  e.voided=0 and dataInicio is not null group by nid,dataInicio;





String sqlSelect = " SELECT	p.nid, ";
sqlSelect += "		dataInicio.dataInicio ";
sqlSelect += " FROM	t_paciente p  ";
sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id ";
sqlSelect += "        inner join  ";
sqlSelect += "        (	SELECT	o.person_id,e.encounter_id,o.value_datetime as dataInicio ";
sqlSelect += "			FROM 	encounter e ";
sqlSelect += "					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "			WHERE 	e.encounter_type in (6,9) and o.concept_id=1113  and o.voided=0 and e.voided=0 ";
sqlSelect += "			UNION ";
sqlSelect += "			SELECT	o.person_id,e.encounter_id,o.obs_datetime as dataInicio ";
sqlSelect += "			FROM 	encounter e ";
sqlSelect += "					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "			WHERE 	e.encounter_type in (6,9) and o.concept_id=1268 and o.value_coded=1256 and o.voided=0 and e.voided=0 ";
sqlSelect += "         ) dataInicio on dataInicio.encounter_id=e.encounter_id ";
sqlSelect += " WHERE	e.encounter_type in (6,9) and   ";
sqlSelect += "		e.voided=0 and p.nid is not null and  ";
sqlSelect += "		p.datanasc is not null and dataabertura between '2007-01-01' and '2011-12-31' "; 
sqlSelect += " GROUP BY nid,dataInicio ";





String sqlSelect = "SELECT	p.nid, ";
sqlSelect += "				datafim.datafim  ";
sqlSelect += " FROM	t_paciente p   ";
sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id  ";
sqlSelect += "        inner join   ";
sqlSelect += "		(	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datafim ";
sqlSelect += "            FROM 	encounter e  ";
sqlSelect += "                    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "            WHERE 	e.encounter_type in (6,9) and o.concept_id=6120 and o.voided=0 and e.voided=0 ";
sqlSelect += "         ) datafim on datafim.encounter_id=e.encounter_id ";