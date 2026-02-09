-- ============================================================
-- Script para Popular o Banco de Dados MenuDB
-- Dados de Teste para Plataforma de Gestão de Menu
-- ============================================================

-- ============================================================
-- 1. INSERIR ESTABELECIMENTOS
-- ============================================================

INSERT INTO Estabelecimentos (Id, Nome, Email, Telefone, Endereco, Cidade, Estado, CEP, Descricao, Plano, Status, Ativo)
VALUES 
(NEWID(), 'Café Senador', 'contato@cafesenador.com', '(11) 3456-7890', 'Rua Senador Flaquer, 282', 'Santo André', 'SP', '09010-120', 'Café especializado em bebidas quentes e doces', 'Premium', 'Ativo', 1),
(NEWID(), 'Bar do João', 'contato@bardojoao.com', '(11) 2345-6789', 'Av. Brasil, 1500', 'São Paulo', 'SP', '01310-100', 'Bar tradicional com bebidas e petiscos', 'Profissional', 'Ativo', 1),
(NEWID(), 'Restaurante Sabor Mineiro', 'contato@sabor-mineiro.com', '(31) 3333-3333', 'Rua das Flores, 250', 'Belo Horizonte', 'MG', '30130-100', 'Restaurante com comida mineira tradicional', 'Premium', 'Ativo', 1),
(NEWID(), 'Sorveteria Gelato', 'contato@gelato.com', '(21) 7777-7777', 'Av. Copacabana, 500', 'Rio de Janeiro', 'RJ', '22020-001', 'Sorveteria com sorvetes artesanais', 'Básico', 'Ativo', 1),
(NEWID(), 'Padaria do Bairro', 'contato@padaria.com', '(85) 3333-3333', 'Rua Principal, 100', 'Fortaleza', 'CE', '60010-140', 'Padaria com pães e bolos caseiros', 'Profissional', 'Ativo', 1);

-- ============================================================
-- 2. INSERIR CATEGORIAS
-- ============================================================

DECLARE @est1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Estabelecimentos WHERE Nome = 'Café Senador');
DECLARE @est2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Estabelecimentos WHERE Nome = 'Bar do João');
DECLARE @est3 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Estabelecimentos WHERE Nome = 'Restaurante Sabor Mineiro');
DECLARE @est4 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Estabelecimentos WHERE Nome = 'Sorveteria Gelato');
DECLARE @est5 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Estabelecimentos WHERE Nome = 'Padaria do Bairro');

INSERT INTO Categorias (Id, EstabelecimentoId, Nome, Descricao, Ordem, Ativo)
VALUES 
(NEWID(), @est1, 'Bebidas Quentes', 'Café, chá e bebidas quentes', 1, 1),
(NEWID(), @est1, 'Bebidas Frias', 'Sucos, refrigerantes e bebidas geladas', 2, 1),
(NEWID(), @est1, 'Doces e Bolos', 'Bolos, tortas e sobremesas', 3, 1),
(NEWID(), @est1, 'Salgados', 'Croissants, sanduíches e salgados', 4, 1),
(NEWID(), @est2, 'Bebidas Alcoólicas', 'Cervejas, drinks e destilados', 1, 1),
(NEWID(), @est2, 'Bebidas Não Alcoólicas', 'Refrigerantes, sucos e água', 2, 1),
(NEWID(), @est2, 'Petiscos', 'Batatas fritas, amendoim e petiscos', 3, 1),
(NEWID(), @est3, 'Pratos Principais', 'Carnes, frango e peixe', 1, 1),
(NEWID(), @est3, 'Acompanhamentos', 'Arroz, feijão e legumes', 2, 1),
(NEWID(), @est3, 'Sobremesas', 'Doces e sobremesas', 3, 1),
(NEWID(), @est4, 'Sorvetes', 'Sorvetes diversos', 1, 1),
(NEWID(), @est4, 'Açaí', 'Açaí e frutas', 2, 1),
(NEWID(), @est5, 'Pães', 'Pães diversos', 1, 1),
(NEWID(), @est5, 'Bolos', 'Bolos e broinhas', 2, 1);

-- ============================================================
-- 3. INSERIR SUBCATEGORIAS
-- ============================================================

DECLARE @cat1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Bebidas Quentes' AND EstabelecimentoId = @est1);
DECLARE @cat2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Bebidas Frias' AND EstabelecimentoId = @est1);
DECLARE @cat3 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Doces e Bolos' AND EstabelecimentoId = @est1);
DECLARE @cat4 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Salgados' AND EstabelecimentoId = @est1);
DECLARE @cat5 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Bebidas Alcoólicas' AND EstabelecimentoId = @est2);
DECLARE @cat7 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Petiscos' AND EstabelecimentoId = @est2);
DECLARE @cat8 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Pratos Principais' AND EstabelecimentoId = @est3);
DECLARE @cat9 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Acompanhamentos' AND EstabelecimentoId = @est3);
DECLARE @cat11 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Sorvetes' AND EstabelecimentoId = @est4);
DECLARE @cat12 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Açaí' AND EstabelecimentoId = @est4);
DECLARE @cat13 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Pães' AND EstabelecimentoId = @est5);
DECLARE @cat14 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Bolos' AND EstabelecimentoId = @est5);

INSERT INTO Subcategorias (Id, CategoriaId, EstabelecimentoId, Nome, Descricao, Ordem, Ativo)
VALUES 
(NEWID(), @cat1, @est1, 'Espresso', 'Bebidas à base de espresso', 1, 1),
(NEWID(), @cat1, @est1, 'Café Coado', 'Café tradicional coado', 2, 1),
(NEWID(), @cat2, @est1, 'Sucos', 'Sucos naturais', 1, 1),
(NEWID(), @cat2, @est1, 'Refrigerantes', 'Bebidas geladas', 2, 1),
(NEWID(), @cat3, @est1, 'Bolos', 'Bolos diversos', 1, 1),
(NEWID(), @cat3, @est1, 'Tortas', 'Tortas doces', 2, 1),
(NEWID(), @cat4, @est1, 'Croissants', 'Croissants amanteigados', 1, 1),
(NEWID(), @cat4, @est1, 'Sanduíches', 'Sanduíches diversos', 2, 1),
(NEWID(), @cat5, @est2, 'Cervejas', 'Cervejas nacionais e importadas', 1, 1),
(NEWID(), @cat5, @est2, 'Drinks', 'Drinks especiais', 2, 1),
(NEWID(), @cat7, @est2, 'Fritos', 'Petiscos fritos', 1, 1),
(NEWID(), @cat8, @est3, 'Carnes', 'Pratos com carne', 1, 1),
(NEWID(), @cat8, @est3, 'Frango', 'Pratos com frango', 2, 1),
(NEWID(), @cat9, @est3, 'Arroz e Feijão', 'Acompanhamentos tradicionais', 1, 1),
(NEWID(), @cat11, @est4, 'Sabores Clássicos', 'Sorvetes tradicionais', 1, 1),
(NEWID(), @cat12, @est4, 'Açaí Premium', 'Açaí de qualidade premium', 1, 1),
(NEWID(), @cat13, @est5, 'Pães Franceses', 'Pães franceses frescos', 1, 1),
(NEWID(), @cat14, @est5, 'Bolos Caseiros', 'Bolos feitos em casa', 1, 1);

-- ============================================================
-- 4. INSERIR PRODUTOS
-- ============================================================

DECLARE @subcat1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Espresso');
DECLARE @subcat2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Café Coado');
DECLARE @subcat3 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Sucos');
DECLARE @subcat4 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Refrigerantes');
DECLARE @subcat5 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Bolos');
DECLARE @subcat6 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Tortas');
DECLARE @subcat7 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Croissants');
DECLARE @subcat8 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Sanduíches');
DECLARE @subcat9 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Cervejas');
DECLARE @subcat10 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Drinks');
DECLARE @subcat11 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Fritos');
DECLARE @subcat12 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Carnes');
DECLARE @subcat13 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Frango');
DECLARE @subcat14 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Arroz e Feijão');
DECLARE @subcat15 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Sabores Clássicos');
DECLARE @subcat16 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Açaí Premium');
DECLARE @subcat17 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Pães Franceses');
DECLARE @subcat18 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Subcategorias WHERE Nome = 'Bolos Caseiros');

INSERT INTO Produtos (Id, SubcategoriaId, EstabelecimentoId, Nome, Descricao, Preco, Status, Ordem, Ativo)
VALUES 
-- Café Senador - Espresso
(NEWID(), @subcat1, @est1, 'Café Espresso', 'Espresso clássico italiano', 8.90, 'Ativo', 1, 1),
(NEWID(), @subcat1, @est1, 'Café Duplo', 'Dose dupla de espresso', 13.50, 'Ativo', 2, 1),
(NEWID(), @subcat1, @est1, 'Cappuccino', 'Cappuccino cremoso com espuma', 11.50, 'Ativo', 3, 1),
(NEWID(), @subcat1, @est1, 'Latte', 'Leite vaporizado com espresso', 13.00, 'Ativo', 4, 1),
-- Café Senador - Café Coado
(NEWID(), @subcat2, @est1, 'Café com Leite', 'Café coado com leite vaporizado', 12.50, 'Ativo', 1, 1),
(NEWID(), @subcat2, @est1, 'Café Coado', 'Café coado tradicional', 6.50, 'Ativo', 2, 1),
-- Café Senador - Sucos
(NEWID(), @subcat3, @est1, 'Suco Natural', 'Suco natural de frutas', 10.00, 'Ativo', 1, 1),
(NEWID(), @subcat3, @est1, 'Suco Detox', 'Suco verde detox', 12.00, 'Ativo', 2, 1),
-- Café Senador - Refrigerantes
(NEWID(), @subcat4, @est1, 'Refrigerante', 'Refrigerante 350ml', 6.00, 'Ativo', 1, 1),
(NEWID(), @subcat4, @est1, 'Água Mineral', 'Água mineral 500ml', 4.00, 'Ativo', 2, 1),
-- Café Senador - Bolos
(NEWID(), @subcat5, @est1, 'Bolo de Chocolate', 'Bolo rico em chocolate', 12.90, 'Ativo', 1, 1),
(NEWID(), @subcat5, @est1, 'Bolo de Cenoura', 'Bolo de cenoura com cobertura', 12.90, 'Ativo', 2, 1),
(NEWID(), @subcat5, @est1, 'Brownie', 'Brownie denso e fudgy', 13.50, 'Ativo', 3, 1),
-- Café Senador - Tortas
(NEWID(), @subcat6, @est1, 'Torta de Limão', 'Torta clássica de limão', 16.90, 'Ativo', 1, 1),
(NEWID(), @subcat6, @est1, 'Torta de Morango', 'Torta com morangos frescos', 18.90, 'Ativo', 2, 1),
-- Café Senador - Croissants
(NEWID(), @subcat7, @est1, 'Croissant de Chocolate', 'Croissant amanteigado com chocolate', 18.90, 'Ativo', 1, 1),
(NEWID(), @subcat7, @est1, 'Croissant de Queijo', 'Croissant com queijo derretido', 16.90, 'Ativo', 2, 1),
-- Café Senador - Sanduíches
(NEWID(), @subcat8, @est1, 'Sanduíche Natural', 'Sanduíche com frango e salada', 22.50, 'Ativo', 1, 1),
(NEWID(), @subcat8, @est1, 'Tapioca de Carne Louca', 'Tapioca recheada com carne-louca', 33.90, 'Ativo', 2, 1),
-- Bar do João - Cervejas
(NEWID(), @subcat9, @est2, 'Cerveja Artesanal', 'Cerveja artesanal 500ml', 18.00, 'Ativo', 1, 1),
(NEWID(), @subcat9, @est2, 'Chopp Brahma', 'Chopp gelado 400ml', 15.00, 'Ativo', 2, 1),
-- Bar do João - Drinks
(NEWID(), @subcat10, @est2, 'Caipirinha', 'Caipirinha com cachaça premium', 22.00, 'Ativo', 1, 1),
(NEWID(), @subcat10, @est2, 'Mojito', 'Mojito refrescante', 24.00, 'Ativo', 2, 1),
-- Bar do João - Petiscos
(NEWID(), @subcat11, @est2, 'Batatas Fritas', 'Batatas fritas crocantes', 15.00, 'Ativo', 1, 1),
(NEWID(), @subcat11, @est2, 'Bolinhas de Queijo', 'Bolinhas de queijo frito', 18.00, 'Ativo', 2, 1),
-- Restaurante Sabor Mineiro - Carnes
(NEWID(), @subcat12, @est3, 'Carne de Panela', 'Carne cozida no molho', 48.00, 'Ativo', 1, 1),
(NEWID(), @subcat12, @est3, 'Costela à Mineira', 'Costela assada no forno', 52.00, 'Ativo', 2, 1),
-- Restaurante Sabor Mineiro - Frango
(NEWID(), @subcat13, @est3, 'Frango à Mineira', 'Frango ao molho com quiabo', 42.00, 'Ativo', 1, 1),
(NEWID(), @subcat13, @est3, 'Frango Grelhado', 'Frango grelhado na chapa', 38.00, 'Ativo', 2, 1),
-- Restaurante Sabor Mineiro - Acompanhamentos
(NEWID(), @subcat14, @est3, 'Arroz Branco', 'Arroz branco cozido', 8.00, 'Ativo', 1, 1),
(NEWID(), @subcat14, @est3, 'Feijão Tropeiro', 'Feijão com bacon e linguiça', 12.00, 'Ativo', 2, 1),
-- Sorveteria Gelato - Sorvetes
(NEWID(), @subcat15, @est4, 'Sorvete Chocolate', 'Sorvete de chocolate premium', 12.00, 'Ativo', 1, 1),
(NEWID(), @subcat15, @est4, 'Sorvete Morango', 'Sorvete de morango natural', 12.00, 'Ativo', 2, 1),
(NEWID(), @subcat15, @est4, 'Sorvete Pistache', 'Sorvete de pistache gourmet', 14.00, 'Ativo', 3, 1),
-- Sorveteria Gelato - Açaí
(NEWID(), @subcat16, @est4, 'Açaí Completo', 'Açaí com granola e frutas', 24.50, 'Ativo', 1, 1),
(NEWID(), @subcat16, @est4, 'Açaí com Leite Condensado', 'Açaí com leite condensado', 22.00, 'Ativo', 2, 1),
-- Padaria do Bairro - Pães
(NEWID(), @subcat17, @est5, 'Pão Francês', 'Pão francês quentinho', 1.50, 'Ativo', 1, 1),
(NEWID(), @subcat17, @est5, 'Baguete', 'Baguete francesa', 8.00, 'Ativo', 2, 1),
-- Padaria do Bairro - Bolos
(NEWID(), @subcat18, @est5, 'Bolo de Milho', 'Bolo de milho caseiro', 10.00, 'Ativo', 1, 1),
(NEWID(), @subcat18, @est5, 'Bolo de Fubá', 'Bolo de fubá cremoso', 12.00, 'Ativo', 2, 1);

-- ============================================================
-- 5. INSERIR USUÁRIOS
-- ============================================================

INSERT INTO Usuarios (Id, EstabelecimentoId, Nome, Email, Senha, Perfil, Status, Ativo)
VALUES 
(NEWID(), NULL, 'Admin Master', 'admin@menuplatform.com', 'admin123', 'Admin', 'Ativo', 1),
(NEWID(), @est1, 'João Silva', 'joao@cafesenador.com', 'senha123', 'Gerente', 'Ativo', 1),
(NEWID(), @est1, 'Maria Santos', 'maria@cafesenador.com', 'senha123', 'Operador', 'Ativo', 1),
(NEWID(), @est2, 'Pedro Oliveira', 'pedro@bardojoao.com', 'senha123', 'Gerente', 'Ativo', 1),
(NEWID(), @est3, 'Ana Costa', 'ana@sabor-mineiro.com', 'senha123', 'Gerente', 'Ativo', 1),
(NEWID(), @est4, 'Carlos Mendes', 'carlos@gelato.com', 'senha123', 'Gerente', 'Ativo', 1),
(NEWID(), @est5, 'Lucia Ferreira', 'lucia@padaria.com', 'senha123', 'Gerente', 'Ativo', 1);

-- ============================================================
-- 6. INSERIR CLIENTES
-- ============================================================

INSERT INTO Clientes (Id, EstabelecimentoId, Nome, Email, Telefone, CPF, Status, TotalPedidos, GastoTotal, Ativo)
VALUES 
(NEWID(), @est1, 'João Silva', 'joao.silva@email.com', '(11) 98765-4321', '123.456.789-00', 'Ativo', 0, 0, 1),
(NEWID(), @est1, 'Maria Santos', 'maria.santos@email.com', '(11) 98765-4322', '123.456.789-01', 'Ativo', 0, 0, 1),
(NEWID(), @est1, 'Pedro Costa', 'pedro.costa@email.com', '(11) 98765-4323', '123.456.789-02', 'Ativo', 0, 0, 1),
(NEWID(), @est2, 'Ana Oliveira', 'ana.oliveira@email.com', '(11) 98765-4324', '123.456.789-03', 'Ativo', 0, 0, 1),
(NEWID(), @est2, 'Carlos Mendes', 'carlos.mendes@email.com', '(11) 98765-4325', '123.456.789-04', 'Ativo', 0, 0, 1),
(NEWID(), @est3, 'Lucia Ferreira', 'lucia.ferreira@email.com', '(31) 98765-4326', '123.456.789-05', 'Ativo', 0, 0, 1),
(NEWID(), @est3, 'Roberto Alves', 'roberto.alves@email.com', '(31) 98765-4327', '123.456.789-06', 'Ativo', 0, 0, 1),
(NEWID(), @est4, 'Fernanda Dias', 'fernanda.dias@email.com', '(21) 98765-4328', '123.456.789-07', 'Ativo', 0, 0, 1),
(NEWID(), @est4, 'Gustavo Rocha', 'gustavo.rocha@email.com', '(21) 98765-4329', '123.456.789-08', 'Ativo', 0, 0, 1),
(NEWID(), @est5, 'Helena Lima', 'helena.lima@email.com', '(85) 98765-4330', '123.456.789-09', 'Ativo', 0, 0, 1);

-- ============================================================
-- 7. INSERIR ENDEREÇOS
-- ============================================================

DECLARE @cli1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'João Silva' AND EstabelecimentoId = @est1);
DECLARE @cli2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Maria Santos' AND EstabelecimentoId = @est1);
DECLARE @cli3 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Pedro Costa' AND EstabelecimentoId = @est1);
DECLARE @cli4 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Ana Oliveira' AND EstabelecimentoId = @est2);
DECLARE @cli5 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Carlos Mendes' AND EstabelecimentoId = @est2);
DECLARE @cli6 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Lucia Ferreira' AND EstabelecimentoId = @est3);
DECLARE @cli7 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Roberto Alves' AND EstabelecimentoId = @est3);
DECLARE @cli8 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Fernanda Dias' AND EstabelecimentoId = @est4);
DECLARE @cli9 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Gustavo Rocha' AND EstabelecimentoId = @est4);
DECLARE @cli10 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Nome = 'Helena Lima' AND EstabelecimentoId = @est5);

INSERT INTO Enderecos (Id, ClienteId, Tipo, Endereco, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo)
VALUES 
(NEWID(), @cli1, 'Residencial', 'Rua A', '100', 'Apto 101', 'Centro', 'Santo André', 'SP', '09010-120', 1, 1),
(NEWID(), @cli2, 'Residencial', 'Rua B', '200', 'Apto 202', 'Centro', 'Santo André', 'SP', '09010-130', 1, 1),
(NEWID(), @cli3, 'Comercial', 'Rua C', '300', 'Sala 01', 'Centro', 'São Paulo', 'SP', '01310-100', 1, 1),
(NEWID(), @cli4, 'Residencial', 'Av. Paulista', '1000', 'Apto 1001', 'Bela Vista', 'São Paulo', 'SP', '01311-100', 1, 1),
(NEWID(), @cli5, 'Residencial', 'Rua D', '400', 'Apto 404', 'Consolação', 'São Paulo', 'SP', '01310-200', 1, 1),
(NEWID(), @cli6, 'Residencial', 'Rua E', '500', 'Apto 501', 'Savassi', 'Belo Horizonte', 'MG', '30130-100', 1, 1),
(NEWID(), @cli7, 'Residencial', 'Rua F', '600', 'Apto 602', 'Funcionários', 'Belo Horizonte', 'MG', '30130-200', 1, 1),
(NEWID(), @cli8, 'Residencial', 'Rua G', '700', 'Apto 701', 'Copacabana', 'Rio de Janeiro', 'RJ', '22020-001', 1, 1),
(NEWID(), @cli9, 'Residencial', 'Rua H', '800', 'Apto 802', 'Ipanema', 'Rio de Janeiro', 'RJ', '22410-020', 1, 1),
(NEWID(), @cli10, 'Residencial', 'Rua I', '900', 'Apto 901', 'Centro', 'Fortaleza', 'CE', '60010-140', 1, 1);

-- ============================================================
-- 8. INSERIR PEDIDOS
-- ============================================================

DECLARE @end1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli1);
DECLARE @end2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli2);
DECLARE @end3 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli3);
DECLARE @end4 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli4);
DECLARE @end5 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli5);
DECLARE @end6 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli6);
DECLARE @end7 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli7);
DECLARE @end8 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli8);
DECLARE @end9 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli9);
DECLARE @end10 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Enderecos WHERE ClienteId = @cli10);

INSERT INTO Pedidos (Id, EstabelecimentoId, ClienteId, EnderecoId, NumeroNota, Tipo, Status, Subtotal, Desconto, TaxaEntrega, Total, MetodoPagamento, Observacoes)
VALUES 
(NEWID(), @est1, @cli1, @end1, '001', 'Entrega', 'Entregue', 45.90, 0, 5.00, 50.90, 'Dinheiro', 'Entrega realizada'),
(NEWID(), @est1, @cli2, @end2, '002', 'Entrega', 'Entregue', 67.50, 0, 5.00, 72.50, 'Cartão', 'Cliente satisfeito'),
(NEWID(), @est1, @cli3, @end3, '003', 'Retirada', 'Entregue', 89.00, 0, 0, 89.00, 'Dinheiro', ''),
(NEWID(), @est2, @cli4, @end4, '001', 'Entrega', 'Entregue', 120.00, 10.00, 8.00, 118.00, 'Cartão', 'Pedido completo'),
(NEWID(), @est2, @cli5, @end5, '002', 'Entrega', 'Pendente', 95.50, 0, 8.00, 103.50, 'Cartão', 'Aguardando confirmação'),
(NEWID(), @est3, @cli6, @end6, '001', 'Entrega', 'Entregue', 250.00, 0, 10.00, 260.00, 'Cartão', 'Entrega realizada'),
(NEWID(), @est3, @cli7, @end7, '002', 'Retirada', 'Entregue', 180.00, 0, 0, 180.00, 'Dinheiro', ''),
(NEWID(), @est4, @cli8, @end8, '001', 'Entrega', 'Entregue', 75.00, 5.00, 5.00, 75.00, 'Cartão', 'Cliente satisfeito'),
(NEWID(), @est4, @cli9, @end9, '002', 'Retirada', 'Pendente', 48.50, 0, 0, 48.50, 'Dinheiro', 'Aguardando retirada'),
(NEWID(), @est5, @cli10, @end10, '001', 'Entrega', 'Pendente', 35.00, 0, 5.00, 40.00, 'Dinheiro', 'Pedido novo');

PRINT 'Banco de dados populado com sucesso!'
PRINT 'Estabelecimentos: 5'
PRINT 'Categorias: 14'
PRINT 'Subcategorias: 18'
PRINT 'Produtos: 37'
PRINT 'Usuários: 7'
PRINT 'Clientes: 10'
PRINT 'Endereços: 10'
PRINT 'Pedidos: 10'
