select distinct e.encounter_datetime dataabertura,pi.identifier nid,p.gender sexo,p.birthdate
datanasc,YEAR(e.encounter_datetime)-YEAR(p.birthdate) idade,
      PERIOD_DIFF(DATE_FORMAT(e.encounter_datetime,'%Y%m'),DATE_FORMAT(p.birthdate,'%Y%m')) meses,SUBSTRING(pi.identifier,3,2) coddistrito,
      geral.codproveniencia,geral.designacaoprov,geral.codigoproveniencia,geral.datainiciotarv,if(geral.emtarv=18,'TRUE','FALSE') emtarv,
      geral.codestado,geral.datadiagnostico, if(geral.aconselhado=1065,'SIM','NAO') aconselhado,geral.aceitabuscaactiva,
      geral.dataaceitabuscaactiva

from encounter e, patient_identifier pi,person p,obs o,concept c,
    (select distinct d.person_id,d.value_coded as codproveniencia,k.value_text as designacaoprov,t.value_text as codigoproveniencia,
            dt.value_datetime as datainiciotarv, cd.name as codestado, j.encounter_type as emtarv,i.value_datetime as datadiagnostico,
            z.value_coded as aconselhado, f.encounter_type as aceitabuscaactiva, f.encounter_datetime as dataaceitabuscaactiva

      from
          (select distinct o.obs_id,o.concept_id,o.person_id,o.value_coded,o.value_text
          from concept c, obs o,concept_name cn
          where o.concept_id = c.concept_id and o.concept_id=1594 and o.value_coded not in (1797,978) and o.voided=0
                and cn.concept_id=o.value_coded and cn.locale='pt') d left join

          (select distinct o.person_id,o.concept_id,e.encounter_id,e.encounter_type
            from obs o inner join encounter e on o.encounter_id=e.encounter_id
            where e.encounter_type=18
            group by o.person_id) j on j.person_id=d.person_id left join

          (select distinct o.obs_id,o.concept_id,o.person_id,o.value_coded,o.value_text
            from concept c, obs o
            where o.concept_id = c.concept_id and o.concept_id=1626 and o.voided=0) k on d.person_id=k.person_id

          left join (select distinct o.obs_id,o.concept_id,o.person_id,o.value_coded,o.value_text
                    from concept c, obs o
                    where o.concept_id = c.concept_id and o.concept_id=1627 and o.voided=0) t on d.person_id=k.person_id and k.person_id=t.person_id
          left join (select m.encounter_id,m.person_id,m.concept_id,m.value_datetime
                      from
                    (SELECT distinct o.encounter_id,o.person_id,o.concept_id,o.value_datetime
                    FROM encounter e inner join obs o on e.encounter_id=o.encounter_id
                    where e.encounter_type=18 and o.concept_id=1190) m left join

        (Select distinct o.encounter_id,o.person_id,o.concept_id,o.value_datetime
         from concept c, obs o
         where o.concept_id = c.concept_id and o.concept_id=1255 and o.value_coded=1256 and o.voided=0) p on m.person_id=p.person_id
         and m.encounter_id=p.encounter_id) dt on dt.person_id=d.person_id

         left join (SELECT o.person_id,o.concept_id,o.value_coded,cn.name
                    FROM person p left join obs o on p.person_id=o.person_id
                  left join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt'
                  where o.concept_id in (1708,6138) and o.value_coded in (1707,1706,1366,1704,1709,5622,1706,1707) and o.voided=0)
                   cd on d.person_id=cd.person_id left join
       (select distinct o.person_id,o.concept_id,e.encounter_id,o.value_datetime
            from obs o inner join encounter e on o.encounter_id=e.encounter_id
            where e.encounter_type in (5,7) and o.concept_id=6123 and o.voided=0) i on d.person_id=i.person_id

       left join (select distinct o.person_id,o.concept_id,e.encounter_id,o.value_coded
            from obs o inner join encounter e on o.encounter_id=e.encounter_id
            left join concept c on c.concept_id=o.concept_id
            where e.encounter_type in (5,7) and o.concept_id=1463 and o.value_coded in (1065,1066) and o.voided=0) z on d.person_id=z.person_id

       left join (select distinct o.person_id,o.concept_id,e.encounter_id,e.encounter_type,e.encounter_datetime
            from obs o inner join encounter e on o.encounter_id=e.encounter_id
            where e.encounter_type=30 and o.voided=0) f on f.person_id=d.person_id) geral

where e.patient_id=pi.patient_id and p.person_id=e.patient_id and e.encounter_id=o.encounter_id and o.concept_id=c.concept_id
and e.encounter_type in (5,7) and pi.identifier_type=2 and geral.person_id=o.person_id
and e.voided=0  and p.voided=0 and pi.voided=0 and o.voided=0