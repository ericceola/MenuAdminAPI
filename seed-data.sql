-- ============================================================
-- Script para Popular o Banco de Dados MenuDB
-- Dados de Teste para Plataforma de Gestão de Menu
-- ============================================================

-- Limpar dados existentes (comentado por segurança)
-- DELETE FROM Pedidos;
-- DELETE FROM ItensPedido;
-- DELETE FROM Produtos;
-- DELETE FROM Categorias;
-- DELETE FROM Clientes;
-- DELETE FROM Usuarios;
-- DELETE FROM Estabelecimentos;

-- ============================================================
-- 1. INSERIR ESTABELECIMENTOS
-- ============================================================

INSERT INTO Estabelecimentos (Nome, CNPJ, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, DataCadastro, Status, Plano, Logo)
VALUES 
('Café Senador', '12.345.678/0001-90', 'contato@cafesenador.com', '(11) 3456-7890', '(11) 96064-6979', 'Rua Senador Flaquer, 282', 'Santo André', 'SP', GETDATE(), 'Ativo', 'Premium', '☕'),
('Bar do João', '98.765.432/0001-12', 'contato@bardojoao.com', '(11) 2345-6789', '(11) 98765-4321', 'Av. Brasil, 1500', 'São Paulo', 'SP', GETDATE(), 'Ativo', 'Profissional', '🍺'),
('Restaurante Sabor Mineiro', '55.555.555/0001-55', 'contato@sabor-mineiro.com', '(31) 3333-3333', '(31) 99999-9999', 'Rua das Flores, 250', 'Belo Horizonte', 'MG', GETDATE(), 'Ativo', 'Premium', '🍽️'),
('Sorveteria Gelato', '77.777.777/0001-77', 'contato@gelato.com', '(21) 7777-7777', '(21) 98888-8888', 'Av. Copacabana, 500', 'Rio de Janeiro', 'RJ', GETDATE(), 'Ativo', 'Básico', '🍦'),
('Padaria do Bairro', '33.333.333/0001-33', 'contato@padaria.com', '(85) 3333-3333', '(85) 99999-9999', 'Rua Principal, 100', 'Fortaleza', 'CE', GETDATE(), 'Ativo', 'Profissional', '🥐');

-- ============================================================
-- 2. INSERIR CATEGORIAS
-- ============================================================

INSERT INTO Categorias (EstabelecimentoId, Nome, Descricao, Ativo)
VALUES 
(1, 'Bebidas Quentes', 'Café, chá e bebidas quentes', 1),
(1, 'Bebidas Frias', 'Sucos, refrigerantes e bebidas geladas', 1),
(1, 'Doces e Bolos', 'Bolos, tortas e sobremesas', 1),
(1, 'Salgados', 'Croissants, sanduíches e salgados', 1),
(2, 'Bebidas Alcoólicas', 'Cervejas, drinks e destilados', 1),
(2, 'Bebidas Não Alcoólicas', 'Refrigerantes, sucos e água', 1),
(2, 'Petiscos', 'Batatas fritas, amendoim e petiscos', 1),
(3, 'Pratos Principais', 'Carnes, frango e peixe', 1),
(3, 'Acompanhamentos', 'Arroz, feijão e legumes', 1),
(3, 'Sobremesas', 'Doces e sobremesas', 1),
(4, 'Sorvetes', 'Sorvetes diversos', 1),
(4, 'Açaí', 'Açaí e frutas', 1),
(5, 'Pães', 'Pães diversos', 1),
(5, 'Bolos', 'Bolos e broinhas', 1);

-- ============================================================
-- 3. INSERIR PRODUTOS
-- ============================================================

-- Café Senador - Bebidas Quentes
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(1, 1, 'Café Espresso', 'Espresso clássico italiano', 8.90, 1, '☕', GETDATE()),
(1, 1, 'Café Duplo', 'Dose dupla de espresso', 13.50, 1, '☕', GETDATE()),
(1, 1, 'Café com Leite', 'Café coado com leite vaporizado', 12.50, 1, '☕', GETDATE()),
(1, 1, 'Cappuccino', 'Cappuccino cremoso com espuma', 11.50, 1, '☕', GETDATE()),
(1, 1, 'Latte', 'Leite vaporizado com espresso', 13.00, 1, '☕', GETDATE());

-- Café Senador - Bebidas Frias
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(1, 2, 'Café Gelado', 'Café coado servido gelado', 9.50, 1, '🧊', GETDATE()),
(1, 2, 'Suco Natural', 'Suco natural de frutas', 10.00, 1, '🧃', GETDATE()),
(1, 2, 'Refrigerante', 'Refrigerante 350ml', 6.00, 1, '🥤', GETDATE());

-- Café Senador - Doces e Bolos
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(1, 3, 'Bolo de Chocolate', 'Bolo rico em chocolate', 12.90, 1, '🍰', GETDATE()),
(1, 3, 'Bolo de Cenoura', 'Bolo de cenoura com cobertura', 12.90, 1, '🍰', GETDATE()),
(1, 3, 'Brownie', 'Brownie denso e fudgy', 13.50, 1, '🍫', GETDATE()),
(1, 3, 'Torta de Limão', 'Torta clássica de limão', 16.90, 1, '🍰', GETDATE());

-- Café Senador - Salgados
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(1, 4, 'Croissant de Chocolate', 'Croissant amanteigado com chocolate', 18.90, 1, '🥐', GETDATE()),
(1, 4, 'Croissant de Queijo', 'Croissant com queijo derretido', 16.90, 1, '🥐', GETDATE()),
(1, 4, 'Sanduíche Natural', 'Sanduíche com frango e salada', 22.50, 1, '🥪', GETDATE()),
(1, 4, 'Tapioca de Carne Louca', 'Tapioca recheada com carne-louca', 33.90, 1, '🥞', GETDATE());

-- Bar do João - Bebidas Alcoólicas
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(2, 5, 'Cerveja Artesanal', 'Cerveja artesanal 500ml', 18.00, 1, '🍺', GETDATE()),
(2, 5, 'Chopp Brahma', 'Chopp gelado 400ml', 15.00, 1, '🍺', GETDATE()),
(2, 5, 'Caipirinha', 'Caipirinha com cachaça premium', 22.00, 1, '🍹', GETDATE()),
(2, 5, 'Mojito', 'Mojito refrescante', 24.00, 1, '🍹', GETDATE());

-- Bar do João - Petiscos
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(2, 7, 'Batatas Fritas', 'Batatas fritas crocantes', 15.00, 1, '🍟', GETDATE()),
(2, 7, 'Amendoim Salgado', 'Amendoim torrado e salgado', 12.00, 1, '🥜', GETDATE()),
(2, 7, 'Bolinhas de Queijo', 'Bolinhas de queijo frito', 18.00, 1, '🧀', GETDATE());

-- Restaurante Sabor Mineiro - Pratos Principais
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(3, 8, 'Frango à Mineira', 'Frango ao molho com quiabo', 42.00, 1, '🍖', GETDATE()),
(3, 8, 'Carne de Panela', 'Carne cozida no molho', 48.00, 1, '🍖', GETDATE()),
(3, 8, 'Peixe Grelhado', 'Peixe fresco grelhado', 55.00, 1, '🐟', GETDATE()),
(3, 8, 'Costela à Mineira', 'Costela assada no forno', 52.00, 1, '🍖', GETDATE());

-- Restaurante Sabor Mineiro - Acompanhamentos
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(3, 9, 'Arroz Branco', 'Arroz branco cozido', 8.00, 1, '🍚', GETDATE()),
(3, 9, 'Feijão Tropeiro', 'Feijão com bacon e linguiça', 12.00, 1, '🫘', GETDATE()),
(3, 9, 'Salada Verde', 'Salada fresca com alface e tomate', 15.00, 1, '🥗', GETDATE());

-- Sorveteria Gelato - Sorvetes
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(4, 11, 'Sorvete Chocolate', 'Sorvete de chocolate premium', 12.00, 1, '🍦', GETDATE()),
(4, 11, 'Sorvete Morango', 'Sorvete de morango natural', 12.00, 1, '🍓', GETDATE()),
(4, 11, 'Sorvete Pistache', 'Sorvete de pistache gourmet', 14.00, 1, '🍦', GETDATE()),
(4, 11, 'Sorvete Baunilha', 'Sorvete de baunilha clássico', 10.00, 1, '🍦', GETDATE());

-- Sorveteria Gelato - Açaí
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(4, 12, 'Açaí Completo', 'Açaí com granola e frutas', 24.50, 1, '🫐', GETDATE()),
(4, 12, 'Açaí com Leite Condensado', 'Açaí com leite condensado', 22.00, 1, '🫐', GETDATE());

-- Padaria do Bairro - Pães
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(5, 13, 'Pão Francês', 'Pão francês quentinho', 1.50, 1, '🍞', GETDATE()),
(5, 13, 'Pão de Queijo', 'Pão de queijo caseiro', 4.00, 1, '🥐', GETDATE()),
(5, 13, 'Baguete', 'Baguete francesa', 8.00, 1, '🥖', GETDATE());

-- Padaria do Bairro - Bolos
INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel, Imagem, DataCadastro)
VALUES 
(5, 14, 'Bolo de Milho', 'Bolo de milho caseiro', 10.00, 1, '🧁', GETDATE()),
(5, 14, 'Bolo de Fubá', 'Bolo de fubá cremoso', 12.00, 1, '🧁', GETDATE()),
(5, 14, 'Broinhas de Chuva', 'Broinhas doces', 2.50, 1, '🧁', GETDATE());

-- ============================================================
-- 4. INSERIR USUÁRIOS
-- ============================================================

INSERT INTO Usuarios (EstabelecimentoId, Nome, Email, Senha, Telefone, Funcao, Ativo, DataCadastro)
VALUES 
(NULL, 'Admin Master', 'admin@menuplatform.com', 'hashed_password_admin123', '(11) 99999-9999', 'Admin Master', 1, GETDATE()),
(1, 'João Silva', 'joao@cafesenador.com', 'hashed_password_joao123', '(11) 96064-6979', 'Gerente', 1, GETDATE()),
(1, 'Maria Santos', 'maria@cafesenador.com', 'hashed_password_maria123', '(11) 96064-6980', 'Operador', 1, GETDATE()),
(2, 'Pedro Oliveira', 'pedro@bardojoao.com', 'hashed_password_pedro123', '(11) 98765-4321', 'Gerente', 1, GETDATE()),
(3, 'Ana Costa', 'ana@sabor-mineiro.com', 'hashed_password_ana123', '(31) 99999-9999', 'Gerente', 1, GETDATE()),
(4, 'Carlos Mendes', 'carlos@gelato.com', 'hashed_password_carlos123', '(21) 98888-8888', 'Gerente', 1, GETDATE()),
(5, 'Lucia Ferreira', 'lucia@padaria.com', 'hashed_password_lucia123', '(85) 99999-9999', 'Gerente', 1, GETDATE());

-- ============================================================
-- 5. INSERIR CLIENTES
-- ============================================================

INSERT INTO Clientes (EstabelecimentoId, Nome, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, DataCadastro, Status)
VALUES 
(1, 'João Silva', 'joao.silva@email.com', '(11) 98765-4321', '(11) 98765-4321', 'Rua A, 100', 'Santo André', 'SP', GETDATE(), 'Ativo'),
(1, 'Maria Santos', 'maria.santos@email.com', '(11) 98765-4322', '(11) 98765-4322', 'Rua B, 200', 'Santo André', 'SP', GETDATE(), 'Ativo'),
(1, 'Pedro Costa', 'pedro.costa@email.com', '(11) 98765-4323', '(11) 98765-4323', 'Rua C, 300', 'São Paulo', 'SP', GETDATE(), 'Ativo'),
(2, 'Ana Oliveira', 'ana.oliveira@email.com', '(11) 98765-4324', '(11) 98765-4324', 'Av. Paulista, 1000', 'São Paulo', 'SP', GETDATE(), 'Ativo'),
(2, 'Carlos Mendes', 'carlos.mendes@email.com', '(11) 98765-4325', '(11) 98765-4325', 'Rua D, 400', 'São Paulo', 'SP', GETDATE(), 'Ativo'),
(3, 'Lucia Ferreira', 'lucia.ferreira@email.com', '(31) 98765-4326', '(31) 98765-4326', 'Rua E, 500', 'Belo Horizonte', 'MG', GETDATE(), 'Ativo'),
(3, 'Roberto Alves', 'roberto.alves@email.com', '(31) 98765-4327', '(31) 98765-4327', 'Rua F, 600', 'Belo Horizonte', 'MG', GETDATE(), 'Ativo'),
(4, 'Fernanda Dias', 'fernanda.dias@email.com', '(21) 98765-4328', '(21) 98765-4328', 'Rua G, 700', 'Rio de Janeiro', 'RJ', GETDATE(), 'Ativo'),
(4, 'Gustavo Rocha', 'gustavo.rocha@email.com', '(21) 98765-4329', '(21) 98765-4329', 'Rua H, 800', 'Rio de Janeiro', 'RJ', GETDATE(), 'Ativo'),
(5, 'Helena Lima', 'helena.lima@email.com', '(85) 98765-4330', '(85) 98765-4330', 'Rua I, 900', 'Fortaleza', 'CE', GETDATE(), 'Ativo');

-- ============================================================
-- 6. INSERIR PEDIDOS
-- ============================================================

INSERT INTO Pedidos (EstabelecimentoId, ClienteId, DataPedido, Status, Total, Observacoes)
VALUES 
(1, 1, DATEADD(DAY, -5, GETDATE()), 'Entregue', 45.90, 'Entrega realizada'),
(1, 2, DATEADD(DAY, -4, GETDATE()), 'Entregue', 67.50, 'Cliente satisfeito'),
(1, 3, DATEADD(DAY, -3, GETDATE()), 'Entregue', 89.00, ''),
(2, 4, DATEADD(DAY, -2, GETDATE()), 'Entregue', 120.00, 'Pedido completo'),
(2, 5, DATEADD(DAY, -1, GETDATE()), 'Pendente', 95.50, 'Aguardando confirmação'),
(3, 6, DATEADD(DAY, -5, GETDATE()), 'Entregue', 250.00, 'Entrega realizada'),
(3, 7, DATEADD(DAY, -3, GETDATE()), 'Entregue', 180.00, ''),
(4, 8, DATEADD(DAY, -2, GETDATE()), 'Entregue', 75.00, 'Cliente satisfeito'),
(4, 9, DATEADD(DAY, -1, GETDATE()), 'Pendente', 48.50, 'Aguardando retirada'),
(5, 10, GETDATE(), 'Pendente', 35.00, 'Pedido novo');

-- ============================================================
-- 7. INSERIR ITENS DE PEDIDO
-- ============================================================

INSERT INTO ItensPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario, Subtotal)
VALUES 
-- Pedido 1
(1, 1, 2, 8.90, 17.80),
(1, 2, 1, 13.50, 13.50),
(1, 9, 1, 12.90, 12.90),
-- Pedido 2
(2, 3, 1, 12.50, 12.50),
(2, 4, 2, 11.50, 23.00),
(2, 10, 1, 16.90, 16.90),
(2, 11, 1, 13.50, 13.50),
-- Pedido 3
(3, 5, 2, 13.00, 26.00),
(3, 7, 1, 10.00, 10.00),
(3, 8, 2, 6.00, 12.00),
(3, 16, 1, 22.00, 22.00),
(3, 17, 1, 18.00, 18.00),
-- Pedido 4
(4, 19, 1, 15.00, 15.00),
(4, 20, 2, 18.00, 36.00),
(4, 21, 1, 24.00, 24.00),
(4, 22, 1, 15.00, 15.00),
(4, 23, 1, 12.00, 12.00),
(4, 24, 1, 18.00, 18.00),
-- Pedido 5
(5, 25, 1, 42.00, 42.00),
(5, 28, 1, 8.00, 8.00),
(5, 29, 1, 12.00, 12.00),
(5, 30, 1, 15.00, 15.00),
(5, 31, 1, 12.00, 12.00),
-- Pedido 6
(6, 32, 1, 55.00, 55.00),
(6, 28, 2, 8.00, 16.00),
(6, 29, 2, 12.00, 24.00),
(6, 30, 1, 15.00, 15.00),
(6, 33, 1, 48.00, 48.00),
(6, 34, 1, 52.00, 52.00),
(6, 35, 1, 12.00, 12.00),
(6, 36, 1, 14.00, 14.00),
-- Pedido 7
(7, 26, 1, 48.00, 48.00),
(7, 28, 1, 8.00, 8.00),
(7, 29, 1, 12.00, 12.00),
(7, 30, 1, 15.00, 15.00),
(7, 37, 1, 12.00, 12.00),
(7, 38, 1, 14.00, 14.00),
(7, 39, 1, 10.00, 10.00),
(7, 40, 1, 22.00, 22.00),
(7, 41, 1, 24.00, 24.00),
-- Pedido 8
(8, 42, 1, 12.00, 12.00),
(8, 43, 1, 12.00, 12.00),
(8, 44, 1, 14.00, 14.00),
(8, 45, 1, 10.00, 10.00),
(8, 46, 1, 24.50, 24.50),
-- Pedido 9
(9, 47, 1, 22.00, 22.00),
(9, 48, 1, 12.00, 12.00),
(9, 49, 1, 8.00, 8.00),
(9, 50, 1, 4.00, 4.00),
-- Pedido 10
(10, 51, 1, 1.50, 1.50),
(10, 52, 2, 4.00, 8.00),
(10, 53, 1, 8.00, 8.00),
(10, 54, 1, 10.00, 10.00),
(10, 55, 1, 12.00, 12.00),
(10, 56, 2, 2.50, 5.00);

-- ============================================================
-- Resumo de Dados Inseridos
-- ============================================================
-- Estabelecimentos: 5
-- Categorias: 14
-- Produtos: 56
-- Usuários: 7 (1 Admin Master + 6 Gerentes/Operadores)
-- Clientes: 10
-- Pedidos: 10
-- Itens de Pedido: 56

PRINT 'Banco de dados populado com sucesso!'
PRINT 'Estabelecimentos: 5'
PRINT 'Produtos: 56'
PRINT 'Clientes: 10'
PRINT 'Pedidos: 10'
