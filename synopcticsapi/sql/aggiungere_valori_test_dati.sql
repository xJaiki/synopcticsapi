-- Inserisci i dati di test per 4 elementi
INSERT INTO SynopticDatas (ElementId, SynopticLayout, Text1, Text2, Text3, Status, LastUpdate)
VALUES 
('Status_12', 'sinoptico_test', 'ABC', 'XYZ', '123', 1, GETDATE()),
('Status_14', 'sinoptico_test', 'DEF', 'UVW', '456', 2, GETDATE()),
('Status_40', 'sinoptico_test', 'GHI', 'RST', '789', 0, GETDATE()),
('Status_42', 'sinoptico_test', 'JKL', 'MNO', '012', 3, GETDATE());