SELECT distinct cn.name as estadiooms,codigoio.codigoio,p.identifier as nid,codigoio.data
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
     inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as codigoio,o.obs_datetime as data
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in (6,9) and o.concept_id in(1564,1565,1566,1569,1558,1561,1566,1066) 
           and o.value_coded in (5327,5329,5012,5332,5330,298,5027,5333,5337,5018,1567,5339,5334,5338,882,1570,5041,5035,5043,507,5046,5340,1568,823,5033,5355,5344,5034,5042,5345,
           5328,5332,1296,5030,42,1215,825,2065,1210,5012,1212,2064,836,298,5018,5338,1218,882,507,1216,5350,5044,5025,1844,5345,5050) 
           and o.voided=0 and cn.voided=0 and e.voided=0) codigoio on codigoio.person_id=p.patient_id
           and codigoio.encounter_id=e.encounter_id
where p.identifier_type=2 and e.encounter_type in (6,9) and o.concept_id in (5356) and o.value_coded in (1207,1206,1205,1204) and p.voided=0 and o.voided=0 and e.voided=0




------------------------
Analise de Performance



Select estadiooms.estadiooms,codigoio.codigoio,p.nid,codigoio.data
From t_paciente p inner join encounter e on e.patient_id=p.patient_id
left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1204 then 'I'
					when 1205 then 'II'
                    when 1206 then 'III'
                    when 1207 then 'IV'
					else '' end as estadiooms
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type in (6,9) and o.concept_id=5356 and o.voided=0 and e.voided=0		   
		   ) estadiooms on estadiooms.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as codigoio,o.obs_datetime as data
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in (6,9) and o.concept_id in(1564,1565,1566,1569,1558,1561,1566,1066) 
           and o.value_coded in (5327,5329,5012,5332,5330,298,5027,5333,5337,5018,1567,5339,5334,5338,882,1570,5041,5035,5043,507,5046,5340,1568,823,5033,5355,5344,5034,5042,5345,
           5328,5332,1296,5030,42,1215,825,2065,1210,5012,1212,2064,836,298,5018,5338,1218,882,507,1216,5350,5044,5025,1844,5345,5050) 
           and o.voided=0 and cn.voided=0 and e.voided=0) codigoio on
         codigoio.encounter_id=e.encounter_id

where e.encounter_type in (6,9) and  e.voided=0;


SELECT	p.patient_id,
		p.nid,
		e.encounter_id,
		case	o.concept_id
				when 1564 then 'I'
				when 1565 then 'II'
				when 1566 then 'III'
				when 1569 then 'IV'
				when 1558 then 'I'
				when 1561 then 'II'
				when 1562 then 'III'
				when 2066 then 'IV'
				end as estadioms,
		cn.name as codigoio,
		o.obs_datetime as data
FROM	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
		inner join obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'           
WHERE	e.encounter_type in (6,9) and o.concept_id in (1564,1565,1566,1569,1558,1561,1562,2066) and o.voided=0 and cn.voided=0 and e.voided=0



SELECT	case	o.concept_id
				when 1564 then 'I'
				when 1565 then 'II'
				when 1566 then 'III'
				when 1569 then 'IV'
				when 1558 then 'I'
				when 1561 then 'II'
				when 1562 then 'III'
				when 2066 then 'IV'
				end as estadioms,
		cn.name as codigoio,
		p.nid,
		o.obs_datetime as data
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED'           
WHERE	e.encounter_type in (6,9) and o.concept_id in (1564,1565,1566,1569,1558,1561,1562,2066) and 
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null;