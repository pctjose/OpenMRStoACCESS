
// PARTE A


Select p.nid,codprofissao.codprofissao,codnivel.codnivel,nrconviventes.nrconviventes,codestadocivil.codestadocivil,nrconjuges.nrconjuges,
       serologiaHivconjuge.serologiaHivconjuge,SUBSTRING(p.nid,10,7) as Nrprocesso,outrosparceiros.outrosparceiros,nrfilhos.nrfilhos,
       nrfilhostestados.nrfilhostestados,nrfilhoshiv.nrfilhoshiv,tabaco.tabaco,alcool.alcool,droga.droga,nrparceiros.nrparceiros,
       antecedentesgenelogicos.antecedentesgenelogicos,datamestruacao.datamestruacao,aborto.aborto,ptv.ptv,ptvquais.ptvquais,gravida.gravida,
       semanagravidez.semanagravidez,dataprevistoparto.dataprevistoparto,dataparto.puerpera,dataparto.dataparto,tipoaleitamento.tipoaleitamento,
       Alergiamedicamentos.Alergiamedicamentos,Alergiasquais.Alergiasquais,Antecedentestarv.Antecedentestarv,antecedentesquais.antecedentesquais,
       exposicaoacidental.exposicaoacidental,tipoacidente.tipoacidente
From t_paciente p inner join encounter e on e.patient_id=p.patient_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as codprofissao
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1459 and o.voided=0 and e.voided=0
			) codprofissao on codprofissao.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1445 then 'NENHUM'
					when 1446 then 'PRIMARIO'
                    when 1447 then 'SECUNDARIO BASICO'
					when 6124 then 'TECNICO BASICO'
                    when 1444 then 'SECUNDARIO MEDIO'
					when 6125 then 'TECNICO MEDIO'
                    when 1448 then 'UNIVERSITARIO'
					else '' end as codnivel
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1443 and o.voided=0 and e.voided=0		   
		   ) codnivel on codnivel.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconviventes
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1656 and o.voided=0 and e.voided=0
			) nrconviventes on nrconviventes.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1057 then 'S'
					when 5555 then 'C'
                    when 1059 then 'V'
					when 1060 then 'U'
                    when 1056 then 'SEPARADO'
					when 1058 then 'DIVORCIADO'
					else '' end as codestadocivil
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1054 and o.voided=0 and e.voided=0		   
		   ) codestadocivil on codestadocivil.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconjuges
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=5557 and o.voided=0 and e.voided=0
			) nrconjuges on nrconjuges.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1169 then 'POSETIVO'
					when 1066 then 'NEGATIVO'
                    when 1457 then 'SEM INFORMACAO'
					else '' end as serologiaHivconjuge
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1449 and o.voided=0 and e.voided=0		   
		   ) serologiaHivconjuge on serologiaHivconjuge.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as outrosparceiros
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1451 and o.voided=0 and e.voided=0
			) outrosparceiros on outrosparceiros.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhos
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=5573 and o.voided=0 and e.voided=0
			) nrfilhos on nrfilhos.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhostestados
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1452 and o.voided=0 and e.voided=0
			) nrfilhostestados on nrfilhostestados.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhoshiv
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1453 and o.voided=0 and e.voided=0
			) nrfilhoshiv on nrfilhoshiv.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then true
					when 1066 then false
					else false end as tabaco
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1388 and o.voided=0 and e.voided=0		   
		   ) tabaco on tabaco.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then true
					when 1066 then false
					else false end as alcool
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1603 and o.voided=0 and e.voided=0		   
		   ) alcool on alcool.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then true
					when 1066 then false
					else false end as droga
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=105 and o.voided=0 and e.voided=0		   
		   ) droga on droga.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1662 then 1
					when 1663 then 2
                    when 1664 then 3
					else 4 end as nrparceiros
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1666 and o.voided=0 and e.voided=0		   
		   ) nrparceiros on nrparceiros.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as antecedentesgenelogicos
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1394 and o.voided=0 and e.voided=0
			) antecedentesgenelogicos on antecedentesgenelogicos.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datamestruacao
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1465 and o.voided=0 and e.voided=0
			) datamestruacao on datamestruacao.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 50 then true
					when 1066 then false
					else false end as aborto
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1667 and o.voided=0 and e.voided=0		   
		   ) aborto on aborto.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then true
					when 1066 then false
					else false end as ptv
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1466 and o.voided=0 and e.voided=0		   
		   ) ptv on ptv.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 631 then 'NVP'
					when 797 then 'AZT'
                    when 792 then 'D4T+3TC+NVP'
					when 1800 then 'TARV'
                    when 1801 then 'AZT+NVP'
					when 630 then 'AZT+3TC'
                    when 628 then '3TC'
					else '' end as ptvquais
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1504 and o.voided=0 and e.voided=0		   
		   ) ptvquais on ptvquais.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 44 then true
					when 1066 then false                    
					else false end as gravida
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1982 and o.voided=0 and e.voided=0		   
		   ) gravida on gravida.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as semanagravidez
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1279 and o.voided=0 and e.voided=0
			) semanagravidez on semanagravidez.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataprevistoparto
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=1600 and o.voided=0 and e.voided=0
			) dataprevistoparto on dataprevistoparto.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataparto
			FROM 	encounter e 
					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
			WHERE 	e.encounter_type=5 and o.concept_id=5599 and o.voided=0 and e.voided=0
			) dataparto on dataparto.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 5526 then 'MATERNO'
					when 5254 then 'ARTIFICIAL'
                    when 6046 then 'MISTO'
					else 'OUTRO' end as tipoaleitamento
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1151 and o.voided=0 and e.voided=0		   
		   ) tipoaleitamento on tipoaleitamento.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'SIM'
					when 1066 then 'NAO'
                    when 1067 then 'NAO SABE'
					else 'OUTRO' end as Alergiamedicamentos
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1601 and o.voided=0 and e.voided=0		   
		   ) Alergiamedicamentos on Alergiamedicamentos.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as Alergiasquais
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1517 and o.voided=0 and e.voided=0		   
		   ) Alergiasquais on Alergiasquais.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1065 then 'TRUE'
					when 1066 then 'FALSE'
					else '' end as Antecedentestarv
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1192 and o.voided=0 and e.voided=0		   
		   ) Antecedentestarv on Antecedentestarv.encounter_id=e.encounter_id

left join  (SELECT o.person_id,o.encounter_id,cn.name as antecedentesquais
           FROM obs o inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'
           inner join encounter e on e.encounter_id=o.encounter_id
           where e.encounter_type=5 and o.concept_id in (1087) and o.value_coded in (814,797,5424,628,633,631,635,749,795,817,1651,796,1702,1701,625,792,1703,792,
           1700,630,1579,6100,6101,6102,6103,6104,6105,6106,6107,6108,6109,6110,6111,6112,6113,6114,6115,6116,6117,6118,6119,1827,1067) 
           and o.voided=0 and cn.voided=0 and e.voided=0) antecedentesquais on antecedentesquais.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 1443 then 'TRUE'
					when 1066 then 'FALSE'
					else '' end as exposicaoacidental
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1687 and o.voided=0 and e.voided=0		   
		   ) exposicaoacidental on exposicaoacidental.encounter_id=e.encounter_id

left join (	SELECT 	o.person_id,e.encounter_id,
					case o.value_coded
					when 181 then 'Pica com agulha contaminada'
					when 5564 then 'Suspeito de um parceiro contaminado'
                    when 1508 then ' Extracao dentaria'
					when 1507 then 'Injeccao'
                    when 1509 then 'Escarificacao'
					else '' end as tipoacidente
			FROM 	encounter e 					
					inner join obs o on e.encounter_id=o.encounter_id 
           WHERE 	e.encounter_type=5 and o.concept_id=1061 and o.voided=0 and e.voided=0		   
		   ) tipoacidente on tipoacidente.encounter_id=e.encounter_id

where e.encounter_type=5 and  e.voided=0;




// PATE B


SELECT	p.nid,
		historiaactual.historiaactual,
		hipotesedediagnostico.hipotesedediagnostico,
		codkarnosfsky.codkarnosfsky,
		geleira.geleira,
		electricidade.electricidade,
		sexualidade.sexualidade
FROM	t_paciente p 
		inner join encounter e on e.patient_id=p.patient_id

		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as historiaactual
					FROM 	encounter e 
							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
					WHERE 	e.encounter_type=1 and o.concept_id=1671 and o.voided=0 and e.voided=0
				  ) historiaactual on historiaactual.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as hipotesedediagnostico
					FROM 	encounter e 
							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
					WHERE 	e.encounter_type=1 and o.concept_id=1649 and o.voided=0 and e.voided=0
				  ) hipotesedediagnostico on hipotesedediagnostico.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as codkarnosfsky
					FROM 	encounter e 
							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id
					WHERE 	e.encounter_type=1 and o.concept_id=5283 and o.voided=0 and e.voided=0
				  ) codkarnosfsky on codkarnosfsky.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							else false end as geleira
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=5 and o.concept_id=1455 and o.voided=0 and e.voided=0		   
				 ) geleira on geleira.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,e.encounter_id,
							case o.value_coded
								when 1065 then true
								when 1066 then false
							else false end as electricidade
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=5 and o.concept_id=5609 and o.voided=0 and e.voided=0		   
				  ) electricidade on electricidade.encounter_id=e.encounter_id

		left join (	SELECT 	o.person_id,e.encounter_id,
							case o.value_coded
								when 1376 then 'HETEROSSEXUAL'
								when 1377 then 'HOMOSSEXUAL'
								when 1378 then 'BISSEXUAL'
							else 'OUTRO' end as sexualidade
					FROM 	encounter e 					
							inner join obs o on e.encounter_id=o.encounter_id 
					WHERE 	e.encounter_type=5 and o.concept_id=1375 and o.voided=0 and e.voided=0		   
				 ) sexualidade on sexualidade.encounter_id=e.encounter_id

WHERE	e.encounter_type in (1,5) and  e.voided=0;



===================================================================================================================================

Rev. Eurico 04-02-2012

 SELECT	DISTINCT	p.nid, 
					codprofissao.codprofissao, 
					codnivel.codnivel, 
					nrconviventes.nrconviventes, 
					codestadocivil.codestadocivil, 
					nrconjuges.nrconjuges,  
					serologiaHivconjuge.serologiaHivconjuge, 
					Nrprocesso.Nrprocesso, 
					outrosparceiros.outrosparceiros, 
					nrfilhos.nrfilhos,  
					nrfilhostestados.nrfilhostestados, 
					nrfilhoshiv.nrfilhoshiv, 
					tabaco.tabaco, 
					alcool.alcool, 
					droga.droga, 
					nrparceiros.nrparceiros,  
					antecedentesgenelogicos.antecedentesgenelogicos, 
					datamestruacao.datamestruacao, 
					aborto.aborto, 
					ptv.ptv, 					
					gravida.gravida,  
					semanagravidez.semanagravidez, 
					dataprevistoparto.dataprevistoparto, 
					dataparto.dataparto, 
					tipoaleitamento.tipoaleitamento,  
					Alergiamedicamentos.Alergiamedicamentos, 
					Alergiasquais.Alergiasquais, 
					Antecedentestarv.Antecedentestarv, 
					antecedentesquais.antecedentesquais,  
					exposicaoacidental.exposicaoacidental   
 FROM		t_paciente p  
			inner join encounter e on e.patient_id=p.patient_id  ";
            left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as codprofissao 
                        FROM 	encounter e  
                                inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                        WHERE 	e.encounter_type=5 and o.concept_id=1459 and o.voided=0 and e.voided=0 
                       ) codprofissao on codprofissao.encounter_id=e.encounter_id 
            left join (	SELECT 	o.person_id,e.encounter_id,  
                                case o.value_coded  
									when 1445 then 'NENHUM' 
                                    when 1446 then 'PRIMARIO' 
                                    when 1447 then 'SECUNDARIO BASICO' 
                                    when 6124 then 'TECNICO BASICO' 
                                    when 1444 then 'SECUNDARIO MEDIO' 
                                    when 6125 then 'TECNICO MEDIO' 
                                    when 1448 then 'UNIVERSITARIO' 
                                    else 'OUTRO' end as codnivel 
                                FROM 	encounter e 
                                      inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1443 and o.voided=0 and e.voided=0	 
                            ) codnivel on codnivel.encounter_id=e.encounter_id  
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconviventes  
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1656 and o.voided=0 and e.voided=0  
                            ) nrconviventes on nrconviventes.encounter_id=e.encounter_id  
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 
                                        when 1057 then 'S' 
                                        when 5555 then 'C' 
                                        when 1059 then 'V' 
                                        when 1060 then 'U' 
                                        when 1056 then 'SEPARADO' 
                                        when 1058 then 'DIVORCIADO' 
                                        else 'OUTRO' end as codestadocivil 
                                FROM 	encounter e  
                                     inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1054 and o.voided=0 and e.voided=0 
                             ) codestadocivil on codestadocivil.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconjuges 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=5557 and o.voided=0 and e.voided=0 
                             ) nrconjuges on nrconjuges.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 
                                        when 1169 then 'POSETIVO' 
                                        when 1066 then 'NEGATIVO' 
                                        when 1457 then 'SEM INFORMACAO' 
                                        else '' end as serologiaHivconjuge 
                                FROM 	encounter e  
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1449 and o.voided=0 and e.voided=0 
                             ) serologiaHivconjuge on serologiaHivconjuge.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as outrosparceiros 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1451 and o.voided=0 and e.voided=0 
                             ) outrosparceiros on outrosparceiros.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhos 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=5573 and o.voided=0 and e.voided=0 
                            ) nrfilhos on nrfilhos.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhostestados 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1452 and o.voided=0 and e.voided=0 
                             ) nrfilhostestados on nrfilhostestados.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhoshiv 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1453 and o.voided=0 and e.voided=0 
                             ) nrfilhoshiv on nrfilhoshiv.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 			
                                        when 1065 then true 			
                                        when 1066 then false 			
                                        else false end as tabaco 			
                                        FROM 	encounter e  
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 			
                                        WHERE 	e.encounter_type=5 and o.concept_id=1388 and o.voided=0 and e.voided=0	 			
                             ) tabaco on tabaco.encounter_id=e.encounter_id 			
                  left join (	SELECT 	o.person_id,e.encounter_id, 			
                                        case o.value_coded 			
                                        when 1065 then true 			
                                        when 1066 then false 			
                                        else false end as alcool 		
                                FROM 	encounter e  
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1603 and o.voided=0 and e.voided=0	 
                             ) alcool on alcool.encounter_id=e.encounter_id  
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 
                                        when 1065 then true 
                                        when 1066 then false 
                                        else false end as droga 
                                FROM 	encounter e  
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=105 and o.voided=0 and e.voided=0 
                             ) droga on droga.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 
                                        when 1662 then 1 
                                        when 1663 then 2 
                                        when 1664 then 3 
                                        else 4 end as nrparceiros 
                                FROM 	encounter e 
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1666 and o.voided=0 and e.voided=0 
                             ) nrparceiros on nrparceiros.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as antecedentesgenelogicos 
                                FROM 	encounter e  
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1394 and o.voided=0 and e.voided=0 
                             ) antecedentesgenelogicos on antecedentesgenelogicos.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datamestruacao 
                                FROM 	encounter e 
                                        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                                WHERE 	e.encounter_type=5 and o.concept_id=1465 and o.voided=0 and e.voided=0 
                             ) datamestruacao on datamestruacao.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,  
                                        case o.value_coded  
                                        when 50 then true  
                                        when 1066 then false  
                                        else false end as aborto  
                                FROM 	encounter e  
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1667 and o.voided=0 and e.voided=0 
                             ) aborto on aborto.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                                        case o.value_coded 
                                        when 1065 then true 
                                        when 1066 then false 
                                        else false end as ptv 
                                FROM 	encounter e 
                                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1466 and o.voided=0 and e.voided=0 
                             ) ptv on ptv.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                 					    case o.value_coded 
                 				    	when 44 then true  
                 				    	when 1066 then false  
                 				        else false end as gravida 
                 			    FROM 	encounter e  
                			    		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1982 and o.voided=0 and e.voided=0	 
                 	         ) gravida on gravida.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as semanagravidez 
                 			    FROM 	encounter e  
                 					    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                 			    WHERE 	e.encounter_type=5 and o.concept_id=1279 and o.voided=0 and e.voided=0 
                 			 ) semanagravidez on semanagravidez.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataprevistoparto 
                 			    FROM 	encounter e  
                 				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                 			    WHERE 	e.encounter_type=5 and o.concept_id=1600 and o.voided=0 and e.voided=0 
                 			 ) dataprevistoparto on dataprevistoparto.encounter_id=e.encounter_id  
                  left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataparto 
                 			    FROM 	encounter e  
                 				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id 
                 			    WHERE 	e.encounter_type=5 and o.concept_id=5599 and o.voided=0 and e.voided=0 
                 			 ) dataparto on dataparto.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                 				    	case o.value_coded 
                 					    when 5526 then 'MATERNO' 
                 				    	when 5254 then 'ARTIFICIAL' 
                                        when 6046 then 'MISTO' 
                 					    else 'OUTRO' end as tipoaleitamento 
                 			    FROM 	encounter e  
                				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1151 and o.voided=0 and e.voided=0 
                 		     ) tipoaleitamento on tipoaleitamento.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id,  
                 					    case o.value_coded  
                 			    		when 1065 then 'SIM'  
                 					    when 1066 then 'NAO'  
                                        when 1067 then 'NAO SABE'  
                 					    else 'OUTRO' end as Alergiamedicamentos  
                 		    	FROM 	encounter e  
                			    		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1601 and o.voided=0 and e.voided=0	 
                 		     ) Alergiamedicamentos on Alergiamedicamentos.encounter_id=e.encounter_id 
                 left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as Alergiasquais  
                 			    FROM 	encounter e  
                 					    inner join obs o on e.encounter_id=o.encounter_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1517 and o.voided=0 and e.voided=0 
                 		     ) Alergiasquais on Alergiasquais.encounter_id=e.encounter_id 
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                 					    case o.value_coded 
                 					    when 1065 then true  
                 					    when 1066 then false  
                 					    else false end as Antecedentestarv 
                 			    FROM 	encounter e  
                					    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1192 and o.voided=0 and e.voided=0 
                 		      ) Antecedentestarv on Antecedentestarv.encounter_id=e.encounter_id 
                  left join  (  SELECT  o.person_id,o.encounter_id,cn.name as antecedentesquais 
                                FROM    obs o   inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED' 
                                                inner join encounter e on e.encounter_id=o.encounter_id and o.voided=0 and cn.voided=0 and e.voided=0 
                                WHERE   e.encounter_type=5 and o.concept_id=1087 and o.voided=0 and cn.voided=0 and e.voided=0  
                            ) antecedentesquais on antecedentesquais.encounter_id=e.encounter_id  
                  left join (	SELECT 	o.person_id,e.encounter_id, 
                 					    case o.value_coded 
                 					    when 1443 then 'SIM' 
                 					    when 1066 then 'NAO' 
                 					    else '' end as exposicaoacidental 
                 			    FROM 	encounter e  
                 					    inner join obs o on e.encounter_id=o.encounter_id  
                                WHERE 	e.encounter_type=5 and o.concept_id=1687 and o.voided=0 and e.voided=0 
                 		     ) exposicaoacidental on exposicaoacidental.encounter_id=e.encounter_id 
                 left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as Nrprocesso  
                			    FROM 	encounter e  
                					    inner join obs o on e.encounter_id=o.encounter_id  
                               WHERE 	e.encounter_type=5 and o.concept_id=1450 and o.voided=0 and e.voided=0 
                		     ) Nrprocesso on Nrprocesso.encounter_id=e.encounter_id  
                 where    e.encounter_type=5 and  e.voided=0 
                           
=======================================================

String  sqlSelect = " SELECT	p.nid, ";
sqlSelect+="		historiaactual.historiaactual, ";
sqlSelect+="		hipotesedediagnostico.hipotesedediagnostico, ";
sqlSelect+="		codkarnosfsky.codkarnosfsky, ";
sqlSelect+="		geleira.geleira, ";
sqlSelect+="		electricidade.electricidade, ";
sqlSelect+="		sexualidade.sexualidade ";
sqlSelect+=" FROM	t_paciente p  ";
sqlSelect+="		inner join encounter e on e.patient_id=p.patient_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as historiaactual ";
sqlSelect+="					FROM 	encounter e  ";
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect+="					WHERE 	e.encounter_type=1 and o.concept_id=1671 and o.voided=0 and e.voided=0 ";
sqlSelect+="				  ) historiaactual on historiaactual.encounter_id=e.encounter_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as hipotesedediagnostico ";
sqlSelect+="					FROM 	encounter e  ";
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect+="					WHERE 	e.encounter_type=1 and o.concept_id=1649 and o.voided=0 and e.voided=0 ";
sqlSelect+="				  ) hipotesedediagnostico on hipotesedediagnostico.encounter_id=e.encounter_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as codkarnosfsky ";
sqlSelect+="					FROM 	encounter e  ";
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
sqlSelect+="					WHERE 	e.encounter_type=1 and o.concept_id=5283 and o.voided=0 and e.voided=0 ";
sqlSelect+="				  ) codkarnosfsky on codkarnosfsky.encounter_id=e.encounter_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id, ";
sqlSelect+="							case o.value_coded ";
sqlSelect+="								when 1065 then true ";
sqlSelect+="								when 1066 then false "; 
sqlSelect+="							else false end as geleira ";
sqlSelect+="					FROM 	encounter e 	 ";				
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect+="					WHERE 	e.encounter_type=5 and o.concept_id=1455 and o.voided=0 and e.voided=0	 ";	   
sqlSelect+="				 ) geleira on geleira.encounter_id=e.encounter_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id, ";
sqlSelect+="							case o.value_coded ";
sqlSelect+="								when 1065 then true ";
sqlSelect+="								when 1066 then false ";
sqlSelect+="							else false end as electricidade ";
sqlSelect+="					FROM 	encounter e 		 ";			
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect+="					WHERE 	e.encounter_type=5 and o.concept_id=5609 and o.voided=0 and e.voided=0 ";		   
sqlSelect+="				  ) electricidade on electricidade.encounter_id=e.encounter_id ";
sqlSelect+="		left join (	SELECT 	o.person_id,e.encounter_id, ";
sqlSelect+="							case o.value_coded ";
sqlSelect+="								when 1376 then 'HETEROSSEXUAL' ";
sqlSelect+="								when 1377 then 'HOMOSSEXUAL' ";
sqlSelect+="								when 1378 then 'BISSEXUAL' ";
sqlSelect+="							else 'OUTRO' end as sexualidade ";
sqlSelect+="					FROM 	encounter e  ";					
sqlSelect+="							inner join obs o on e.encounter_id=o.encounter_id  ";
sqlSelect+="					WHERE 	e.encounter_type=5 and o.concept_id=1375 and o.voided=0 and e.voided=0 ";		   
sqlSelect+="				 ) sexualidade on sexualidade.encounter_id=e.encounter_id ";
sqlSelect+=" WHERE	e.encounter_type in (1,5) and  e.voided=0";


SELECT 	o.person_id,
		e.encounter_id,
		case o.value_coded
			when 181 then 'Pica com agulha contaminada'
            when 5564 then 'Suspeito de um parceiro contaminado'
            when 1508 then ' Extracao dentaria'
            when 1507 then 'Injeccao'
            when 1509 then 'Escarificacao'
        else '' end as tipoacidente
FROM 	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
        inner join obs o on e.encounter_id=o.encounter_id 
WHERE 	e.encounter_type=5 and o.concept_id=1061 and o.voided=0 and e.voided=0

SELECT 	o.person_id,
		e.encounter_id,
		o.value_text as tipoacidente
FROM 	t_paciente p
		inner join encounter e on p.patient_id=e.patient_id
        inner join obs o on e.encounter_id=o.encounter_id 
WHERE 	e.encounter_type=5 and o.concept_id=1435 and o.voided=0 and e.voided=0


SELECT 	o.person_id,
		e.encounter_id, 
        case o.value_coded 
			when 631 then 'NVP' 
            when 797 then 'AZT' 
            when 792 then 'D4T+3TC+NVP' 
            when 1800 then 'TARV' 
            when 1801 then 'AZT+NVP' 
            when 630 then 'AZT+3TC' 
            when 628 then '3TC'
            when 916 then 'CTZ' 
        else 'OUTRO' end as ptvquais 
FROM 	t_paciente p 
		inner join encounter e on p.patient_id=e.patient_id
        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id  
WHERE 	e.encounter_type=5 and o.concept_id=1504 and o.voided=0 and e.voided=0 
ORDER BY p.patient_id; 

Select	person_id,
		value as telefone
from	person_attribute
where	person_attribute_type_id=9 and voided=0
                            


