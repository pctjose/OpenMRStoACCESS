SELECT distinct coddiagnostico.coddiagnostico,p.identifier as nid,coddiagnostico.value_datetime as Data
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
left join (SELECT distinct o.person_id,o.encounter_id,o.value_coded,o.value_datetime,cn.name as coddiagnostico
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in(6,9) and o.concept_id in (1406,1649) and o.value_coded in (3,123,68) and o.voided=0 and cn.voided=0 and e.voided=0) coddiagnostico on coddiagnostico.person_id=p.patient_id
           and coddiagnostico.encounter_id=e.encounter_id
where p.identifier_type=2 and e.encounter_type in (6,9)  and p.voided=0 and o.voided=0 and e.voided=0



------------------------
Analise de Performance



Select coddiagnostico.coddiagnostico,p.nid,coddiagnostico.data
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

left join (SELECT distinct o.person_id,o.encounter_id,o.value_coded,o.value_datetime as data,cn.name as coddiagnostico
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type in(6,9) and o.concept_id in (1406,1649) and o.value_coded in (3,123,68,2142,1218,2141) and o.voided=0 and cn.voided=0 and e.voided=0) coddiagnostico on coddiagnostico.person_id=p.patient_id
           and coddiagnostico.encounter_id=e.encounter_id

where e.encounter_type in (6,9) and  e.voided=0;