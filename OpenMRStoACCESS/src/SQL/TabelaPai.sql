Select p.nid,nome.nome,idade.idade,vivo.vivo,doente.doente,doenca.doenca,codprofissao.codprofissao,resultadohiv.resultadohiv,emtarv.emtarv
From t_paciente p inner join encounter e on e.patient_id=p.patient_id
left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as nome
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1485 and o.voided=0 and e.voided=0
			) nome on nome.encounter_id=e.encounter_id
left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as idade
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1486 and o.voided=0 and e.voided=0
			) idade on idade.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as vivo
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1487 and o.voided=0 and e.voided=0		   
		   ) vivo on vivo.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as doente
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1488 and o.voided=0 and e.voided=0		   
		   ) doente on doente.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as doenca
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1489 and o.voided=0 and e.voided=0
			) doenca on doenca.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as codprofissao
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1490 and o.voided=0 and e.voided=0
			) codprofissao on codprofissao.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 703 then 'POSETIVO'
					when 664 then 'NEGATIVO'
                    when 1138 then 'INDETERMINADO'
                    when 1118 then 'NAO FEZ'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as resultadohiv
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1491 and o.voided=0 and e.voided=0		   
		   ) resultadohiv on resultadohiv.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as emtarv
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1492 and o.voided=0 and e.voided=0		   
		   ) emtarv on emtarv.encounter_id=e.encounter_id

where e.encounter_type in (7) and  e.voided=0 and nome.nome is not null and p.dataabertura between '2007-01-01' and '2011-12-30' ;