SELECT distinct cn.name as codtratamento,p.identifier as nid,o.obs_datetime as data
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
where p.identifier_type=2 and e.encounter_type in (6,9) and o.concept_id=1719 and o.value_coded in (1535,784,496,269,329,265,916,272,100,656,266,87,5622,917,732,912,461,88,786,920,
257,928,913,960,268,941,931,740,765,89,920,921,436,918,919,237,751,767,243,256,450,447,766,5829,95,247,446,99,926,250,1243,238,358,244,429,745,753,922,748,
755,735,791,754,292,734,356,763,254,759,777,775,798,738,750,768,756,90,270,752,1242,409,769,1108,799,733,731,746,737,773,736,742,779,739,744,743,741,1131) 
and p.voided=0 and o.voided=0 and e.voided=0 and cn.voided=0



------------------------
Analise de Performance



Select codtratamento.codtratamento,p.nid,codtratamento.data
From t_paciente p inner join encounter e on e.patient_id=p.patient_id
left join (	SELECT 	o.person_id,e.encounter_id,cn.name as codtratamento,o.obs_datetime as data
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
                    left join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
			WHERE 	e.encounter_type in (6,9) and o.concept_id=1719 and o.value_coded in (1535,784,496,269,329,265,916,272,100,656,266,87,5622,917,732,912,461,88,786,920,
              257,928,913,960,268,941,931,740,765,89,920,921,436,918,919,237,751,767,243,256,450,447,766,5829,95,247,446,99,926,250,1243,238,358,244,429,745,753,922,748,
              755,735,791,754,292,734,356,763,254,759,777,775,798,738,750,768,756,90,270,752,1242,409,769,1108,799,733,731,746,737,773,736,742,779,739,744,743,741,1131)
              and o.voided=0 and e.voided=0
			) codtratamento on codtratamento.encounter_id=e.encounter_id

where e.encounter_type in (6,9) and  e.voided=0;


SELECT	cn.name as codtratamento,
		p.nid,
		o.obs_datetime as data
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED'           
WHERE	e.encounter_type in (6,9) and o.concept_id=1719 and 
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null;
