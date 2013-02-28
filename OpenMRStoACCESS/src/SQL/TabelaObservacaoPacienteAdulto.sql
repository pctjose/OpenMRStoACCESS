NUMERIC EXAM
---------------------		
SELECT	p.patient_id,
		e.encounter_id,
		c.concept_id,
		case c.concept_id
			when 730 then 'CD4'
			when 5497 then 'CD4'
			when 5085 then 'TENSAO ARTERIAL'
			when 5086 then 'TENSAO ARTERIAL'
			when 653 then 'AST'
			when 654 then 'ALT'
			when 1021 then 'LINFOCITO'
			when 952 then 'LINFOCITO'
			when 1022 then 'NEUTROFILO'
			when 1330 then 'NEUTROFILO'
			when 1024 then 'EOSINOFILO'
			when 1332 then 'EOSINOFILO'
			when 1025 then 'BASOFILO'
			when 1333 then 'BASOFILO'
			when 1023 then 'MONOCITO'
			when 1331 then 'MONOCITO'
			when 1017 then 'CMHC'
			when 851 then 'VCM'
			when 21 then 'HB'
			when 1018 then 'HGM'
			when 678 then 'WBC'
			when 679 then 'RBC'
			when 5283 then 'INDICE DE KARNOFSKY'
			when 5314 then 'CIRCUNFERENCIA CRANIANA'
			when 1342 then 'IMC'
		else cn.name end as codobservacao,
		case c.concept_id
			when 730 then 'PERCENTUAL'
			when 5497 then 'ABSOLUTO'
			when 5085 then 'SUPERIOR'
			when 5086 then 'INFERIOR'
			when 1021 then 'PERCENTUAL'
			when 952 then 'ABSOLUTO'
			when 1022 then 'PERCENTUAL'
			when 1330 then 'ABSOLUTO'
			when 1024 then 'PERCENTUAL'
			when 1332 then 'ABSOLUTO'
			when 1025 then 'PERCENTUAL'
			when 1333 then 'ABSOLUTO'
			when 1023 then 'PERCENTUAL'
			when 1331 then 'ABSOLUTO'
		else null end as codestado,
		o.value_numeric as valor,	
		p.nid,	
		o.obs_datetime as data		
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED' 
		inner join	concept c on c.concept_id=o.concept_id          
WHERE	e.encounter_type in (1,3) and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=1 and c.is_set=0;
		


CODED EXAM
---------------------

SELECT	p.patient_id,
		e.encounter_id,
		c.concept_id,
		case c.concept_id
			when 300 then 'TIPAGEM SANGUINEA'
			when 1655 then 'RPR'
			when 299 then 'VDRL'
			when 307 then 'BACILOSCOPIA'
			when 1030 then 'PCR'			
		else cn.name end as codobservacao,
		cnc.name as codestado,
		p.nid,	
		o.obs_datetime as data		
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED' 
		inner join	concept_name cnc on cnc.concept_id=o.value_coded and cnc.locale='pt' and 
					cnc.concept_name_type='FULLY_SPECIFIED' 
		inner join	concept c on c.concept_id=o.concept_id		          
WHERE	e.encounter_type in (1,3) and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=2 and c.is_set=0;	
		
		
		
		
String sqlSelect = " SELECT	p.patient_id, ";
sqlSelect += "		e.encounter_id, ";
sqlSelect += "		c.concept_id, ";
sqlSelect += "		case c.concept_id ";
sqlSelect += "			when 300 then 'TIPAGEM SANGUINEA' ";
sqlSelect += "			when 1655 then 'RPR' ";
sqlSelect += "			when 299 then 'VDRL' ";
sqlSelect += "			when 307 then 'BACILOSCOPIA' ";
sqlSelect += "			when 1030 then 'PCR' ";			
sqlSelect += "		else cn.name end as codobservacao, ";
sqlSelect += "		cnc.name as codestado, ";
sqlSelect += "		p.nid,	 ";
sqlSelect += "		o.obs_datetime as data	 ";	
sqlSelect += " FROM	t_paciente p  ";
sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and  ";
sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED'  ";
sqlSelect += "		inner join	concept_name cnc on cnc.concept_id=o.value_coded and cnc.locale='pt' and  ";
sqlSelect += "					cnc.concept_name_type='FULLY_SPECIFIED'  ";
sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id	 ";	          
sqlSelect += " WHERE	e.encounter_type in (1,3) and   ";
sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and  ";
sqlSelect += "		c.datatype_id=2 and c.is_set=0  ";	
		
		
Text EXAM
---------------------

SELECT	p.patient_id,
		e.encounter_id,
		c.concept_id,					
		cn.name as codobservacao,
		o.value_text as valor,
		p.nid,	
		o.obs_datetime as dataresultado			
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED'		
		inner join	concept c on c.concept_id=o.concept_id		          
WHERE	e.encounter_type in (1,3) and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=3 and c.is_set=0;
		
		
String sqlSelect = " SELECT	p.patient_id, ";
sqlSelect += "		e.encounter_id, ";
sqlSelect += "		c.concept_id, ";					
sqlSelect += "		cn.name as codobservacao, ";
sqlSelect += "		o.value_text as valor, ";
sqlSelect += "		p.nid,	 ";
sqlSelect += "		o.obs_datetime as dataresultado ";			
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and  ";
sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";		
sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id	 ";	          
sqlSelect += " WHERE	e.encounter_type in (1,3) and  "; 
sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and  ";
sqlSelect += "		c.datatype_id=3 and c.is_set=0; ";
		
		
Date EXAM
---------------------

SELECT	p.patient_id,
		e.encounter_id,
		c.concept_id,					
		cn.name as codobservacao,
		o.value_datetime as valor,
		p.nid,	
		o.obs_datetime as dataresultado		
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED'		
		inner join	concept c on c.concept_id=o.concept_id		          
WHERE	e.encounter_type in (1,3) and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=6 and c.is_set=0;	