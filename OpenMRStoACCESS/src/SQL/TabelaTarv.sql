select distinct a.person_id as nid, datatarv, a.value_coded as codregime, b.value_coded as tipotarv, c.value_datetime as dataproxima, QtdComp, QtdSaldo
from (select person_id, encounter_datetime as datatarv, value_coded
from obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=1088 and o.voided=0 and e.voided=0 and encounter_type=18)a left join 

(select person_id, value_coded
from
obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=1255 and o.voided=0 and e.voided=0 
group by person_id) b on a.person_id=b.person_id

left join 

(select person_id, value_datetime
from
obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=5096 and o.voided=0 and e.voided=0 
group by person_id) c on a.person_id=c.person_id
left join


(select person_id, value_numeric as QtdComp
from
obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=1715 and o.voided=0 and e.voided=0 
group by person_id) d on a.person_id=d.person_id left join


(select person_id, value_numeric as QtdSaldo
from
obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=1713 and o.voided=0 and e.voided=0 
group by person_id) e on b.person_id=e.person_id
left join

(select person_id, value_datetime
from
obs o inner join encounter e on o.encounter_id=e.encounter_id and o.person_id=e.patient_id
            where o.concept_id=1190 and o.voided=0 and e.voided=0 
group by person_id) f on b.person_id=f.person_id
order by nid, datatarv



Select	p.patient_id,
		e.encounter_id,
		p.nid,
		e.encounter_datetime as datatarv,
		regime.codRegime,		
		case o.value_coded
			when 1256 then 'INICIO'
			when 1257 then 'MANTER'
			when 1259 then 'ALTERAR'
			when 1369 then 'TRANSFERIDO DE'
			when 1705 then 'REINICIAR'
			when 1708 then 'SAIDA'
			else 'OUTRO' end as tipotarv,
			proxima.dataproxima,
			aviada.qtdComp,
			saldo.qtdSaldo,
			outro.dataoutro
from	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id
		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
		left join	(
						select e.encounter_id,
								case o.value_coded
								when 792 then 'D4T+3TC+NVP'
								when 6110 then 'D4T+3TC+NVP'
								when 1827 then 'D4T+3TC+EFV'
								when 6103 then 'D4T+3TC+LPV'
								when 1651 then 'AZT+3TC+NVP'
								when 1703 then 'AZT+3TC+EFV'
								when 1702 then 'AZT+3TC+NFV'
								when 6100 then 'AZT+3TC+LPV'
								when 817 then 'AZT+3TC+ABC'
								when 6104 then 'ABC+3TC+EFV'
								when 6106 then 'ABC+3TC+LPV'
								when 6105 then 'ABC+3TC+NVP'
								when 6243 then 'TDF+3TC+NVP'
								when 6244 then 'AZT+3TC+RTV'
								when 1700 then 'AZT+DDI+NFV'
								when 633 then 'EFV'
								when 1701 then 'ABC+DDI+NFV'																
								else 'OUTRO' end as codRegime
						from	encounter e inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and o.concept_id=1088 and e.encounter_type=18
					) regime on regime.encounter_id=e.encounter_id
		left join	(
						select	e.encounter_id,
								o.value_datetime as dataproxima
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=5096
					) proxima on e.encounter_id=proxima.encounter_id
		left join	(
						select	e.encounter_id,
								o.value_numeric as qtdComp
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1715
					) aviada on e.encounter_id=aviada.encounter_id
		left join	(
						select	e.encounter_id,
								o.value_numeric as qtdSaldo
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1713
					) saldo on e.encounter_id=saldo.encounter_id
		left join	(
						select	e.encounter_id,
								o.value_datetime as dataoutro
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=18 and o.concept_id=1190
					) outro on e.encounter_id=outro.encounter_id

where	e.voided=0 and e.encounter_type=18 and o.voided=0 and o.concept_id=1255 and o.value_coded<>1708 and 
		e.encounter_datetime between '2007-01-01' and '2011-12-31' and regime.codregime is not null;