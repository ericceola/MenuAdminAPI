-- =====================================================================
-- Script: Suporte a Variantes e Atributos de Produto
-- Data: 2026-03-12
-- Descrição: Adiciona campo PossuiVariantes na tabela Produtos e cria
--            as tabelas AtributosProduto, AtributosProdutoValores,
--            ProdutoVariantes e ProdutoVariantesValores
-- =====================================================================

-- 1. Adicionar campo PossuiVariantes na tabela Produtos
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Produtos')
    AND name = 'PossuiVariantes'
)
BEGIN
    ALTER TABLE dbo.Produtos
    ADD PossuiVariantes BIT NOT NULL DEFAULT 0;
    PRINT 'Campo PossuiVariantes adicionado à tabela Produtos.';
END
ELSE
BEGIN
    PRINT 'Campo PossuiVariantes já existe na tabela Produtos.';
END
GO

-- 2. Criar tabela AtributosProduto
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.AtributosProduto') AND type = 'U')
BEGIN
    CREATE TABLE dbo.AtributosProduto (
        [Id]              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [Nome]            NVARCHAR(100)    NOT NULL,
        [DataCriacao]     DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [DataAtualizacao] DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [Ativo]           BIT              NOT NULL DEFAULT 1,
        CONSTRAINT PK_AtributosProduto PRIMARY KEY (Id)
    );
    PRINT 'Tabela AtributosProduto criada.';
END
ELSE
BEGIN
    PRINT 'Tabela AtributosProduto já existe.';
END
GO

-- 3. Criar tabela AtributosProdutoValores
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.AtributosProdutoValores') AND type = 'U')
BEGIN
    CREATE TABLE dbo.AtributosProdutoValores (
        [Id]                  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [AtributoProdutoId]   UNIQUEIDENTIFIER NOT NULL,
        [Valor]               NVARCHAR(100)    NOT NULL,
        [DataCriacao]         DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [DataAtualizacao]     DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [Ativo]               BIT              NOT NULL DEFAULT 1,
        CONSTRAINT PK_AtributosProdutoValores PRIMARY KEY (Id),
        CONSTRAINT FK_AtributosProdutoValores_AtributosProduto
            FOREIGN KEY (AtributoProdutoId) REFERENCES dbo.AtributosProduto(Id)
    );
    PRINT 'Tabela AtributosProdutoValores criada.';
END
ELSE
BEGIN
    PRINT 'Tabela AtributosProdutoValores já existe.';
END
GO

-- 4. Criar tabela ProdutoVariantes
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.ProdutoVariantes') AND type = 'U')
BEGIN
    CREATE TABLE dbo.ProdutoVariantes (
        [Id]              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [ProdutoId]       UNIQUEIDENTIFIER NOT NULL,
        [Nome]            NVARCHAR(255)    NOT NULL,
        [SKU]             NVARCHAR(100)    NULL,
        [Preco]           DECIMAL(10,2)    NOT NULL,
        [ImagemUrl]       NVARCHAR(MAX)    NULL,
        [ImagemBlobName]  NVARCHAR(255)    NULL,
        [Ordem]           INT              NOT NULL DEFAULT 0,
        [Status]          NVARCHAR(50)     NOT NULL DEFAULT 'ativo',
        [DataCriacao]     DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [DataAtualizacao] DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        [DataExclusao]    DATETIME2(7)     NULL,
        [Ativo]           BIT              NOT NULL DEFAULT 1,
        CONSTRAINT PK_ProdutoVariantes PRIMARY KEY (Id),
        CONSTRAINT FK_ProdutoVariantes_Produtos
            FOREIGN KEY (ProdutoId) REFERENCES dbo.Produtos(Id)
    );
    PRINT 'Tabela ProdutoVariantes criada.';
END
ELSE
BEGIN
    PRINT 'Tabela ProdutoVariantes já existe.';
END
GO

-- 5. Criar tabela ProdutoVariantesValores
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.ProdutoVariantesValores') AND type = 'U')
BEGIN
    CREATE TABLE dbo.ProdutoVariantesValores (
        [Id]                      UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [ProdutoVarianteId]       UNIQUEIDENTIFIER NOT NULL,
        [AtributoProdutoValorId]  UNIQUEIDENTIFIER NOT NULL,
        [DataCriacao]             DATETIME2(7)     NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_ProdutoVariantesValores PRIMARY KEY (Id),
        CONSTRAINT FK_ProdutoVariantesValores_ProdutoVariantes
            FOREIGN KEY (ProdutoVarianteId) REFERENCES dbo.ProdutoVariantes(Id),
        CONSTRAINT FK_ProdutoVariantesValores_AtributosProdutoValores
            FOREIGN KEY (AtributoProdutoValorId) REFERENCES dbo.AtributosProdutoValores(Id),
        -- Garantir unicidade: mesma variante não pode ter o mesmo valor de atributo duas vezes
        CONSTRAINT UQ_ProdutoVariantesValores UNIQUE (ProdutoVarianteId, AtributoProdutoValorId)
    );
    PRINT 'Tabela ProdutoVariantesValores criada.';
END
ELSE
BEGIN
    PRINT 'Tabela ProdutoVariantesValores já existe.';
END
GO

PRINT 'Script executado com sucesso!';
