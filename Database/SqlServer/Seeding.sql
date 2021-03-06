USE CLEANTRACK
GO

--UO DI DEFAULT
INSERT INTO UO (ID, DESCRIZIONE) VALUES(1, 'TO_DEFINE')
--SEDE DI DEFAULT
INSERT INTO SEDIESAME (ID, DESCRIZIONE) VALUES (1, 'TO_DEFINE')
INSERT INTO UO_SEDI (IDUO, IDSEDE) VALUES (1, 1)

-- valori di configurazione
INSERT INTO CONFIGURAZIONE (CHIAVE, VALORE) VALUES ('Attiva trigger compatibilità', '0')
INSERT INTO CONFIGURAZIONE (CHIAVE, VALORE) VALUES ('db_version', '99.99')
INSERT INTO CONFIGURAZIONE (CHIAVE, VALORE) VALUES ('Server Host', 'TO_DEFINE_IP_OR_SERVER_NAME')
INSERT INTO CONFIGURAZIONE (CHIAVE, VALORE) VALUES ('Server Port', '8091')
GO
-- gruppi utente
INSERT INTO GRUPPI (NOME, DESCRIZIONE, PERMESSI, PERMESSI2) VALUES ('ADMINISTRATORS', 'ADMINISTRATORS', 1, 0)
INSERT INTO GRUPPI (NOME, DESCRIZIONE, PERMESSI, PERMESSI2) VALUES ('DEFAULT', 'DEFAULT', 0, 0)
GO
-- stati di defualt
DECLARE @ID INT
DECLARE @DESCRIZIONE VARCHAR(255)
DECLARE @DESCRIZIONEAZIONE VARCHAR(255)
DECLARE @BARCODE VARCHAR(255)
DECLARE @COLORE INT
DECLARE @ELIMINATO BIT
DECLARE @VISIBILEMENU INT
DECLARE @VISIBILELISTA INT
DECLARE @INIZIOCICLO BIT
DECLARE @INIZIOSTERILIZZAZIONE BIT
DECLARE @FINESTERILIZZAZIONE BIT
DECLARE @INIZIOSTOCCAGGIO BIT
DECLARE @FINESTOCCAGGIO BIT
DECLARE @VISIBILELISTADETTAGLIO INT
DECLARE @SCELTARAPIDA VARCHAR(50)
DECLARE @VISIBILEPRINCIPALE INT
-- stato pulito
SET @ID = 1
SET @DESCRIZIONE = 'PULITO'
SET @DESCRIZIONEAZIONE = 'FINE STERILIZZAZIONE'
SET @BARCODE = '00000001'
SET @COLORE = 100
SET @ELIMINATO = 0
SET @VISIBILEMENU = 4
SET @VISIBILELISTA = 4
SET @INIZIOCICLO = 0
SET @INIZIOSTERILIZZAZIONE = 0
SET @FINESTERILIZZAZIONE = 1
SET @INIZIOSTOCCAGGIO = 0
SET @FINESTOCCAGGIO = 0
SET @VISIBILELISTADETTAGLIO = 4
SET @SCELTARAPIDA = 'F00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)

-- stato sporco
SET @ID = 2
SET @DESCRIZIONE = 'SPORCO'
SET @DESCRIZIONEAZIONE = 'SPORCO'
SET @BARCODE = '00000002'
SET @COLORE = 0
SET @ELIMINATO = 0
SET @VISIBILEMENU = 1
SET @VISIBILELISTA = 1
SET @INIZIOCICLO = 1
SET @INIZIOSTERILIZZAZIONE = 0
SET @FINESTERILIZZAZIONE = 0
SET @INIZIOSTOCCAGGIO = 0
SET @FINESTOCCAGGIO = 0
SET @VISIBILELISTADETTAGLIO = 1
SET @SCELTARAPIDA = 'U00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)

-- stato sterilizzazione
SET @ID = 3
SET @DESCRIZIONE = 'STERILIZZAZIONE'
SET @DESCRIZIONEAZIONE = 'INIZIO STERILIZZAZIONE'
SET @BARCODE = '00000003'
SET @COLORE = 60
SET @ELIMINATO = 0
SET @VISIBILEMENU = 3
SET @VISIBILELISTA = 3
SET @INIZIOCICLO = 0
SET @INIZIOSTERILIZZAZIONE = 1
SET @FINESTERILIZZAZIONE = 0
SET @INIZIOSTOCCAGGIO = 0
SET @FINESTOCCAGGIO = 0
SET @VISIBILELISTADETTAGLIO = 3
SET @SCELTARAPIDA = 'I00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)


-- stato lavaggio manuale
SET @ID = 4
SET @DESCRIZIONE = 'LAVAGGIO MANUALE'
SET @DESCRIZIONEAZIONE = 'LAVAGGIO MANUALE'
SET @BARCODE = '00000004'
SET @COLORE = 30
SET @ELIMINATO = 0
SET @VISIBILEMENU = 2
SET @VISIBILELISTA = 2
SET @INIZIOCICLO = 0
SET @INIZIOSTERILIZZAZIONE = 0
SET @FINESTERILIZZAZIONE = 0
SET @INIZIOSTOCCAGGIO = 0
SET @FINESTOCCAGGIO = 0
SET @VISIBILELISTADETTAGLIO = 2
SET @SCELTARAPIDA = 'L00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)

-- stato inizio stoccaggio
SET @ID = 5
SET @DESCRIZIONE = 'INIZIO STOCCAGGIO'
SET @DESCRIZIONEAZIONE = 'INIZIO STOCCAGGIO'
SET @BARCODE = '00000005'
SET @COLORE = 180
SET @ELIMINATO = 0
SET @VISIBILEMENU = 5
SET @VISIBILELISTA = 5
SET @INIZIOCICLO = 0
SET @INIZIOSTERILIZZAZIONE = 0
SET @FINESTERILIZZAZIONE = 0
SET @INIZIOSTOCCAGGIO = 1
SET @FINESTOCCAGGIO = 0
SET @VISIBILELISTADETTAGLIO = 5
SET @SCELTARAPIDA = 'S00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)

-- stato fine stoccaggio
SET @ID = 6
SET @DESCRIZIONE = 'FINE STOCCAGGIO'
SET @DESCRIZIONEAZIONE = 'FINE STOCCAGGIO'
SET @BARCODE = '00000006'
SET @COLORE = 100
SET @ELIMINATO = 0
SET @VISIBILEMENU = 6
SET @VISIBILELISTA = 6
SET @INIZIOCICLO = 0
SET @INIZIOSTERILIZZAZIONE = 0
SET @FINESTERILIZZAZIONE = 0
SET @INIZIOSTOCCAGGIO = 0
SET @FINESTOCCAGGIO = 1
SET @VISIBILELISTADETTAGLIO = 6
SET @SCELTARAPIDA = 'S00'
SET @VISIBILEPRINCIPALE = 0

INSERT INTO STATO (ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, ELIMINATO, VISIBILEMENU, VISIBILELISTA, INIZIOCICLO, INIZIOSTERILIZZAZIONE, 
	FINESTERILIZZAZIONE, INIZIOSTOCCAGGIO, FINESTOCCAGGIO, VISIBILELISTADETTAGLIO, SCELTARAPIDA, VISIBILEPRINCIPALE)
VALUES(@ID, @DESCRIZIONE, @DESCRIZIONEAZIONE, @BARCODE, @COLORE, @ELIMINATO, @VISIBILEMENU, @VISIBILELISTA, @INIZIOCICLO, @INIZIOSTERILIZZAZIONE, 
	 @FINESTERILIZZAZIONE, @INIZIOSTOCCAGGIO, @FINESTOCCAGGIO, @VISIBILELISTADETTAGLIO, @SCELTARAPIDA, @VISIBILEPRINCIPALE)

DECLARE @ID_STATO_PULITO INT
SET @ID_STATO_PULITO = 1
DECLARE @ID_STATO_SPORCO INT
SET @ID_STATO_SPORCO = 2
DECLARE @ID_STATO_STERILIZZAZIONE INT
SET @ID_STATO_STERILIZZAZIONE = 3
DECLARE @ID_STATO_LAVAGGIO_MANUALE INT
SET @ID_STATO_LAVAGGIO_MANUALE = 4
DECLARE @ID_STATO_INIZIO_STOCCAGGIO INT
SET @ID_STATO_INIZIO_STOCCAGGIO = 5
DECLARE @ID_STATO_FINE_STOCCAGGIO INT
SET @ID_STATO_FINE_STOCCAGGIO = 6
DECLARE @IDSEDE INT
SELECT @IDSEDE = MIN(ID) FROM SEDIESAME
INSERT INTO STATOCAMBIO(IDSTATOOLD, IDSTATONEW, IDSEDE) VALUES (@ID_STATO_SPORCO, @ID_STATO_STERILIZZAZIONE, @IDSEDE)
INSERT INTO STATOCAMBIO(IDSTATOOLD, IDSTATONEW, IDSEDE) VALUES (@ID_STATO_STERILIZZAZIONE, @ID_STATO_PULITO, @IDSEDE)
INSERT INTO STATOCAMBIO(IDSTATOOLD, IDSTATONEW, IDSEDE) VALUES (@ID_STATO_PULITO, @ID_STATO_INIZIO_STOCCAGGIO, @IDSEDE)
INSERT INTO STATOCAMBIO(IDSTATOOLD, IDSTATONEW, IDSEDE) VALUES (@ID_STATO_INIZIO_STOCCAGGIO, @ID_STATO_FINE_STOCCAGGIO, @IDSEDE)
INSERT INTO STATOCAMBIO(IDSTATOOLD, IDSTATONEW, IDSEDE) VALUES (@ID_STATO_FINE_STOCCAGGIO, @ID_STATO_SPORCO, @IDSEDE)

GO

-- INSERIMENTO DATI PASSWORD UTENTI ATTUALI PER AMLOGIN
INSERT INTO UTENTI_PASSWORDS (IDUTENTE, DATAORA, PASSWORD, ELIMINATO)
SELECT ID, DATA_PASSWORD, PASSWORD, 0  
FROM UTENTI
WHERE ELIMINATO = 0

