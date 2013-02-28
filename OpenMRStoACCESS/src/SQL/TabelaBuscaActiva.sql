SELECT distinct p.identifier as nid,o.value_datetime as datacomecoufaltar,dataentregaactivista.dataentregaactivista,pacientelocalizado.pacientelocalizado,
o.obs_datetime as datalocalizacao,codmotivoabandono.codmotivoabandono
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=p.patient_id and o.concept_id=2004 and e.encounter_id=o.encounter_id
left join (Select distinct o.person_id,e.encounter_id,o.value_datetime as dataentregaactivista
           from obs o inner join encounter e on o.person_id=e.patient_id and o.encounter_id=e.encounter_id
           where e.encounter_type=21 and o.concept_id=2173 and e.voided=0 and o.voided=0 ) dataentregaactivista on dataentregaactivista.person_id=p.patient_id
           and dataentregaactivista.encounter_id=e.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,if(o.value_coded=1065,'TRUE','FALSE') as pacientelocalizado
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=21 and o.concept_id=2003 and o.value_coded in (1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0 and o.voided=0) pacientelocalizado on pacientelocalizado.person_id=p.patient_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as codmotivoabandono
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=21 and o.concept_id in (2016,2017) and o.value_coded in (2005,2006,2007,2008,2009,2010,2011,2012,2013,2014,2015) 
           and o.voided=0 and cn.voided=0 and e.voided=0) codmotivoabandono on codmotivoabandono.person_id=p.patient_id
where p.identifier_type=2 and e.encounter_type=21  and p.voided=0 and e.voided=0 and o.voided=0




------------------------
Analise de Performance

SELECT	p.nid,
		e.encounter_datetime,
		datacomecoufaltar.datacomecoufaltar,
		dataentregaactivista.dataentregaactivista,
		pacientelocalizado.pacientelocalizado,
		pacientelocalizado.datalocalizacao,
		codmotivoabandono.codmotivoabandono,
		codreferencia.codreferencia,
		entregueconvite.entregueconvite,
		confidenteidentificado.confidenteidentificado,
		codinformacaodadapor.codinformacaodadapor,
		codservicorefere.codservicorefere
FROM	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id
		inner join
		(SELECT patient_id,date_add(max(encounter_datetime), interval 61 day) as datacomecoufaltar
			FROM
			(	SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on o.encounter_id=e.encounter_id 		
				WHERE 	encounter_type=18 and e.voided=0 and  
						o.concept_id=1255 and o.value_coded<>1708 and 
						encounter_datetime<='2011-12-31' and p.patient_id=402
				GROUP BY p.patient_id

				UNION

				SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p
						inner join encounter e on p.patient_id=e.patient_id		
				WHERE 	e.voided=0 and e.encounter_type in (9,6) and 
						e.encounter_datetime<='2011-12-31' and p.patient_id=402
				GROUP BY p.patient_id
			)	ultima_visita

		GROUP BY patient_id) datacomecoufaltar on datacomecoufaltar.patient_id=p.patient_id

		left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataentregaactivista
					FROM 	encounter e 
							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
					WHERE 	e.encounter_type=21 and o.concept_id=2173 and o.voided=0 and e.voided=0
				  ) dataentregaactivista on dataentregaactivista.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,
							e.encounter_id,
							if(o.value_coded=1065,o.obs_datetime,null) as datalocalizacao,
							case o.value_coded
								when 1065 then 'SIM'
								when 1066 then 'NAO'
							else null end as pacientelocalizado
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=2003 and o.voided=0 and e.voided=0		   
				  ) pacientelocalizado on pacientelocalizado.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 2005 then 'ESQUECEU A DATA'
								when 2006 then 'ESTA ACAMADO EM CASA'
								when 2007 then 'DISTANCIA/DINHEIRO TRANSPORTE'
								when 2008 then 'PROBLEMAS DE ALIMENTACAO'
								when 2009 then 'PROBLEMAS FAMILIARES'
								when 2010 then 'INSATISFACCAO COM SERVICO NO HDD'
								when 2011 then 'VIAJOU'
								when 2012 then 'DESMOTIVACAO'
								when 2013 then 'TRATAMENTO TRADICIONAL'
								when 2014 then 'TRABALHO'
								when 2015 then 'EFEITOS SECUNDARIOS ARV'
								when 2017 then 'OUTRO'
							else o.value_coded end as codmotivoabandono
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=2016 and o.voided=0 and e.voided=0		   
				  ) codmotivoabandono on codmotivoabandono.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1797 then 'Encaminhado para a US'
								when 1977 then 'Encaminhado para os grupos de apoio'
								when 5488 then 'Orientado sobre a toma correcta dos ARV'
								when 2159 then 'Familiar foi referido para a US'								
							else 'OUTRO' end as codreferencia
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=1272 and o.voided=0 and e.voided=0		   
				  ) codreferencia on codreferencia.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,							
							if(o.value_datetime is not null,'SIM','NAO') as entregueconvite
					FROM 	encounter e 
							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
					WHERE 	e.encounter_type=21 and o.concept_id=2179 and o.voided=0 and e.voided=0
				  ) entregueconvite on entregueconvite.encounter_id=e.encounter_id  
		left join (	SELECT 	o.person_id,
							e.encounter_id,							
							case o.value_coded
								when 1065 then 'SIM'
								when 1066 then 'NAO'
							else null end as confidenteidentificado
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=1739 and o.voided=0 and e.voided=0		   
				  ) confidenteidentificado on confidenteidentificado.encounter_id=e.encounter_id		
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 2034 then 'Vizinho'
								when 2033 then 'Confidente'
								when 2035 then 'Familiar'
								when 2036 then 'Secretário do Bairro'								
							else 'OUTRO' end as codinformacaodadapor
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=2037 and o.voided=0 and e.voided=0		   
				  ) codinformacaodadapor on codinformacaodadapor.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 2175 then 'TARV Adulto'
								when 2174 then 'TARV Pediatrico'
								when 1414 then 'PNCT'
								when 1598 then 'PTV'								
							else 'OUTRO' end as codservicorefere
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=21 and o.concept_id=2176 and o.voided=0 and e.voided=0		   
				  ) codservicorefere on codservicorefere.encounter_id=e.encounter_id
where e.encounter_type=21 and  e.voided=0;


-----------------------------------------------------


		(SELECT patient_id,date_add(max(encounter_datetime), interval 61 day) as datacomecoufaltar
		FROM
			(	SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on o.encounter_id=e.encounter_id 		
				WHERE 	encounter_type=18 and e.voided=0 and  
						o.concept_id=1255 and o.value_coded<>1708 and encounter_datetime<='2011-12-31'
				GROUP BY p.patient_id

				UNION

				SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p
						inner join encounter e on p.patient_id=e.patient_id		
				WHERE 	e.voided=0 and e.encounter_type in (9,6) and e.encounter_datetime<='2011-12-31'
				GROUP BY p.patient_id
			)	ultima_visita

		GROUP BY patient_id) datacomecoufaltar
		
================================================================================================================================

		(SELECT patient_id,date_add(max(encounter_datetime), interval 61 day) as datacomecoufaltar
		FROM
			(	SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on o.encounter_id=e.encounter_id 		
				WHERE 	encounter_type=18 and e.voided=0 and  
						o.concept_id=1255 and o.value_coded<>1708 and 
						encounter_datetime<'2011-12-31' and p.patient_id=402
				GROUP BY p.patient_id

				UNION

				SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime
				FROM 	t_paciente p
						inner join encounter e on p.patient_id=e.patient_id		
				WHERE 	e.voided=0 and e.encounter_type in (9,6) and 
						e.encounter_datetime<'2011-12-31' and p.patient_id=402
				GROUP BY p.patient_id
			)	ultima_visita

		GROUP BY patient_id) datacomecoufaltar


SELECT patient_id,encounter_id,encounter_datetime


=============================================================================================================================

String sqlSelect = "SELECT	p.nid, ";
sqlSelect += "		e.encounter_datetime, ";
sqlSelect += "		datacomecoufaltar.datacomecoufaltar, ";
sqlSelect += "		dataentregaactivista.dataentregaactivista, ";
sqlSelect += "		pacientelocalizado.pacientelocalizado, ";
sqlSelect += "		pacientelocalizado.datalocalizacao, ";
sqlSelect += "		codmotivoabandono.codmotivoabandono, ";
sqlSelect += "		codreferencia.codreferencia, ";
sqlSelect += "		entregueconvite.entregueconvite, ";
sqlSelect += "		confidenteidentificado.confidenteidentificado, ";
sqlSelect += "		codinformacaodadapor.codinformacaodadapor, ";
sqlSelect += "		codservicorefere.codservicorefere ";
sqlSelect += "FROM	t_paciente p  ";
sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id ";
sqlSelect += "		inner join ";
sqlSelect += "		(SELECT patient_id,date_add(max(encounter_datetime), interval 61 day) as datacomecoufaltar ";
sqlSelect += "			FROM ";
sqlSelect += "			(	SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime ";
sqlSelect += "				FROM 	t_paciente p  ";
sqlSelect += "						inner join encounter e on p.patient_id=e.patient_id ";
sqlSelect += "						inner join obs o on o.encounter_id=e.encounter_id  ";		
sqlSelect += "				WHERE 	encounter_type=18 and e.voided=0 and  "; 
sqlSelect += "						o.concept_id=1255 and o.value_coded<>1708 and  ";
sqlSelect += "						encounter_datetime<='2011-12-31' and p.patient_id=402 ";
sqlSelect += "				GROUP BY p.patient_id ";
sqlSelect += "				UNION ";
sqlSelect += "				SELECT 	p.patient_id,max(encounter_datetime) as encounter_datetime ";
sqlSelect += "				FROM 	t_paciente p ";
sqlSelect += "						inner join encounter e on p.patient_id=e.patient_id ";
sqlSelect += "				WHERE 	e.voided=0 and e.encounter_type in (9,6) and  ";
sqlSelect += "						e.encounter_datetime<='2011-12-31' and p.patient_id=402 ";
sqlSelect += "				GROUP BY p.patient_id ";
sqlSelect += "			)	ultima_visita ";
sqlSelect += "		GROUP BY patient_id) datacomecoufaltar on datacomecoufaltar.patient_id=p.patient_id ";
sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataentregaactivista ";
sqlSelect += "					FROM 	encounter e  ";
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2173 and o.voided=0 and e.voided=0 ";
sqlSelect += "				  ) dataentregaactivista on dataentregaactivista.encounter_id=e.encounter_id ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";
sqlSelect += "							if(o.value_coded=1065,o.obs_datetime,null) as datalocalizacao, ";
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 1065 then 'SIM' ";
sqlSelect += "								when 1066 then 'NAO' ";
sqlSelect += "							else null end as pacientelocalizado ";
sqlSelect += "					FROM 	encounter e  ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2003 and o.voided=0 and e.voided=0	 ";	   
sqlSelect += "				  ) pacientelocalizado on pacientelocalizado.encounter_id=e.encounter_id ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 2005 then 'ESQUECEU A DATA' ";
sqlSelect += "								when 2006 then 'ESTA ACAMADO EM CASA' ";
sqlSelect += "								when 2007 then 'DISTANCIA/DINHEIRO TRANSPORTE' ";
sqlSelect += "								when 2008 then 'PROBLEMAS DE ALIMENTACAO' ";
sqlSelect += "								when 2009 then 'PROBLEMAS FAMILIARES' ";
sqlSelect += "								when 2010 then 'INSATISFACCAO COM SERVICO NO HDD' ";
sqlSelect += "								when 2011 then 'VIAJOU' ";
sqlSelect += "								when 2012 then 'DESMOTIVACAO' ";
sqlSelect += "								when 2013 then 'TRATAMENTO TRADICIONAL' ";
sqlSelect += "								when 2014 then 'TRABALHO' ";
sqlSelect += "								when 2015 then 'EFEITOS SECUNDARIOS ARV' ";
sqlSelect += "								when 2017 then 'OUTRO' ";
sqlSelect += "							else o.value_coded end as codmotivoabandono ";
sqlSelect += "					FROM 	encounter e 	 ";				
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2016 and o.voided=0 and e.voided=0	 ";	   
sqlSelect += "				  ) codmotivoabandono on codmotivoabandono.encounter_id=e.encounter_id ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 1797 then 'Encaminhado para a US' ";
sqlSelect += "								when 1977 then 'Encaminhado para os grupos de apoio' ";
sqlSelect += "								when 5488 then 'Orientado sobre a toma correcta dos ARV' ";
sqlSelect += "								when 2159 then 'Familiar foi referido para a US' ";								
sqlSelect += "							else 'OUTRO' end as codreferencia ";
sqlSelect += "					FROM 	encounter e  ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=1272 and o.voided=0 and e.voided=0 ";		   
sqlSelect += "				  ) codreferencia on codreferencia.encounter_id=e.encounter_id ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";							
sqlSelect += "							if(o.value_datetime is not null,'SIM','NAO') as entregueconvite ";
sqlSelect += "					FROM 	encounter e  ";
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2179 and o.voided=0 and e.voided=0 ";
sqlSelect += "				  ) entregueconvite on entregueconvite.encounter_id=e.encounter_id   ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id,	 ";						
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 1065 then 'SIM' ";
sqlSelect += "								when 1066 then 'NAO' ";
sqlSelect += "							else null end as confidenteidentificado ";
sqlSelect += "					FROM 	encounter e  ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=1739 and o.voided=0 and e.voided=0 ";		   
sqlSelect += "				  ) confidenteidentificado on confidenteidentificado.encounter_id=e.encounter_id ";		
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 2034 then 'Vizinho' ";
sqlSelect += "								when 2033 then 'Confidente' ";
sqlSelect += "								when 2035 then 'Familiar' ";
sqlSelect += "								when 2036 then 'Secretário do Bairro' ";								
sqlSelect += "							else 'OUTRO' end as codinformacaodadapor ";
sqlSelect += "					FROM 	encounter e  ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2037 and o.voided=0 and e.voided=0	 ";	   
sqlSelect += "				  ) codinformacaodadapor on codinformacaodadapor.encounter_id=e.encounter_id ";
sqlSelect += "		left join (	SELECT 	o.person_id, ";
sqlSelect += "							e.encounter_id, ";
sqlSelect += "							case o.value_coded ";
sqlSelect += "								when 2175 then 'TARV Adulto' ";
sqlSelect += "								when 2174 then 'TARV Pediatrico' ";
sqlSelect += "								when 1414 then 'PNCT' ";
sqlSelect += "								when 1598 then 'PTV' ";								
sqlSelect += "							else 'OUTRO' end as codservicorefere ";
sqlSelect += "					FROM 	encounter e  ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2176 and o.voided=0 and e.voided=0 ";		   
sqlSelect += "				  ) codservicorefere on codservicorefere.encounter_id=e.encounter_id ";
sqlSelect += "WHERE e.encounter_type=21 and  e.voided=0 ";