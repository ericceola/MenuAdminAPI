-- Script para popular os novos campos na tabela Estabelecimentos
-- Atualiza os 3 estabelecimentos existentes com dados realistas

-- Café Senador
UPDATE Estabelecimentos 
SET 
    RazaoSocial = 'Café Senador LTDA',
    CNPJ = '12.345.678/0001-90',
    CEP = '09010-010',
    NomeResponsavel = 'João Silva Santos',
    TelefoneResponsavel = '(11) 98765-4321',
    EhMatriz = 1,
    TemFiliais = 0,
    MatrizId = NULL
WHERE Nome = 'Café Senador';

-- Bar do João
UPDATE Estabelecimentos 
SET 
    RazaoSocial = 'Bar do João ME',
    CNPJ = '23.456.789/0001-01',
    CEP = '01311-100',
    NomeResponsavel = 'João Pereira Costa',
    TelefoneResponsavel = '(11) 99876-5432',
    EhMatriz = 1,
    TemFiliais = 0,
    MatrizId = NULL
WHERE Nome = 'Bar do João';

-- Restaurante Sabor
UPDATE Estabelecimentos 
SET 
    RazaoSocial = 'Restaurante Sabor LTDA',
    CNPJ = '34.567.890/0001-12',
    CEP = '04543-132',
    NomeResponsavel = 'Maria Silva Oliveira',
    TelefoneResponsavel = '(11) 97654-3210',
    EhMatriz = 1,
    TemFiliais = 1,
    MatrizId = NULL
WHERE Nome = 'Restaurante Sabor';

-- Verificar os dados atualizados
SELECT Id, Nome, RazaoSocial, CNPJ, CEP, NomeResponsavel, TelefoneResponsavel, EhMatriz, TemFiliais 
FROM Estabelecimentos 
ORDER BY DataCriacao DESC;
