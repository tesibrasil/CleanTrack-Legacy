/*Cleantrack versione 10.0, nessuna modifica rispetto alla 9.12*/
USE CLEANTRACK
GO

UPDATE CONFIGURAZIONE SET VALORE = '99.99' WHERE CHIAVE = 'db_version';
GO