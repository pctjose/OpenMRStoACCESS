SELECT distinct p.identifier as nid,if(YEAR(e.encounter_datetime)-YEAR(pr.birthdate)<2,PERIOD_DIFF(DATE_FORMAT(e.encounter_datetime,'%Y%m'),DATE_FORMAT(pr.birthdate,'%Y%m')),YEAR(e.encounter_datetime)-YEAR(pr.birthdate)) idade,
                estadohiv.estadohiv,e.encounter_datetime as dataseguimento,estadiooms.estadiooms,dataproximaconsulta.dataproximaconsulta,Gravidez.Gravidez
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
inner join person pr on pr.person_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
left join (SELECT distinct o.person_id,o.encounter_id,cn.name as estadohiv
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=9 and o.concept_id=1040 and o.value_coded in (703,664,1138) and o.voided=0 and cn.voided=0 and e.voided=0) estadohiv on estadohiv.person_id=p.patient_id
           and estadohiv.encounter_id=e.encounter_id
left join (SELECT distinct o.person_id,o.encounter_id,cn.name as estadiooms
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in (6,9) and o.concept_id in (5356) and o.value_coded in (1207,1206,1205,1204) and o.voided=0 and cn.voided=0 and e.voided=0) estadiooms on estadiooms.person_id=p.patient_id
           and estadiooms.encounter_id=e.encounter_id
left join (SELECT distinct o.person_id,o.encounter_id,o.value_datetime as dataproximaconsulta
           FROM obs o inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in (6,9) and o.concept_id in (1410) and o.voided=0 and e.voided=0) dataproximaconsulta on dataproximaconsulta.person_id=p.patient_id
           and dataproximaconsulta.encounter_id=e.encounter_id
left join (SELECT distinct o.person_id,o.encounter_id,o.value_numeric as Gravidez
           FROM obs o inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in (6) and o.concept_id in (5992) and o.voided=0 and e.voided=0) Gravidez on Gravidez.person_id=p.patient_id
           and Gravidez.encounter_id=e.encounter_id
where p.identifier_type=2  and e.encounter_type in (6,9) and p.voided=0 and o.voided=0 and e.voided=0


------------------------
Analise de Performance

SELECT  p.patient_id,
		e.encounter_id,
		p.nid,
		YEAR(e.encounter_datetime)-YEAR(p.datanasc) as idade,
		if(YEAR(e.encounter_datetime)-YEAR(p.datanasc)<2,PERIOD_DIFF(DATE_FORMAT(e.encounter_datetime,'%Y%m'),DATE_FORMAT(p.datanasc,'%Y%m')),null) meses,
        estadohiv.estadohiv,
        e.encounter_datetime as dataseguimento,
        estadiooms.estadiooms,
        dataproximaconsulta.dataproximaconsulta,
        Gravidez.Gravidez,
        outrodiagnostico.diagnostico
FROM 	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id
		left join	(	SELECT 	o.person_id,e.encounter_id,
								case o.value_coded
								when 703 then 'Positivo'
								when 664 then 'Negativo'
								when 1138 then 'Indeterminado'
								else 'OUTRO' end as estadohiv
						FROM 	encounter e 					
								inner join obs o on e.encounter_id=o.encounter_id 
						WHERE 	e.encounter_type=9 and o.concept_id=1040 and o.voided=0 and e.voided=0		   
					) estadohiv on estadohiv.encounter_id=e.encounter_id
		left join	(	SELECT  o.person_id,e.encounter_id,
								case o.value_coded
								when 1204 then 'I'
								when 1205 then 'II'
								when 1206 then 'III'
								when 1207 then 'IV'
								else 'OUTRO' end as estadiooms
						FROM 	encounter e 					
								inner join obs o on e.encounter_id=o.encounter_id
						WHERE 	e.encounter_type in (6,9) and o.concept_id=5356 and o.voided=0 and e.voided=0
					) estadiooms on estadiooms.encounter_id=e.encounter_id
		left join	(	SELECT 	o.person_id,
								e.encounter_id,
								o.value_datetime as dataproximaconsulta
						FROM 	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						WHERE 	e.encounter_type in (6,9) and o.concept_id=1410 and o.voided=0 and e.voided=0
					) dataproximaconsulta on dataproximaconsulta.encounter_id=e.encounter_id
		left join	(	SELECT 	o.person_id,
								o.encounter_id,
								o.value_numeric as Gravidez
						FROM 	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						WHERE 	e.encounter_type=6 and o.concept_id= 5992 and o.voided=0 and e.voided=0
					) Gravidez on Gravidez.encounter_id=e.encounter_id
		left join	(	SELECT 	o.person_id,
								o.encounter_id,
								o.value_text as diagnostico
						FROM 	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						WHERE 	e.encounter_type in (6,9) and o.concept_id= 1649 and o.voided=0 and e.voided=0
					) outrodiagnostico on outrodiagnostico.encounter_id=e.encounter_id
WHERE	e.encounter_type in (6,9) and  
		e.voided=0 and 
		p.nid is not null and 
		p.dataabertura is not null and 
		p.datanasc is not null and 
		p.dataabertura between '2007-01-01' and '2011-12-31' and 
		e.encounter_datetime between '2007-01-01' and '2011-12-31';