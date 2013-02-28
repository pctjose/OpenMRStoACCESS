Select distinct p.nid,tarv.tarv
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

inner join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 631 then 'NVP'
					when 797 then 'AZT'
          when 792 then 'D4T+3TC+NVP'
          when 1800 then 'TARV'
					when 1801 then 'AZT+NVP'
          when 630 then 'AZT+3TC'
          when 628 then '3TC'
					else '' end as tarv
			FROM 	encounter e
					inner join obs o on e.encounter_id=o.encounter_id
           WHERE 	e.encounter_type=7 and o.concept_id=1503 and o.voided=0 and e.voided=0
		   ) tarv on tarv.encounter_id=e.encounter_id

where e.encounter_type in (7) and  e.voided=0;


------------------------
Analise de Performance


Select p.nid,tarv.tarv
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as tarv
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1503 and o.value_coded in (631,797,792,1800,1801,630,628) and o.voided=0 and cn.voided=0 and e.voided=0) 
           tarv on tarv.person_id=p.patient_id and tarv.encounter_id=e.encounter_id

where e.encounter_type in (7) and  e.voided=0;