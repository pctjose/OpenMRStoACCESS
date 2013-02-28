SELECT	p.patient_id,
		p.nid,
		pa.value
FROM	t_paciente p
		inner join person_attribute pa on p.patient_id=pa.person_id
WHERE	pa.person_attribute_type_id=9 and 
		pa.value is not null and 
		pa.value<>'' and 
		p.nid is not null and 
		p.datanasc is not null;

UNION 


SELECT 	o.person_id,
		p.nid,
		o.value_text as contacto
FROM 	t_paciente p 
		inner join encounter e on p.patient_id=e.patient_id
		inner join obs o on e.encounter_id=o.encounter_id
WHERE 	e.encounter_type in (5,7) and 
		o.concept_id=1611 and 
		o.voided=0 and e.voided=0 and 
		concat('',replace(value_text,' ','') * 1) = value_text and 
		p.nid is not null and 
		p.datanasc is not null;
		
		
--------------------------------------------------------------------------

String  sqlSelect = " SELECT	p.patient_id, ";
sqlSelect += "		p.nid, ";
sqlSelect += "		pa.value ";
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join person_attribute pa on p.patient_id=pa.person_id ";
sqlSelect += " WHERE	pa.person_attribute_type_id=9 and ";
sqlSelect += "		pa.value is not null and ";
sqlSelect += "		pa.value<>'' and ";
sqlSelect += "		p.nid is not null and ";
sqlSelect += "		p.datanasc is not null ";
sqlSelect += " UNION ";
sqlSelect += " SELECT 	o.person_id, ";
sqlSelect += "		p.nid, ";
sqlSelect += "		o.value_text as contacto ";
sqlSelect += " FROM 	t_paciente p ";
sqlSelect += "		inner join encounter e on p.patient_id=e.patient_id ";
sqlSelect += "		inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += " WHERE 	e.encounter_type in (5,7) and ";
sqlSelect += "		o.concept_id=1611 and ";
sqlSelect += "		o.voided=0 and e.voided=0 and ";
sqlSelect += "		concat('',replace(value_text,' ','') * 1) = value_text and ";
sqlSelect += "		p.nid is not null and ";
sqlSelect += "		p.datanasc is not null ";


SELECT	p.nid,
		nome.nome,
		apelido.apelido,
		contacto.contacto
FROM	t_paciente p
		left join
			(	SELECT 	p.patient_id,						
						o.value_text as contacto
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1611 and 
						o.voided=0 and e.voided=0 and concat('',replace(value_text,' ','') * 1) = value_text
			) contacto on contacto.patient_id=p.patient_id
		left join
			(	SELECT 	p.patient_id,						
						o.value_text as nome
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1441 and 
						o.voided=0 and e.voided=0 
			) nome on nome.patient_id=p.patient_id
		left join
			(	SELECT 	p.patient_id,						
						o.value_text as apelido
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1442 and 
						o.voided=0 and e.voided=0 
			) apelido on apelido.patient_id=p.patient_id;
			
-----------------------------------------------------------------

SELECT	p.nid,
		nome.nome,
		apelido.apelido,
		contacto.contacto
FROM	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id and e.encounter_datetime=p.dataabertura
		left join
			(	SELECT 	p.patient_id,
						e.encounter_id,						
						o.value_text as contacto
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1611 and 
						o.voided=0 and e.voided=0 and concat('',replace(value_text,' ','') * 1) = value_text
			) contacto on contacto.encounter_id=e.encounter_id and contacto.patient_id=p.patient_id
		left join
			(	SELECT 	p.patient_id,
						e.encounter_id,						
						o.value_text as nome
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1441 and 
						o.voided=0 and e.voided=0 
			) nome on nome.encounter_id=e.encounter_id and nome.patient_id=p.patient_id
		left join
			(	SELECT 	p.patient_id,	
						e.encounter_id,					
						o.value_text as apelido
				FROM 	t_paciente p 
						inner join encounter e on p.patient_id=e.patient_id
						inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
				WHERE 	e.encounter_type in (5,7) and o.concept_id=1442 and 
						o.voided=0 and e.voided=0 
			) apelido on apelido.encounter_id=e.encounter_id and apelido.patient_id=p.patient_id
WHERE	e.encounter_type in (5,7) and e.voided=0;


------------------------------------------------------------
String  sqlSelect = " SELECT	p.nid ";
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join ";
sqlSelect += "		(SELECT	e.patient_id, ";
sqlSelect += "				e.encounter_id,	 ";					
sqlSelect += "				o.value_text as nome ";
sqlSelect += "		 FROM 	encounter e  ";
sqlSelect += "				inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect += "				WHERE 	e.encounter_type in (5,7) and o.concept_id=1441 and "; 
sqlSelect += "						o.voided=0 and e.voided=0  ";
sqlSelect += "		 ) nome  ";
sqlSelect += "		 on p.patient_id=nome.patient_id ";
sqlSelect += " WHERE	p.nid is not null and p.datanasc is not null  ";