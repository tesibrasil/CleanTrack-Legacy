INSERT INTO UTENTI (USERNAME, PASSWORD, ID_GRUPPO, DISABILITATO, SCADENZA_PASSWORD, DATA_PASSWORD, PRIMO_LOGIN, TIPO_PASSWORD)
VALUES 
(
'cantel',
'cantel',
(SELECT ID FROM GRUPPI WHERE NOME = 'ADMINISTRATORS'),
0,
9999,
(SELECT REPLACE(CONVERT(VARCHAR, GETDATE(),23),'-','') + REPLACE(CONVERT(VARCHAR, GETDATE(),108),':','')),
0,
0
)