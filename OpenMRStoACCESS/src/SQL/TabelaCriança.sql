SELECT distinct p.identifier as nid,tipoparto.tipoparto,local.local,termo.termo,pesonascimento.pesonascimento,exposicaotarvmae.exposicaotarvmae,
       exposicaotarvnascenca.exposicaotarvnascenca,patologianeonatal.patologianeonatal,injeccoes.injeccoes,escarificacoes.escarificacoes,
       extracoesdentarias.extracoesdentarias,aleitamentomaterno.aleitamentomaterno,aleitamentoexclusivo.aleitamentoexclusivo,idadedesmame.idadedesmame,
       pavcompleto.pavcompleto,idadecronologica.idadecronologica,bailey.bailey
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as tipoparto
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=5630 and o.value_coded in (1170,1171) and o.voided=0 and cn.voided=0 and e.voided=0) tipoparto on tipoparto.person_id=p.patient_id
           and tipoparto.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_text as local
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=1505 and e.voided=0 and o.voided=0) local on local.person_id=p.patient_id
          and local.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as termo
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1500 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) termo on termo.person_id=p.patient_id
           and termo.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_numeric as pesonascimento
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=5916 and e.voided=0 and o.voided=0) pesonascimento on pesonascimento.person_id=p.patient_id
          and pesonascimento.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as exposicaotarvmae
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1501 and o.value_coded in (1065,1066,1457) and o.voided=0 and cn.voided=0 and e.voided=0) exposicaotarvmae on exposicaotarvmae.person_id=p.patient_id
           and exposicaotarvmae.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as exposicaotarvnascenca
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1502 and o.value_coded in (1065,1066,1457) and o.voided=0 and cn.voided=0 and e.voided=0) exposicaotarvnascenca on exposicaotarvmae.person_id=p.patient_id
           and exposicaotarvnascenca.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_text as patologianeonatal
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=1506 and e.voided=0 and o.voided=0) patologianeonatal on patologianeonatal.person_id=p.patient_id
          and patologianeonatal.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as injeccoes
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1507 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) injeccoes on injeccoes.person_id=p.patient_id
           and injeccoes.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as escarificacoes
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1509 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) escarificacoes on escarificacoes.person_id=p.patient_id
           and escarificacoes.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as extracoesdentarias
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1508 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) extracoesdentarias on extracoesdentarias.person_id=p.patient_id
           and extracoesdentarias.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as aleitamentomaterno
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=6061 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) aleitamentomaterno on aleitamentomaterno.person_id=p.patient_id
           and aleitamentomaterno.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as aleitamentoexclusivo
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1613 and o.value_coded in (5526,1066) and o.voided=0 and cn.voided=0 and e.voided=0) aleitamentoexclusivo on aleitamentoexclusivo.person_id=p.patient_id
           and aleitamentoexclusivo.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_numeric as idadedesmame
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=1510 and e.voided=0 and o.voided=0) idadedesmame on idadedesmame.person_id=p.patient_id
          and idadedesmame.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as pavcompleto
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=7 and o.concept_id=1511 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) pavcompleto on pavcompleto.person_id=p.patient_id
           and pavcompleto.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_numeric as idadecronologica
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=1512 and e.voided=0 and o.voided=0) idadecronologica on idadecronologica.person_id=p.patient_id
          and idadecronologica.encounter_id=e.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_numeric as bailey
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=7 and o.concept_id=1514 and e.voided=0 and o.voided=0) bailey on bailey.person_id=p.patient_id
          and bailey.encounter_id=e.encounter_id
where p.identifier_type=2 and e.encounter_type=7 and p.voided=0 and o.voided=0 and e.voided=0



------------------------
Analise de Performance


Select distinct p.nid,tipoparto.tipoparto,local.local,termo.termo,pesonascimento.pesonascimento,exposicaotarvmae.exposicaotarvmae,exposicaotarvnascenca.exposicaotarvnascenca,
       patologianeonatal.patologianeonatal,injeccoes.injeccoes,escarificacoes.escarificacoes,extracoesdentarias.extracoesdentarias,aleitamentomaterno.aleitamentomaterno,
       aleitamentoexclusivo.aleitamentoexclusivo,idadedesmame.idadedesmame,pavcompleto.pavcompleto,idadecronologica.idadecronologica,bailey.bailey

From t_paciente p inner join encounter e on e.patient_id=p.patient_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1170 then 'VAGINAL'
					when 1171 then 'CESARIANA'
					else '' end as tipoparto
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=5630 and o.voided=0 and e.voided=0		   
		   ) tipoparto on tipoparto.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as local
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1505 and o.voided=0 and e.voided=0
			) local on local.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as termo
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1500 and o.voided=0 and e.voided=0		   
		   ) termo on termo.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as pesonascimento
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=5916 and o.voided=0 and e.voided=0
			) pesonascimento on pesonascimento.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as exposicaotarvmae
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1501 and o.voided=0 and e.voided=0		   
		   ) exposicaotarvmae on exposicaotarvmae.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as exposicaotarvnascenca
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1502 and o.voided=0 and e.voided=0		   
		   ) exposicaotarvnascenca on exposicaotarvnascenca.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as patologianeonatal
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1506 and o.voided=0 and e.voided=0
			) patologianeonatal on patologianeonatal.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as injeccoes
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1507 and o.voided=0 and e.voided=0		   
		   ) injeccoes on injeccoes.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as escarificacoes
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1509 and o.voided=0 and e.voided=0		   
		   ) escarificacoes on escarificacoes.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as extracoesdentarias
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1508 and o.voided=0 and e.voided=0		   
		   ) extracoesdentarias on extracoesdentarias.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as aleitamentomaterno
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=6061 and o.voided=0 and e.voided=0		   
		   ) aleitamentomaterno on aleitamentomaterno.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 5526 then 'SIM'
					when 1066 then 'NAO'
					else '' end as aleitamentoexclusivo
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1613 and o.voided=0 and e.voided=0		   
		   ) aleitamentoexclusivo on aleitamentoexclusivo.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as idadedesmame
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id=1510 and o.voided=0 and e.voided=0
			) idadedesmame on idadedesmame.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
					else '' end as pavcompleto
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=7 and o.concept_id=1511 and o.voided=0 and e.voided=0		   
		   ) pavcompleto on pavcompleto.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as idadecronologica
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id in (1512,1513) and o.voided=0 and e.voided=0
			) idadecronologica on idadecronologica.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as bailey
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type in (7) and o.concept_id in (1514,1515) and o.voided=0 and e.voided=0
			) bailey on bailey.encounter_id=e.encounter_id

where e.encounter_type=7 and  e.voided=0;