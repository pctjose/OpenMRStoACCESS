SELECT distinct p.identifier as nid,criteriosmedicos.criteriosmedicos,conceitos.conceitos,interessado.interessado,confidente.confidente,apareceregularmente.apareceregularmente,
                 riscopobreaderencia.riscopobreaderencia,regimetratamento.regimetratamento,prontotarv.prontotarv,prontotarv.datapronto,o.value_text as obs
FROM patient_identifier p inner join encounter e on e.patient_id=p.patient_id
left join obs o on o.person_id=e.patient_id and e.encounter_id=o.encounter_id
left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as criteriosmedicos
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1248 and o.value_coded in(1065,1066) and o.voided=0 and e.voided=0 and o.voided=0 and cn.voided=0 and e.voided=0) criteriosmedicos on criteriosmedicos.person_id=p.patient_id
           and criteriosmedicos.encounter_id=e.encounter_id and o.encounter_id=o.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as conceitos
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1729 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) conceitos on conceitos.person_id=p.patient_id
           and conceitos.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as interessado
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1736 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) interessado on interessado.person_id=p.patient_id
           and interessado.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as confidente
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1728 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) confidente on confidente.person_id=p.patient_id
           and confidente.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as apareceregularmente
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1743 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) apareceregularmente on apareceregularmente.person_id=p.patient_id
           and apareceregularmente.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as riscopobreaderencia
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1749 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) riscopobreaderencia on riscopobreaderencia.person_id=p.patient_id
           and riscopobreaderencia.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as regimetratamento
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1752 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) regimetratamento on regimetratamento.person_id=p.patient_id
           and regimetratamento.encounter_id=e.encounter_id

left join  (SELECT distinct o.person_id,o.encounter_id,cn.name as prontotarv,o.obs_datetime as datapronto
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
           where e.encounter_type=19 and o.concept_id=1756 and o.value_coded in(1065,1066) and o.voided=0 and cn.voided=0 and e.voided=0) prontotarv on prontotarv.person_id=p.patient_id
           and prontotarv.encounter_id=e.encounter_id

where p.identifier_type=2 and e.encounter_type=19 and o.concept_id=1757 and p.voided=0 and o.voided=0 and e.voided=0



------------------------
Analise de Performance

String  sqlSelect = " SELECT	p.patient_id,";
sqlSelect += "		e.encounter_id,";
sqlSelect += "		p.nid,";
sqlSelect += "		e.encounter_datetime,";
sqlSelect += "		sessao.nrsessao,";
sqlSelect += "		tipo.tipoactividade,";
sqlSelect += "		confidente.apresentouconfidente";
sqlSelect += " FROM	t_paciente p";
sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
sqlSelect += "		left join	(	SELECT 	o.person_id,";
sqlSelect += "								o.encounter_id,";
sqlSelect += "								o.value_numeric as nrsessao";
sqlSelect += "						FROM 	encounter e ";
sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1724 and o.voided=0 and e.voided=0";
sqlSelect += "					) sessao on sessao.encounter_id=e.encounter_id";
sqlSelect += "		left join	(	SELECT 	o.person_id,";
sqlSelect += "								o.encounter_id,";
sqlSelect += "								case o.value_coded";
sqlSelect += "									when 1725 then 'GRUPO'";
sqlSelect += "									when 1726 then 'INDIVIDUAL'";
sqlSelect += "								end as tipoactividade";
sqlSelect += "						FROM 	encounter e ";
sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1727 and o.voided=0 and e.voided=0";
sqlSelect += "					) tipo on tipo.encounter_id=e.encounter_id";
sqlSelect += "		left join	(	SELECT 	o.person_id,";
sqlSelect += "								o.encounter_id,";
sqlSelect += "								case o.value_coded";
sqlSelect += "									when 1065 then true";
sqlSelect += "									when 1066 then false";
sqlSelect += "								end as apresentouconfidente";
sqlSelect += "						FROM 	encounter e ";
sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id";
sqlSelect += "						WHERE 	e.encounter_type in (19,29) and o.concept_id= 1728 and o.voided=0 and e.voided=0";
sqlSelect += "					) confidente on confidente.encounter_id=e.encounter_id";
sqlSelect += " WHERE	p.nid is not null and ";
sqlSelect += "			p.datanasc is not null and ";
sqlSelect += "			e.voided=0 and e.encounter_type in (19,29)";


String  sqlSelect = " SELECT	p.patient_id,";
sqlSelect += "		e.encounter_id,";
sqlSelect += "		p.nid,";
sqlSelect += "		criteriosmedicos.criteriosmedicos,";
sqlSelect += "		conceitos.conceitos,";
sqlSelect += "		interessado.interessado,";
sqlSelect += "		confidente.confidente,";
sqlSelect += "		apareceregularmente.apareceregularmente,";
sqlSelect += "		riscopobreaderencia.riscopobreaderencia,";
sqlSelect += "		regimetratamento.regimetratamento,";
sqlSelect += "		prontotarv.prontotarv,";
sqlSelect += "		prontotarv.datapronto,";
sqlSelect += "		obs.obs";
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as criteriosmedicos";
sqlSelect += "					FROM 	encounter e 	";				
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1248 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) criteriosmedicos on criteriosmedicos.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as conceitos";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1729 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) conceitos on conceitos.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as interessado";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1736 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) interessado on interessado.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as confidente";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1739 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) confidente on confidente.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as apareceregularmente";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id"; 
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1743 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) apareceregularmente on apareceregularmente.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as riscopobreaderencia";
sqlSelect += "					FROM 	encounter e 	";				
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1749 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) riscopobreaderencia on riscopobreaderencia.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as regimetratamento";
sqlSelect += "					FROM 	encounter e 	";				
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1752 and o.voided=0 and e.voided=0	";	   
sqlSelect += "				  ) regimetratamento on regimetratamento.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then true";
sqlSelect += "								when 1066 then false";
sqlSelect += "							end as prontotarv,";
sqlSelect += "							case o.value_coded";
sqlSelect += "								when 1065 then o.obs_datetime";
sqlSelect += "							else null end as datapronto";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1756 and o.voided=0 and e.voided=0";		   
sqlSelect += "				  ) prontotarv on prontotarv.encounter_id=e.encounter_id";
sqlSelect += "		left join (	SELECT 	o.person_id,";
sqlSelect += "							e.encounter_id,";
sqlSelect += "							o.value_text as obs";
sqlSelect += "					FROM 	encounter e ";					
sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "					WHERE 	e.encounter_type in (19,29) and o.concept_id=1757 and o.voided=0 and e.voided=0";		   
sqlSelect += "				  ) obs on obs.encounter_id=e.encounter_id  ";
sqlSelect += " WHERE	e.encounter_type in (19,29) and  e.voided=0 and";
sqlSelect += "			p.nid is not null and ";
sqlSelect += "			p.datanasc is not null";
		
-----------------------------------------------------------------------------		
SELECT	p.patient_id,
		e.encounter_id,
		p.nid,
		criteriosmedicos.criteriosmedicos,
		conceitos.conceitos,
		interessado.interessado,
		confidente.confidente,
		apareceregularmente.apareceregularmente,
		riscopobreaderencia.riscopobreaderencia,
		regimetratamento.regimetratamento,
		prontotarv.prontotarv,
		prontotarv.datapronto,
		obs.obs
FROM	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as criteriosmedicos
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1248 and o.voided=0 and e.voided=0		   
				  ) criteriosmedicos on criteriosmedicos.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as conceitos
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1729 and o.voided=0 and e.voided=0		   
				  ) conceitos on conceitos.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as interessado
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1736 and o.voided=0 and e.voided=0		   
				  ) interessado on interessado.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as confidente
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1739 and o.voided=0 and e.voided=0		   
				  ) confidente on confidente.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as apareceregularmente
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1743 and o.voided=0 and e.voided=0		   
				  ) apareceregularmente on apareceregularmente.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as riscopobreaderencia
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1749 and o.voided=0 and e.voided=0		   
				  ) riscopobreaderencia on riscopobreaderencia.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as regimetratamento
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1752 and o.voided=0 and e.voided=0		   
				  ) regimetratamento on regimetratamento.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							end as prontotarv,
							case o.value_coded
								when 1065 then o.obs_datetime
							else null end as datapronto
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1756 and o.voided=0 and e.voided=0		   
				  ) prontotarv on prontotarv.encounter_id=e.encounter_id
		left join (	SELECT 	o.person_id,
							e.encounter_id,
							o.value_text as obs
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type in (19,29) and o.concept_id=1757 and o.voided=0 and e.voided=0		   
				  ) obs on obs.encounter_id=e.encounter_id  

WHERE	e.encounter_type in (19,29) and  e.voided=0 and
		p.nid is not null and 
		p.datanasc is not null;