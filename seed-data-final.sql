-- ============================================================
-- Script para Popular o Banco de Dados MenuDB
-- Dados de Teste para Plataforma de Gestão de Menu
-- ============================================================

-- ============================================================
-- 1. INSERIR ESTABELECIMENTOS
-- ============================================================

INSERT INTO Estabelecimentos (Nome, CNPJ, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, Status, Plano, Logo)
VALUES 
('Café Senador', '12.345.678/0001-90', 'contato@cafesenador.com', '(11) 3456-7890', '(11) 96064-6979', 'Rua Senador Flaquer, 282', 'Santo André', 'SP', 'Ativo', 'Premium', '☕'),
('Bar do João', '98.765.432/0001-12', 'contato@bardojoao.com', '(11) 2345-6789', '(11) 98765-4321', 'Av. Brasil, 1500', 'São Paulo', 'SP', 'Ativo', 'Profissional', '🍺'),
('Restaurante Sabor Mineiro', '55.555.555/0001-55', 'contato@sabor-mineiro.com', '(31) 3333-3333', '(31) 99999-9999', 'Rua das Flores, 250', 'Belo Horizonte', 'MG', 'Ativo', 'Premium', '🍽️'),
('Sorveteria Gelato', '77.777.777/0001-77', 'contato@gelato.com', '(21) 7777-7777', '(21) 98888-8888', 'Av. Copacabana, 500', 'Rio de Janeiro', 'RJ', 'Ativo', 'Básico', '🍦'),
('Padaria do Bairro', '33.333.333/0001-33', 'contato@padaria.com', '(85) 3333-3333', '(85) 99999-9999', 'Rua Principal, 100', 'Fortaleza', 'CE', 'Ativo', 'Profissional', '🥐');

-- ============================================================
-- 2. INSERIR CATEGORIAS
-- ============================================================

DECLARE @est1 UNIQUEIDENTIFIER = (SELECT Id FROM Estabelecimentos WHERE Nome = 'Café Senador');
DECLARE @est2 UNIQUEIDENTIFIER = (SELECT Id FROM Estabelecimentos WHERE Nome = 'Bar do João');
DECLARE @est3 UNIQUEIDENTIFIER = (SELECT Id FROM Estabelecimentos WHERE Nome = 'Restaurante Sabor Mineiro');
DECLARE @est4 UNIQUEIDENTIFIER = (SELECT Id FROM Estabelecimentos WHERE Nome = 'Sorveteria Gelato');
DECLARE @est5 UNIQUEIDENTIFIER = (SELECT Id FROM Estabelecimentos WHERE Nome = 'Padaria do Bairro');

INSERT INTO Categorias (EstabelecimentoId, Nome, Descricao, Ativo)
VALUES 
(@est1, 'Bebidas Quentes', 'Café, chá e bebidas quentes', 1),
(@est1, 'Bebidas Frias', 'Sucos, refrigerantes e bebidas geladas', 1),
(@est1, 'Doces e Bolos', 'Bolos, tortas e sobremesas', 1),
(@est1, 'Salgados', 'Croissants, sanduíches e salgados', 1),
(@est2, 'Bebidas Alcoólicas', 'Cervejas, drinks e destilados', 1),
(@est2, 'Bebidas Não Alcoólicas', 'Refrigerantes, sucos e água', 1),
(@est2, 'Petiscos', 'Batatas fritas, amendoim e petiscos', 1),
(@est3, 'Pratos Principais', 'Carnes, frango e peixe', 1),
(@est3, 'Acompanhamentos', 'Arroz, feijão e legumes', 1),
(@est3, 'Sobremesas', 'Doces e sobremesas', 1),
(@est4, 'Sorvetes', 'Sorvetes diversos', 1),
(@est4, 'Açaí', 'Açaí e frutas', 1),
(@est5, 'Pães', 'Pães diversos', 1),
(@est5, 'Bolos', 'Bolos e broinhas', 1);

-- ============================================================
-- 3. INSERIR PRODUTOS
-- ============================================================

DECLARE @cat1 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Bebidas Quentes' AND EstabelecimentoId = @est1);
DECLARE @cat2 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Bebidas Frias' AND EstabelecimentoId = @est1);
DECLARE @cat3 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Doces e Bolos' AND EstabelecimentoId = @est1);
DECLARE @cat4 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Salgados' AND EstabelecimentoId = @est1);
DECLARE @cat5 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Bebidas Alcoólicas' AND EstabelecimentoId = @est2);
DECLARE @cat7 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Petiscos' AND EstabelecimentoId = @est2);
DECLARE @cat8 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Pratos Principais' AND EstabelecimentoId = @est3);
DECLARE @cat9 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Acompanhamentos' AND EstabelecimentoId = @est3);
DECLARE @cat11 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Sorvetes' AND EstabelecimentoId = @est4);
DECLARE @cat12 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Açaí' AND EstabelecimentoId = @est4);
DECLARE @cat13 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Pães' AND EstabelecimentoId = @est5);
DECLARE @cat14 UNIQUEIDENTIFIER = (SELECT Id FROM Categorias WHERE Nome = 'Bolos' AND EstabelecimentoId = @est5);

INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel)
VALUES 
-- Café Senador - Bebidas Quentes
(@est1, @cat1, 'Café Espresso', 'Espresso clássico italiano', 8.90, 1),
(@est1, @cat1, 'Café Duplo', 'Dose dupla de espresso', 13.50, 1),
(@est1, @cat1, 'Café com Leite', 'Café coado com leite vaporizado', 12.50, 1),
(@est1, @cat1, 'Cappuccino', 'Cappuccino cremoso com espuma', 11.50, 1),
(@est1, @cat1, 'Latte', 'Leite vaporizado com espresso', 13.00, 1),
-- Café Senador - Bebidas Frias
(@est1, @cat2, 'Café Gelado', 'Café coado servido gelado', 9.50, 1),
(@est1, @cat2, 'Suco Natural', 'Suco natural de frutas', 10.00, 1),
(@est1, @cat2, 'Refrigerante', 'Refrigerante 350ml', 6.00, 1),
-- Café Senador - Doces e Bolos
(@est1, @cat3, 'Bolo de Chocolate', 'Bolo rico em chocolate', 12.90, 1),
(@est1, @cat3, 'Bolo de Cenoura', 'Bolo de cenoura com cobertura', 12.90, 1),
(@est1, @cat3, 'Brownie', 'Brownie denso e fudgy', 13.50, 1),
(@est1, @cat3, 'Torta de Limão', 'Torta clássica de limão', 16.90, 1),
-- Café Senador - Salgados
(@est1, @cat4, 'Croissant de Chocolate', 'Croissant amanteigado com chocolate', 18.90, 1),
(@est1, @cat4, 'Croissant de Queijo', 'Croissant com queijo derretido', 16.90, 1),
(@est1, @cat4, 'Sanduíche Natural', 'Sanduíche com frango e salada', 22.50, 1),
(@est1, @cat4, 'Tapioca de Carne Louca', 'Tapioca recheada com carne-louca', 33.90, 1),
-- Bar do João - Bebidas Alcoólicas
(@est2, @cat5, 'Cerveja Artesanal', 'Cerveja artesanal 500ml', 18.00, 1),
(@est2, @cat5, 'Chopp Brahma', 'Chopp gelado 400ml', 15.00, 1),
(@est2, @cat5, 'Caipirinha', 'Caipirinha com cachaça premium', 22.00, 1),
(@est2, @cat5, 'Mojito', 'Mojito refrescante', 24.00, 1),
-- Bar do João - Petiscos
(@est2, @cat7, 'Batatas Fritas', 'Batatas fritas crocantes', 15.00, 1),
(@est2, @cat7, 'Amendoim Salgado', 'Amendoim torrado e salgado', 12.00, 1),
(@est2, @cat7, 'Bolinhas de Queijo', 'Bolinhas de queijo frito', 18.00, 1),
-- Restaurante Sabor Mineiro - Pratos Principais
(@est3, @cat8, 'Frango à Mineira', 'Frango ao molho com quiabo', 42.00, 1),
(@est3, @cat8, 'Carne de Panela', 'Carne cozida no molho', 48.00, 1),
(@est3, @cat8, 'Peixe Grelhado', 'Peixe fresco grelhado', 55.00, 1),
(@est3, @cat8, 'Costela à Mineira', 'Costela assada no forno', 52.00, 1),
-- Restaurante Sabor Mineiro - Acompanhamentos
(@est3, @cat9, 'Arroz Branco', 'Arroz branco cozido', 8.00, 1),
(@est3, @cat9, 'Feijão Tropeiro', 'Feijão com bacon e linguiça', 12.00, 1),
(@est3, @cat9, 'Salada Verde', 'Salada fresca com alface e tomate', 15.00, 1),
-- Sorveteria Gelato - Sorvetes
(@est4, @cat11, 'Sorvete Chocolate', 'Sorvete de chocolate premium', 12.00, 1),
(@est4, @cat11, 'Sorvete Morango', 'Sorvete de morango natural', 12.00, 1),
(@est4, @cat11, 'Sorvete Pistache', 'Sorvete de pistache gourmet', 14.00, 1),
(@est4, @cat11, 'Sorvete Baunilha', 'Sorvete de baunilha clássico', 10.00, 1),
-- Sorveteria Gelato - Açaí
(@est4, @cat12, 'Açaí Completo', 'Açaí com granola e frutas', 24.50, 1),
(@est4, @cat12, 'Açaí com Leite Condensado', 'Açaí com leite condensado', 22.00, 1),
-- Padaria do Bairro - Pães
(@est5, @cat13, 'Pão Francês', 'Pão francês quentinho', 1.50, 1),
(@est5, @cat13, 'Pão de Queijo', 'Pão de queijo caseiro', 4.00, 1),
(@est5, @cat13, 'Baguete', 'Baguete francesa', 8.00, 1),
-- Padaria do Bairro - Bolos
(@est5, @cat14, 'Bolo de Milho', 'Bolo de milho caseiro', 10.00, 1),
(@est5, @cat14, 'Bolo de Fubá', 'Bolo de fubá cremoso', 12.00, 1),
(@est5, @cat14, 'Broinhas de Chuva', 'Broinhas doces', 2.50, 1);

-- ============================================================
-- 4. INSERIR USUÁRIOS
-- ============================================================

INSERT INTO Usuarios (EstabelecimentoId, Nome, Email, Telefone, Funcao, Ativo)
VALUES 
(NULL, 'Admin Master', 'admin@menuplatform.com', '(11) 99999-9999', 'Admin Master', 1),
(@est1, 'João Silva', 'joao@cafesenador.com', '(11) 96064-6979', 'Gerente', 1),
(@est1, 'Maria Santos', 'maria@cafesenador.com', '(11) 96064-6980', 'Operador', 1),
(@est2, 'Pedro Oliveira', 'pedro@bardojoao.com', '(11) 98765-4321', 'Gerente', 1),
(@est3, 'Ana Costa', 'ana@sabor-mineiro.com', '(31) 99999-9999', 'Gerente', 1),
(@est4, 'Carlos Mendes', 'carlos@gelato.com', '(21) 98888-8888', 'Gerente', 1),
(@est5, 'Lucia Ferreira', 'lucia@padaria.com', '(85) 99999-9999', 'Gerente', 1);

-- ============================================================
-- 5. INSERIR CLIENTES
-- ============================================================

INSERT INTO Clientes (EstabelecimentoId, Nome, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, Status)
VALUES 
(@est1, 'João Silva', 'joao.silva@email.com', '(11) 98765-4321', '(11) 98765-4321', 'Rua A, 100', 'Santo André', 'SP', 'Ativo'),
(@est1, 'Maria Santos', 'maria.santos@email.com', '(11) 98765-4322', '(11) 98765-4322', 'Rua B, 200', 'Santo André', 'SP', 'Ativo'),
(@est1, 'Pedro Costa', 'pedro.costa@email.com', '(11) 98765-4323', '(11) 98765-4323', 'Rua C, 300', 'São Paulo', 'SP', 'Ativo'),
(@est2, 'Ana Oliveira', 'ana.oliveira@email.com', '(11) 98765-4324', '(11) 98765-4324', 'Av. Paulista, 1000', 'São Paulo', 'SP', 'Ativo'),
(@est2, 'Carlos Mendes', 'carlos.mendes@email.com', '(11) 98765-4325', '(11) 98765-4325', 'Rua D, 400', 'São Paulo', 'SP', 'Ativo'),
(@est3, 'Lucia Ferreira', 'lucia.ferreira@email.com', '(31) 98765-4326', '(31) 98765-4326', 'Rua E, 500', 'Belo Horizonte', 'MG', 'Ativo'),
(@est3, 'Roberto Alves', 'roberto.alves@email.com', '(31) 98765-4327', '(31) 98765-4327', 'Rua F, 600', 'Belo Horizonte', 'MG', 'Ativo'),
(@est4, 'Fernanda Dias', 'fernanda.dias@email.com', '(21) 98765-4328', '(21) 98765-4328', 'Rua G, 700', 'Rio de Janeiro', 'RJ', 'Ativo'),
(@est4, 'Gustavo Rocha', 'gustavo.rocha@email.com', '(21) 98765-4329', '(21) 98765-4329', 'Rua H, 800', 'Rio de Janeiro', 'RJ', 'Ativo'),
(@est5, 'Helena Lima', 'helena.lima@email.com', '(85) 98765-4330', '(85) 98765-4330', 'Rua I, 900', 'Fortaleza', 'CE', 'Ativo');

-- ============================================================
-- 6. INSERIR PEDIDOS
-- ============================================================

INSERT INTO Pedidos (EstabelecimentoId, ClienteId, Status, Total, Observacoes)
SELECT @est1, Id, 'Entregue', 45.90, 'Entrega realizada' FROM Clientes WHERE EstabelecimentoId = @est1 AND Nome = 'João Silva'
UNION ALL
SELECT @est1, Id, 'Entregue', 67.50, 'Cliente satisfeito' FROM Clientes WHERE EstabelecimentoId = @est1 AND Nome = 'Maria Santos'
UNION ALL
SELECT @est1, Id, 'Entregue', 89.00, '' FROM Clientes WHERE EstabelecimentoId = @est1 AND Nome = 'Pedro Costa'
UNION ALL
SELECT @est2, Id, 'Entregue', 120.00, 'Pedido completo' FROM Clientes WHERE EstabelecimentoId = @est2 AND Nome = 'Ana Oliveira'
UNION ALL
SELECT @est2, Id, 'Pendente', 95.50, 'Aguardando confirmação' FROM Clientes WHERE EstabelecimentoId = @est2 AND Nome = 'Carlos Mendes'
UNION ALL
SELECT @est3, Id, 'Entregue', 250.00, 'Entrega realizada' FROM Clientes WHERE EstabelecimentoId = @est3 AND Nome = 'Lucia Ferreira'
UNION ALL
SELECT @est3, Id, 'Entregue', 180.00, '' FROM Clientes WHERE EstabelecimentoId = @est3 AND Nome = 'Roberto Alves'
UNION ALL
SELECT @est4, Id, 'Entregue', 75.00, 'Cliente satisfeito' FROM Clientes WHERE EstabelecimentoId = @est4 AND Nome = 'Fernanda Dias'
UNION ALL
SELECT @est4, Id, 'Pendente', 48.50, 'Aguardando retirada' FROM Clientes WHERE EstabelecimentoId = @est4 AND Nome = 'Gustavo Rocha'
UNION ALL
SELECT @est5, Id, 'Pendente', 35.00, 'Pedido novo' FROM Clientes WHERE EstabelecimentoId = @est5 AND Nome = 'Helena Lima';

PRINT 'Banco de dados populado com sucesso!'
