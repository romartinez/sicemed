/*
   jueves, 04 de octubre de 201211:07:04 a.m.
   User: 
   Server: .
   Database: Sicemed
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_Consultorio
GO
ALTER TABLE dbo.Consultorios SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_Especialidad
GO
ALTER TABLE dbo.Especialidades SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_Persona_CanceladoPor
GO
ALTER TABLE dbo.Personas SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_PersonaRol_Paciente
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_PersonaRol_Profesional
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_PersonaRol_Secretaria_ReservadoraTurno
GO
ALTER TABLE dbo.Turnos
	DROP CONSTRAINT FK_Turno_PersonaRol_Secretaria_Recepcionista
GO
ALTER TABLE dbo.PersonaRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Turnos
	(
	Id bigint NOT NULL,
	FechaGeneracion datetime NOT NULL,
	FechaTurno datetime NOT NULL,
	DuracionTurno bigint NOT NULL DEFAULT 15,
	FechaIngreso datetime NULL,
	FechaAtencion datetime NULL,
	FechaCancelacion datetime NULL,
	Nota nvarchar(255) NULL,
	MotivoCancelacion nvarchar(255) NULL,
	IpPaciente nvarchar(255) NULL,
	EsTelefonico bit NOT NULL,
	EsSobreTurno bit NOT NULL,
	Estado int NOT NULL,
	FechaEstado datetime NOT NULL,
	PacienteId bigint NOT NULL,
	ProfesionalId bigint NOT NULL,
	SecretariaReservadoraTurnoId bigint NULL,
	SecretariaRecepcionistaId bigint NULL,
	CanceladoPorId bigint NULL,
	EspecialidadId bigint NOT NULL,
	ConsultorioId bigint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Turnos SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.Turnos)
	 EXEC('INSERT INTO dbo.Tmp_Turnos (Id, FechaGeneracion, FechaTurno, FechaIngreso, FechaAtencion, FechaCancelacion, Nota, MotivoCancelacion, IpPaciente, EsTelefonico, Estado, FechaEstado, PacienteId, ProfesionalId, SecretariaReservadoraTurnoId, SecretariaRecepcionistaId, CanceladoPorId, EspecialidadId, ConsultorioId)
		SELECT Id, FechaGeneracion, FechaTurno, FechaIngreso, FechaAtencion, FechaCancelacion, Nota, MotivoCancelacion, IpPaciente, EsTelefonico, Estado, FechaEstado, PacienteId, ProfesionalId, SecretariaReservadoraTurnoId, SecretariaRecepcionistaId, CanceladoPorId, EspecialidadId, ConsultorioId FROM dbo.Turnos WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Turnos
GO
EXECUTE sp_rename N'dbo.Tmp_Turnos', N'Turnos', 'OBJECT' 
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	PK__Turnos__3214EC077F4CAA17 PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_PersonaRol_Paciente FOREIGN KEY
	(
	PacienteId
	) REFERENCES dbo.PersonaRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_PersonaRol_Profesional FOREIGN KEY
	(
	ProfesionalId
	) REFERENCES dbo.PersonaRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_PersonaRol_Secretaria_ReservadoraTurno FOREIGN KEY
	(
	SecretariaReservadoraTurnoId
	) REFERENCES dbo.PersonaRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_PersonaRol_Secretaria_Recepcionista FOREIGN KEY
	(
	SecretariaRecepcionistaId
	) REFERENCES dbo.PersonaRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_Persona_CanceladoPor FOREIGN KEY
	(
	CanceladoPorId
	) REFERENCES dbo.Personas
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_Especialidad FOREIGN KEY
	(
	EspecialidadId
	) REFERENCES dbo.Especialidades
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Turnos ADD CONSTRAINT
	FK_Turno_Consultorio FOREIGN KEY
	(
	ConsultorioId
	) REFERENCES dbo.Consultorios
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
