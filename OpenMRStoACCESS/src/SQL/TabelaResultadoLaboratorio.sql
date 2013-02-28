
NUMERIC EXAM
---------------------		
SELECT	p.patient_id,
		e.encounter_id,
		c.concept_id,
		case c.concept_id
			when 730 then 'CD4'
			when 5497 then 'CD4'
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
		else cn.name end as codexame,
		p.nid,	
		o.obs_datetime as dataresultado,	
		case c.concept_id
			when 730 then 'PERCENTUAL'
			when 5497 then 'ABSOLUTO'
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
		else null end as codparametro,		
		o.value_numeric as resultado,
		pedido.data_pedido
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED' 
		inner join	concept c on c.concept_id=o.concept_id
		
		left join	(
						select	e.encounter_id,
								o.value_datetime as data_pedido
						from	encounter e 
								inner join obs o on e.encounter_id=o.encounter_id
						where	e.voided=0 and o.voided=0 and e.encounter_type=13 and o.concept_id=6246
					) pedido on e.encounter_id=pedido.encounter_id
		          
WHERE	e.encounter_type=13 and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=1 and c.is_set=0;
		
-----------------------------------

String sqlSelect = " SELECT	p.patient_id, ";
sqlSelect += "		e.encounter_id, ";
sqlSelect += "		c.concept_id, ";
sqlSelect += "		case c.concept_id ";
sqlSelect += "			when 730 then 'CD4' ";
sqlSelect += "			when 5497 then 'CD4' ";
sqlSelect += "			when 653 then 'AST' ";
sqlSelect += "			when 654 then 'ALT' ";
sqlSelect += "			when 1021 then 'LINFOCITO' ";
sqlSelect += "			when 952 then 'LINFOCITO' ";
sqlSelect += "			when 1022 then 'NEUTROFILO' ";
sqlSelect += "			when 1330 then 'NEUTROFILO' ";
sqlSelect += "			when 1024 then 'EOSINOFILO' ";
sqlSelect += "			when 1332 then 'EOSINOFILO' ";
sqlSelect += "			when 1025 then 'BASOFILO' ";
sqlSelect += "			when 1333 then 'BASOFILO' ";
sqlSelect += "			when 1023 then 'MONOCITO' ";
sqlSelect += "			when 1331 then 'MONOCITO' ";
sqlSelect += "			when 1017 then 'CMHC' ";
sqlSelect += "			when 851 then 'VCM' ";
sqlSelect += "			when 21 then 'HB' ";
sqlSelect += "			when 1018 then 'HGM' ";
sqlSelect += "			when 678 then 'WBC' ";
sqlSelect += "			when 679 then 'RBC' ";
sqlSelect += "		else cn.name end as codexame, ";
sqlSelect += "		p.nid,	";
sqlSelect += "		o.obs_datetime as dataresultado, ";	
sqlSelect += "		case c.concept_id ";
sqlSelect += "			when 730 then 'PERCENTUAL' ";
sqlSelect += "			when 5497 then 'ABSOLUTO' ";
sqlSelect += "			when 1021 then 'PERCENTUAL' ";
sqlSelect += "			when 952 then 'ABSOLUTO' ";
sqlSelect += "			when 1022 then 'PERCENTUAL' ";
sqlSelect += "			when 1330 then 'ABSOLUTO' ";
sqlSelect += "			when 1024 then 'PERCENTUAL' ";
sqlSelect += "			when 1332 then 'ABSOLUTO' ";
sqlSelect += "			when 1025 then 'PERCENTUAL' ";
sqlSelect += "			when 1333 then 'ABSOLUTO' ";
sqlSelect += "			when 1023 then 'PERCENTUAL' ";
sqlSelect += "			when 1331 then 'ABSOLUTO' ";
sqlSelect += "		else null end as codparametro, ";		
sqlSelect += "		o.value_numeric as resultado ";
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and ";
sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";
sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id   ";       
sqlSelect += " WHERE	e.encounter_type=13 and  ";
sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and ";
sqlSelect += "		c.datatype_id=1 and c.is_set=0;	";	



sqlSelect += " left join	( ";
sqlSelect += "						select	e.encounter_id, ";
sqlSelect += "								o.value_datetime as data_pedido ";
sqlSelect += "						from	encounter e  ";
sqlSelect += "								inner join obs o on e.encounter_id=o.encounter_id ";
sqlSelect += "						where	e.voided=0 and o.voided=0 and e.encounter_type=13 and o.concept_id=6246 ";
sqlSelect += "					) pedido on e.encounter_id=pedido.encounter_id ";


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
		else cn.name end as codexame,
		p.nid,	
		o.obs_datetime as dataresultado,	
		case o.value_coded
			when 1229 then 'NAO REACTIVO'
			when 1228 then 'REACTIVO'
			when 1304 then 'MA QUALIDADE DE AMOSTRA'
			when 664 then 'NEGATIVO'
			when 703 then 'POSITIVO'
			when 1138 then 'INDETERMINADO'
			when 690 then 'A POSITIVO'
			when 692 then 'A NEGATIVO'
			when 694 then 'B POSITIVO'
			when 696 then 'B NEGATIVO'
			when 699 then 'O POSITIVO'
			when 701 then 'O NEGATIVO'
			when 1230 then 'AB POSITIVO'
			when 1231 then 'AB NEGATIVO'
		else 'OUTRO' end as codparametro
FROM	t_paciente p
		inner join	encounter e on p.patient_id=e.patient_id
		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id
		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and 
					cn.concept_name_type='FULLY_SPECIFIED' 
		inner join	concept c on c.concept_id=o.concept_id          
WHERE	e.encounter_type=13 and  
		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and 
		c.datatype_id=2 and c.is_set=0;	
		
-----		
String sqlSelect = " SELECT	p.patient_id, ";
sqlSelect += "		e.encounter_id, ";
sqlSelect += "		c.concept_id, ";
sqlSelect += "		case c.concept_id ";
sqlSelect += "			when 300 then 'TIPAGEM SANGUINEA' ";
sqlSelect += "			when 1655 then 'RPR' ";
sqlSelect += "			when 299 then 'VDRL' ";
sqlSelect += "			when 307 then 'BACILOSCOPIA' ";
sqlSelect += "			when 1030 then 'PCR' ";			
sqlSelect += "		else cn.name end as codexame, ";
sqlSelect += "		p.nid,	";
sqlSelect += "		o.obs_datetime as dataresultado, ";	
sqlSelect += "		case o.value_coded ";
sqlSelect += "			when 1229 then 'NAO REACTIVO' ";
sqlSelect += "			when 1228 then 'REACTIVO' ";
sqlSelect += "			when 1304 then 'MA QUALIDADE DE AMOSTRA' ";
sqlSelect += "			when 664 then 'NEGATIVO' ";
sqlSelect += "			when 703 then 'POSITIVO' ";
sqlSelect += "			when 1138 then 'INDETERMINADO' ";
sqlSelect += "			when 690 then 'A POSITIVO' ";
sqlSelect += "			when 692 then 'A NEGATIVO' ";
sqlSelect += "			when 694 then 'B POSITIVO' ";
sqlSelect += "			when 696 then 'B NEGATIVO' ";
sqlSelect += "			when 699 then 'O POSITIVO' ";
sqlSelect += "			when 701 then 'O NEGATIVO' ";
sqlSelect += "			when 1230 then 'AB POSITIVO' ";
sqlSelect += "			when 1231 then 'AB NEGATIVO' ";
sqlSelect += "		else 'OUTRO' end as codparametro ";
sqlSelect += " FROM	t_paciente p ";
sqlSelect += "		inner join	encounter e on p.patient_id=e.patient_id ";
sqlSelect += "		inner join	obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
sqlSelect += "		inner join	concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and ";
sqlSelect += "					cn.concept_name_type='FULLY_SPECIFIED' ";
sqlSelect += "		inner join	concept c on c.concept_id=o.concept_id  ";        
sqlSelect += " WHERE	e.encounter_type=13 and  ";
sqlSelect += "		o.voided=0 and cn.voided=0 and e.voided=0 and p.nid is not null and ";
sqlSelect += "		c.datatype_id=2 and c.is_set=0	";