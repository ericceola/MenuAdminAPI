-- Script para adicionar campos de endereço à tabela Estabelecimentos
-- Data: 2026-02-13
-- Descrição: Adiciona os campos Numero, Complemento e Bairro que estavam faltando

-- Verificar se os campos já existem antes de adicionar
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Estabelecimentos' AND COLUMN_NAME='Numero')
BEGIN
    ALTER TABLE Estabelecimentos
    ADD Numero NVARCHAR(50) NULL;
    PRINT 'Campo Numero adicionado com sucesso.';
END
ELSE
BEGIN
    PRINT 'Campo Numero já existe na tabela.';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Estabelecimentos' AND COLUMN_NAME='Complemento')
BEGIN
    ALTER TABLE Estabelecimentos
    ADD Complemento NVARCHAR(255) NULL;
    PRINT 'Campo Complemento adicionado com sucesso.';
END
ELSE
BEGIN
    PRINT 'Campo Complemento já existe na tabela.';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Estabelecimentos' AND COLUMN_NAME='Bairro')
BEGIN
    ALTER TABLE Estabelecimentos
    ADD Bairro NVARCHAR(100) NULL;
    PRINT 'Campo Bairro adicionado com sucesso.';
END
ELSE
BEGIN
    PRINT 'Campo Bairro já existe na tabela.';
END

-- Exibir a estrutura da tabela após as alterações
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='Estabelecimentos'
ORDER BY ORDINAL_POSITION;
