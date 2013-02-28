set sql_mode='';
DELIMITER $$

DROP PROCEDURE IF EXISTS FillTPacienteTable $$
CREATE PROCEDURE FillTPacienteTable(dataFinal date,hddid varchar(20),distrito varchar(20))
    READS SQL DATA
begin

truncate table t_paciente;

/*Inscricao*/
insert into t_paciente(patient_id,sexo,datanasc,hdd,coddistrito)
	select 	p.patient_id,pe.gender,pe.birthdate,hddid,distrito
	from 	patient p inner join person pe on p.patient_id=pe.person_id
	where 	p.voided=0 and pe.voided=0;

/*Update data abertura*/
Update 	t_paciente,
		(	Select 	p.patient_id,min(encounter_datetime) data_abertura,if(e.encounter_type=5,'Adulto','Crianca') tipopaciente,e.provider_id,e.location_id	
			from 	patient p inner join encounter e on e.patient_id=p.patient_id
			where 	p.voided=0 and e.encounter_type in (5,7) and e.voided=0 
			group by patient_id
		) abertura
set t_paciente.dataabertura=abertura.data_abertura,
	t_paciente.tipopaciente=abertura.tipopaciente,
	t_paciente.provider_id=abertura.provider_id,
	t_paciente.location_id=abertura.location_id
where t_paciente.patient_id=abertura.patient_id;

/*Update NID*/
update 	t_paciente,
		(	select 	distinct p.patient_id,pd.identifier
			from 	patient_identifier pd 
					inner join patient p on p.patient_id=pd.patient_id 		
			where 	pd.identifier_type=2 and pd.voided=0 and p.voided=0
		) nid
set t_paciente.nid=nid.identifier
where nid.patient_id=t_paciente.patient_id;

/*Update Idade*/
update t_paciente
set idade=round(datediff(dataabertura,datanasc)/365)
where dataabertura is not null;

update t_paciente
set meses=round(datediff(dataabertura,datanasc)/30)
where dataabertura is not null and idade<2;

/*Update CodProveniencia*/
update t_paciente,
		(Select 	p.patient_id, 
				case o.value_coded
				when 1595 then 'ENF'
				when 1596 then 'C.ext'
				when 1414 then 'PNCTL'
				when 1597 then 'GATV'
				when 1987 then 'SAAJ'
				when 1598 then 'PTV'
				when 1872 then 'CCR'
				when 1275 then 'CS'
				when 1984 then 'HG/HR'
				when 1599 then 'CP'
				when 1932 then 'Contacto'
				when 1387 then 'Laboratorio'
				when 1386 then 'Clinica Movel'
				else 'Outro' end as codProv
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=1594 and e.encounter_type in (5,7) and e.voided=0
		) proveniencia
set t_paciente.codproveniencia=proveniencia.codProv
where proveniencia.patient_id=t_paciente.patient_id;

/*Update LocalProveniencia*/
update t_paciente,
		(Select 	p.patient_id, 
					o.value_text
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=1626 and e.encounter_type in (5,7) and e.voided=0
		) desprov
set t_paciente.designacaoprov=desprov.value_text
where desprov.patient_id=t_paciente.patient_id;

/*Update CodigoProveniencia*/
update t_paciente,
		(Select 	p.patient_id, 
					o.value_text
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=1627 and e.encounter_type in (5,7) and e.voided=0
		) desprov
set t_paciente.codigoproveniencia=desprov.value_text
where desprov.patient_id=t_paciente.patient_id;

/*Inicio TARV*/
update t_paciente,
	(	Select 	p.patient_id,
				min(e.encounter_datetime) data_tarv
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type=18 and o.concept_id=1255 and o.value_coded in (1256,1369)
		group by p.patient_id
	) inicio

set t_paciente.emtarv=1,
	t_paciente.datainiciotarv=inicio.data_tarv
where t_paciente.patient_id=inicio.patient_id;

/*Update Inicio Noutro*/
update t_paciente,
	(	Select 	p.patient_id,
				o.value_datetime
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type=18 and o.concept_id=1190
	) inicio

set 	t_paciente.datainiciotarv=inicio.value_datetime
where 	t_paciente.patient_id=inicio.patient_id;

/*Estado Actual TARV*/
update t_paciente,
(select obs.person_id, 
		concept_name.name,
		obs.obs_datetime,
		concept_name.concept_id
from 	openmrs.obs 
		inner join 
		(	select encounter_id,data_ultima_frida.encounter_datetime,data_ultima_frida.patient_id
			from 	openmrs.encounter,
					(	select patient.patient_id,max(encounter_datetime) as encounter_datetime
						from openmrs.encounter inner join openmrs.patient on patient.patient_id=encounter.patient_id
						where 	encounter_type=18 and encounter.voided=0 and patient.voided=0 and encounter_datetime<=dataFinal
						group by patient_id
					) data_ultima_frida
			where 	encounter.encounter_datetime=data_ultima_frida.encounter_datetime and encounter.encounter_type=18 and
				data_ultima_frida.patient_id=encounter.patient_id and encounter.voided=0
		) id_ultimo_levantamento on obs.encounter_id=id_ultimo_levantamento.encounter_id
		inner join openmrs.concept_name on concept_name.concept_id=obs.value_coded
		
where 	concept_name.locale='pt' and 
		obs.concept_id=1708 and obs.voided=0) saida 
		
set 	t_paciente.codestado=saida.name,
		t_paciente.datasaidatarv=saida.obs_datetime
where saida.person_id=t_paciente.patient_id;

/*Estado Actual - Obito Demografico*/
update t_paciente, person
set 	t_paciente.codestado='OBITO',
		t_paciente.datasaidatarv=person.death_date
where person.person_id=t_paciente.patient_id and codestado is null and dead=1;

/*Estado Actual - nao tarv*/
update t_paciente,
		(Select p.patient_id, 
				e.encounter_datetime,
				case o.value_coded
				when 1706 then 'TRANSFERIDO PARA'
				when 1707 then 'ABANDONO'
				else 'OUTRO' end as codSaida
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=6138 and e.encounter_type in (6,9) and e.voided=0 and e.encounter_datetime<=dataFinal
		) saida
set 	t_paciente.codestado=saida.codSaida,
		t_paciente.datasaidatarv=saida.encounter_datetime
where saida.patient_id=t_paciente.patient_id;

/*Data de Diagnostico*/
update t_paciente,
	(	Select 	p.patient_id,
				o.value_datetime
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type in (5,7) and o.concept_id=6123
	) diagnostico

set 	t_paciente.datadiagnostico=diagnostico.value_datetime
where 	t_paciente.patient_id=diagnostico.patient_id;

/*Aconselhado*/
update t_paciente,
	(	Select 	p.patient_id
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type in (5,7) and o.concept_id=1463 and o.value_coded=1065
	) aconselhado
set 	t_paciente.aconselhado=1
where 	t_paciente.patient_id=aconselhado.patient_id;

/*Aceita busca*/
update t_paciente, encounter
set 	t_paciente.aceitabuscaactiva=1,
		t_paciente.dataaceitabuscaactiva=encounter_datetime
where encounter.patient_id=t_paciente.patient_id and encounter_type=30;


/*Update Nome*/
update t_paciente, person_name
set 	t_paciente.nome=concat(person_name.given_name,' ',person_name.middle_name)
where person_name.person_id=t_paciente.patient_id;

/*Update Apelido*/
update t_paciente, person_name
set 	t_paciente.apelido=person_name.family_name
where person_name.person_id=t_paciente.patient_id;

/*Update codFuncionari*/
update t_paciente, person_name
set 	t_paciente.codfuncionario=concat(person_name.given_name,' ',person_name.middle_name,' ',person_name.family_name)
where person_name.person_id=t_paciente.provider_id;

/*Update identificacao*/
update 	t_paciente,
		(	select 	distinct p.patient_id,pd.identifier
			from 	patient_identifier pd 
					inner join patient p on p.patient_id=pd.patient_id 		
			where 	pd.identifier_type=3 and pd.voided=0 and p.voided=0
		) nid
set t_paciente.identificacao=nid.identifier
where nid.patient_id=t_paciente.patient_id;

/*Update Endereco*/
update t_paciente, person_address
set 	t_paciente.coddistrito=person_address.county_district,
		t_paciente.codbairro=person_address.subregion,
		t_paciente.celula=person_address.region,
		t_paciente.avenida=person_address.address1
where person_address.person_id=t_paciente.patient_id;

/*Update Regime*/
update t_paciente,
	(	select inicio.patient_id,inicio.data_tarv,
		case obs.value_coded
				when 792 then 'D4T+3TC+NVP'
				when 1651 then 'AZT+3TC+NVP'
				when 1702 then 'AZT+3TC+NFV'
				when 1827 then 'D4T+3TC+EFV'
				when 1703 then 'AZT+3TC+EFV'
				when 6110 then 'D4T20+3TC+NVP'
				when 6103 then 'D4T+3TC+LPV'
				when 817 then 'ABC+3TC+AZT'
				when 6236 then 'D4T+DDI+RTV-IP'
				when 625 then 'D4T'
				when 631 then 'NVP'
				when 628 then '3TC'
				when 6107 then 'TDF+AZT+3TC+LPV'
				else 'OUTRO' end as name
		from
		(Select 	p.patient_id,
				min(e.encounter_datetime) data_tarv,
				e.encounter_id
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type=18 and o.concept_id=1255 and o.value_coded in (1256,1369)
		group by p.patient_id) inicio
		inner join obs on obs.encounter_id=inicio.encounter_id
		where	inicio.data_tarv=obs.obs_datetime and obs.concept_id=1088 and obs.voided=0
				
	) inicio1

set		t_paciente.codregime=inicio1.name
where	t_paciente.patient_id=inicio1.patient_id;

/*Intervencao Cirurgica*/
update t_paciente,
		(Select p.patient_id, 
				e.encounter_datetime,
				case o.value_coded
				when 1472 then 'Sim'
				when 1066 then 'Nao'
				when 1457 then 'Sem Informação'
				else 'Outro' end as cirurgias
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=1685 and e.encounter_type in (5,7) and e.voided=0
		) cirurgia
set 	t_paciente.cirurgias=cirurgia.cirurgias
where cirurgia.patient_id=t_paciente.patient_id;

/*Intervencao Transfusao*/
update t_paciente,
		(Select p.patient_id, 
				e.encounter_datetime,
				case o.value_coded
				when 1063 then 'Sim'
				when 1066 then 'Nao'
				when 1457 then 'Sem Informação'
				else 'Outro' end as cirurgias
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id=1686 and e.encounter_type in (5,7) and e.voided=0
		) cirurgia
set 	t_paciente.transfusao=cirurgia.cirurgias
where cirurgia.patient_id=t_paciente.patient_id;


/*Update Estadio OMS*/
update t_paciente,
	(	select estadio1.patient_id,
				case obs.value_coded
				when 1204 then 'I'
				when 1205 then 'II'
				when 1206 then 'III'
				when 1207 then 'IV'
				else 'OUTRO' end as nome
				
		from
		(Select p.patient_id,
				min(e.encounter_datetime) data_tarv
		from 	t_paciente p 
				inner join encounter e on p.patient_id=e.patient_id	
				inner join obs o on o.encounter_id=e.encounter_id
		where 	e.voided=0 and o.voided=0 and
				e.encounter_type in (6,9) and o.concept_id=5356
		group by p.patient_id) estadio1
		inner join encounter e on e.patient_id=estadio1.patient_id
		inner join obs on obs.encounter_id=e.encounter_id
		
		where	estadio1.data_tarv=obs.obs_datetime and obs.concept_id=5356 and obs.voided=0 and 
				e.encounter_type in (6,9) and e.voided=0
				
	) estadio

set		t_paciente.estadiooms=estadio.nome
where	t_paciente.patient_id=estadio.patient_id;



/*Tratamento de TB*/
update t_paciente,
		(Select p.patient_id, 
				e.encounter_datetime,
				case o.value_coded
				when 1065 then 1
				when 1066 then 0
				end as tb
		from 	t_paciente p 
				inner join encounter e on e.patient_id=p.patient_id
				inner join obs o on o.encounter_id=e.encounter_id
		where 	o.voided=0 and o.concept_id in (42,5042) and e.encounter_type in (5,7) and e.voided=0
		) tratamentotb
set 	t_paciente.emtratamentotb=tratamentotb.tb
where tratamentotb.patient_id=t_paciente.patient_id;

update t_paciente set hdd='041508' where location_id=48;

update t_paciente set hdd='041507' where location_id=21;

/*Update Ultima Busca
update openmrsreporting.processo_tarv,
	(Select 	patient_id,max(encounter_datetime) encounter_datetime
	from 	openmrs.encounter
	where 	voided=0 and encounter_datetime<=dataFinal and encounter_type=21
	group by patient_id) busca
set processo_tarv.data_busca=busca.encounter_datetime
where busca.patient_id=processo_tarv.patient_id;*/

end $$

DELIMITER ;