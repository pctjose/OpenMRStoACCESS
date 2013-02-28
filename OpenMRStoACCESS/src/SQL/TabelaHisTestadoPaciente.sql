SELECT distinct p.identifier as nid,cn.name as codestado,o.obs_datetime as dataestado,destinopaciente.destinopaciente
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
     inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
left join (Select distinct o.person_id,e.encounter_id,o.value_text as destinopaciente
          from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
          where e.encounter_type in (18,6,9) and o.concept_id=2059 and e.voided=0 and o.voided=0) destinopaciente on destinopaciente.person_id=p.patient_id
          and destinopaciente.encounter_id=e.encounter_id
where p.identifier_type=2 and e.encounter_type in (18,6,9) and o.concept_id in (1708,6138) and o.value_coded in (1707,1706,1366,1704,1709,5622) and p.voided=0 and o.voided=0 and e.voided=0


select	p.patient_id,
		e.encounter_id,
		p.nid,
		case o.value_coded
		when 1707 then 'ABANDONO'
		when 1706 then 'TRANSFERIDO PARA'
		when 1366 then 'OBITO'
		when 1704 then 'HIV NEGATIVO'
		when 1709 then 'SUSPENSO'
		else 'OUTRO' end as codestado,
		e.encounter_datetime as dataestado,
		destino.destinopaciente
from	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
		inner join obs o on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
		left join	(
						select	e.encounter_id,
								o.value_text as destinopaciente
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=2059
					) destino on e.encounter_id=destino.encounter_id
where	e.encounter_type in (18,6,9) and o.concept_id in (1708,6138) and o.voided=0 and e.voided=0 and p.nid is not null and
		p.dataabertura between '2007-01-01' and '2011-12-31' and e.encounter_datetime between '2007-01-01' and '2011-12-31'
		


select	p.patient_id,
		e.encounter_id,
		p.nid,
		case o.value_coded
		when 1707 then 'ABANDONO'
		when 1706 then 'TRANSFERIDO PARA'
		when 1366 then 'OBITO'
		when 1704 then 'HIV NEGATIVO'
		when 1709 then 'SUSPENSO'
		else 'OUTRO' end as codestado,
		e.encounter_datetime as dataestado,
		destino.destinopaciente
from	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
		inner join obs o on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
		left join	(
						select	e.encounter_id,
								o.value_text as destinopaciente
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=2059
					) destino on e.encounter_id=destino.encounter_id
where e.encounter_type in (18,6,9) and o.concept_id in (1708,6138) and o.voided=0 and e.voided=0