-- Script para adicionar coluna Emoji na tabela Categorias
-- Data: 2026-02-20
-- Descrição: Adiciona coluna para armazenar emoji da categoria

-- Verificar se a coluna já existe
IF NOT EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Categorias' 
    AND COLUMN_NAME = 'Emoji'
)
BEGIN
    -- Adicionar coluna Emoji com valor padrão
    ALTER TABLE [dbo].[Categorias]
    ADD [Emoji] NVARCHAR(10) NULL DEFAULT '📦';
    
    PRINT 'Coluna Emoji adicionada com sucesso na tabela Categorias';
END
ELSE
BEGIN
    PRINT 'Coluna Emoji já existe na tabela Categorias';
END

-- Verificar o resultado
SELECT TOP (10) 
    [Id],
    [EstabelecimentoId],
    [Nome],
    [Emoji],
    [Descricao],
    [Ordem],
    [Ativo],
    [DataCriacao]
FROM [dbo].[Categorias]
ORDER BY [DataCriacao] DESC;
