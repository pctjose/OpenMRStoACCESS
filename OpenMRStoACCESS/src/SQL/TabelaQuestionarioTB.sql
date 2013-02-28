Select distinct codopcao.codopcao,if(codopcao.codopcao is not null,'TRUE','FALSE') as estadoopcao,p.nid,e.encounter_datetime
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

inner join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1760 then 'Tosse  por mais de 3 semanas ?'
					when 1761 then 'Tosse com sangue? '
                    when 1762 then 'Suores a noite por mais de 3 semanas? '
                    when 1763 then 'Febre por mais de 3 semanas?'
					when 1764 then 'Perdeu Peso (mais de 3 kg. no ultimo mês)'
                    when 1765 then 'Alguém em casa está tratando a TB?'
					else '' end as codopcao
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=20 and o.concept_id=1766 and o.voided=0 and e.voided=0		   
		   ) codopcao on codopcao.encounter_id=e.encounter_id

where e.encounter_type in (20) and  e.voided=0 and p.nid is not null and p.dataabertura between '2007-01-01' and '2011-12-31';