select person_id as nid, concept_id as codantecedentes, obs_datetime as datadiagnostico, if(value_coded=1065, 'Sim','Não') as Estado 
from obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
where concept_id in (42, 5042, 836, 5334, 5340, 507,1379, 1380, 1381, 5018, 5339, 5027, 1429,1629)and o.voided=0 and e.voided=0 and encounter_type=5
order by nid


------------------------
Analise de Performance


Select distinct codantecendentes.codantecendentes,p.nid,codantecendentes.datadiagnostico
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

inner join (	SELECT 	distinct o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					when 1457 then 'SEM INFORMACAO'
					else '' end as estado,o.obs_datetime as datadiagnostico
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id in (42, 5042, 836, 5334, 5340, 507,1379, 1380, 1381, 5018, 5339, 5027, 1429,5030,5965,5050,204,1215) and o.voided=0 and e.voided=0		   
		   ) codantecendentes on codantecendentes.encounter_id=e.encounter_id

where e.encounter_type in (5) and  e.voided=0;



String  sqlSelect = " SELECT	p.patient_id,";
sqlSelect += "					e.encounter_id,";
sqlSelect += "					cn.name as codantecendentes,";
sqlSelect += "					p.nid,";
sqlSelect += "					o.obs_datetime as datadiagnostico,";		
sqlSelect += "		case o.value_coded";
sqlSelect += "			when 1065 then 'SIM'";
sqlSelect += "			when 1066 then 'NAO'";
sqlSelect += "			when 1457 then 'SEM INFORMACAO'";
sqlSelect += "		else '' end as estado";		
sqlSelect += " FROM	t_paciente p";
sqlSelect += "		inner join encounter e on p.patient_id=e.patient_id";
sqlSelect += "		inner join obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id";
sqlSelect += "		inner join concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'";           
sqlSelect += " WHERE	e.encounter_type in (5,7) and ";
sqlSelect += "		o.concept_id in (42, 5042, 836, 5334, 5340, 507,1379, 1380, 1381, 5018, 5339, 5027, 1429,5030,5965,5050,204,1215) and ";
sqlSelect += "		o.voided=0 and ";
sqlSelect += "		cn.voided=0 and ";
sqlSelect += "		e.voided=0 and ";
sqlSelect += "		p.datanasc is not null and ";
sqlSelect += "		p.nid is not null and ";
		
-------------------------------------------------------------------------

SELECT	p.patient_id,
		e.encounter_id,
		cn.name as codantecendentes,
		p.nid,
		o.obs_datetime as datadiagnostico,		
		case o.value_coded
			when 1065 then 'SIM'
			when 1066 then 'NAO'
			when 1457 then 'SEM INFORMACAO'
		else '' end as estado		
FROM	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
		inner join obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'           
WHERE	e.encounter_type in (5,7) and 
		o.concept_id in (42, 5042, 836, 5334, 5340, 507,1379, 1380, 1381, 5018, 5339, 5027, 1429,5030,5965,5050,204,1215) and 
		o.voided=0 and cn.voided=0 and e.voided=0
