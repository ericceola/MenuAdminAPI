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

INSERT INTO Produtos (EstabelecimentoId, CategoriaId, Nome, Descricao, Preco, Disponivel)
VALUES 
-- Café Senador - Bebidas Quentes
(1, 1, 'Café Espresso', 'Espresso clássico italiano', 8.90, 1),
(1, 1, 'Café Duplo', 'Dose dupla de espresso', 13.50, 1),
(1, 1, 'Café com Leite', 'Café coado com leite vaporizado', 12.50, 1),
(1, 1, 'Cappuccino', 'Cappuccino cremoso com espuma', 11.50, 1),
(1, 1, 'Latte', 'Leite vaporizado com espresso', 13.00, 1),
-- Café Senador - Bebidas Frias
(1, 2, 'Café Gelado', 'Café coado servido gelado', 9.50, 1),
(1, 2, 'Suco Natural', 'Suco natural de frutas', 10.00, 1),
(1, 2, 'Refrigerante', 'Refrigerante 350ml', 6.00, 1),
-- Café Senador - Doces e Bolos
(1, 3, 'Bolo de Chocolate', 'Bolo rico em chocolate', 12.90, 1),
(1, 3, 'Bolo de Cenoura', 'Bolo de cenoura com cobertura', 12.90, 1),
(1, 3, 'Brownie', 'Brownie denso e fudgy', 13.50, 1),
(1, 3, 'Torta de Limão', 'Torta clássica de limão', 16.90, 1),
-- Café Senador - Salgados
(1, 4, 'Croissant de Chocolate', 'Croissant amanteigado com chocolate', 18.90, 1),
(1, 4, 'Croissant de Queijo', 'Croissant com queijo derretido', 16.90, 1),
(1, 4, 'Sanduíche Natural', 'Sanduíche com frango e salada', 22.50, 1),
(1, 4, 'Tapioca de Carne Louca', 'Tapioca recheada com carne-louca', 33.90, 1),
-- Bar do João - Bebidas Alcoólicas
(2, 5, 'Cerveja Artesanal', 'Cerveja artesanal 500ml', 18.00, 1),
(2, 5, 'Chopp Brahma', 'Chopp gelado 400ml', 15.00, 1),
(2, 5, 'Caipirinha', 'Caipirinha com cachaça premium', 22.00, 1),
(2, 5, 'Mojito', 'Mojito refrescante', 24.00, 1),
-- Bar do João - Petiscos
(2, 7, 'Batatas Fritas', 'Batatas fritas crocantes', 15.00, 1),
(2, 7, 'Amendoim Salgado', 'Amendoim torrado e salgado', 12.00, 1),
(2, 7, 'Bolinhas de Queijo', 'Bolinhas de queijo frito', 18.00, 1),
-- Restaurante Sabor Mineiro - Pratos Principais
(3, 8, 'Frango à Mineira', 'Frango ao molho com quiabo', 42.00, 1),
(3, 8, 'Carne de Panela', 'Carne cozida no molho', 48.00, 1),
(3, 8, 'Peixe Grelhado', 'Peixe fresco grelhado', 55.00, 1),
(3, 8, 'Costela à Mineira', 'Costela assada no forno', 52.00, 1),
-- Restaurante Sabor Mineiro - Acompanhamentos
(3, 9, 'Arroz Branco', 'Arroz branco cozido', 8.00, 1),
(3, 9, 'Feijão Tropeiro', 'Feijão com bacon e linguiça', 12.00, 1),
(3, 9, 'Salada Verde', 'Salada fresca com alface e tomate', 15.00, 1),
-- Sorveteria Gelato - Sorvetes
(4, 11, 'Sorvete Chocolate', 'Sorvete de chocolate premium', 12.00, 1),
(4, 11, 'Sorvete Morango', 'Sorvete de morango natural', 12.00, 1),
(4, 11, 'Sorvete Pistache', 'Sorvete de pistache gourmet', 14.00, 1),
(4, 11, 'Sorvete Baunilha', 'Sorvete de baunilha clássico', 10.00, 1),
-- Sorveteria Gelato - Açaí
(4, 12, 'Açaí Completo', 'Açaí com granola e frutas', 24.50, 1),
(4, 12, 'Açaí com Leite Condensado', 'Açaí com leite condensado', 22.00, 1),
-- Padaria do Bairro - Pães
(5, 13, 'Pão Francês', 'Pão francês quentinho', 1.50, 1),
(5, 13, 'Pão de Queijo', 'Pão de queijo caseiro', 4.00, 1),
(5, 13, 'Baguete', 'Baguete francesa', 8.00, 1),
-- Padaria do Bairro - Bolos
(5, 14, 'Bolo de Milho', 'Bolo de milho caseiro', 10.00, 1),
(5, 14, 'Bolo de Fubá', 'Bolo de fubá cremoso', 12.00, 1),
(5, 14, 'Broinhas de Chuva', 'Broinhas doces', 2.50, 1);

-- ============================================================
-- 4. INSERIR USUÁRIOS
-- ============================================================

INSERT INTO Usuarios (EstabelecimentoId, Nome, Email, Telefone, Funcao, Ativo)
VALUES 
(NULL, 'Admin Master', 'admin@menuplatform.com', '(11) 99999-9999', 'Admin Master', 1),
(1, 'João Silva', 'joao@cafesenador.com', '(11) 96064-6979', 'Gerente', 1),
(1, 'Maria Santos', 'maria@cafesenador.com', '(11) 96064-6980', 'Operador', 1),
(2, 'Pedro Oliveira', 'pedro@bardojoao.com', '(11) 98765-4321', 'Gerente', 1),
(3, 'Ana Costa', 'ana@sabor-mineiro.com', '(31) 99999-9999', 'Gerente', 1),
(4, 'Carlos Mendes', 'carlos@gelato.com', '(21) 98888-8888', 'Gerente', 1),
(5, 'Lucia Ferreira', 'lucia@padaria.com', '(85) 99999-9999', 'Gerente', 1);

-- ============================================================
-- 5. INSERIR CLIENTES
-- ============================================================

INSERT INTO Clientes (EstabelecimentoId, Nome, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, Status)
VALUES 
(1, 'João Silva', 'joao.silva@email.com', '(11) 98765-4321', '(11) 98765-4321', 'Rua A, 100', 'Santo André', 'SP', 'Ativo'),
(1, 'Maria Santos', 'maria.santos@email.com', '(11) 98765-4322', '(11) 98765-4322', 'Rua B, 200', 'Santo André', 'SP', 'Ativo'),
(1, 'Pedro Costa', 'pedro.costa@email.com', '(11) 98765-4323', '(11) 98765-4323', 'Rua C, 300', 'São Paulo', 'SP', 'Ativo'),
(2, 'Ana Oliveira', 'ana.oliveira@email.com', '(11) 98765-4324', '(11) 98765-4324', 'Av. Paulista, 1000', 'São Paulo', 'SP', 'Ativo'),
(2, 'Carlos Mendes', 'carlos.mendes@email.com', '(11) 98765-4325', '(11) 98765-4325', 'Rua D, 400', 'São Paulo', 'SP', 'Ativo'),
(3, 'Lucia Ferreira', 'lucia.ferreira@email.com', '(31) 98765-4326', '(31) 98765-4326', 'Rua E, 500', 'Belo Horizonte', 'MG', 'Ativo'),
(3, 'Roberto Alves', 'roberto.alves@email.com', '(31) 98765-4327', '(31) 98765-4327', 'Rua F, 600', 'Belo Horizonte', 'MG', 'Ativo'),
(4, 'Fernanda Dias', 'fernanda.dias@email.com', '(21) 98765-4328', '(21) 98765-4328', 'Rua G, 700', 'Rio de Janeiro', 'RJ', 'Ativo'),
(4, 'Gustavo Rocha', 'gustavo.rocha@email.com', '(21) 98765-4329', '(21) 98765-4329', 'Rua H, 800', 'Rio de Janeiro', 'RJ', 'Ativo'),
(5, 'Helena Lima', 'helena.lima@email.com', '(85) 98765-4330', '(85) 98765-4330', 'Rua I, 900', 'Fortaleza', 'CE', 'Ativo');

-- ============================================================
-- 6. INSERIR PEDIDOS
-- ============================================================

INSERT INTO Pedidos (EstabelecimentoId, ClienteId, Status, Total, Observacoes)
VALUES 
(1, 1, 'Entregue', 45.90, 'Entrega realizada'),
(1, 2, 'Entregue', 67.50, 'Cliente satisfeito'),
(1, 3, 'Entregue', 89.00, ''),
(2, 4, 'Entregue', 120.00, 'Pedido completo'),
(2, 5, 'Pendente', 95.50, 'Aguardando confirmação'),
(3, 6, 'Entregue', 250.00, 'Entrega realizada'),
(3, 7, 'Entregue', 180.00, ''),
(4, 8, 'Entregue', 75.00, 'Cliente satisfeito'),
(4, 9, 'Pendente', 48.50, 'Aguardando retirada'),
(5, 10, 'Pendente', 35.00, 'Pedido novo');

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
(2, 14, 1, 16.90, 16.90),
(2, 11, 1, 13.50, 13.50),
-- Pedido 3
(3, 5, 2, 13.00, 26.00),
(3, 7, 1, 10.00, 10.00),
(3, 8, 2, 6.00, 12.00),
(3, 18, 1, 22.00, 22.00),
(3, 19, 1, 18.00, 18.00),
-- Pedido 4
(4, 21, 1, 15.00, 15.00),
(4, 22, 2, 18.00, 36.00),
(4, 23, 1, 24.00, 24.00),
(4, 24, 1, 15.00, 15.00),
(4, 25, 1, 12.00, 12.00),
(4, 26, 1, 18.00, 18.00),
-- Pedido 5
(5, 27, 1, 42.00, 42.00),
(5, 30, 1, 8.00, 8.00),
(5, 31, 1, 12.00, 12.00),
(5, 32, 1, 15.00, 15.00),
(5, 33, 1, 12.00, 12.00),
-- Pedido 6
(6, 35, 1, 55.00, 55.00),
(6, 30, 2, 8.00, 16.00),
(6, 31, 2, 12.00, 24.00),
(6, 32, 1, 15.00, 15.00),
(6, 36, 1, 48.00, 48.00),
(6, 37, 1, 52.00, 52.00),
(6, 38, 1, 12.00, 12.00),
(6, 39, 1, 14.00, 14.00),
-- Pedido 7
(7, 28, 1, 48.00, 48.00),
(7, 30, 1, 8.00, 8.00),
(7, 31, 1, 12.00, 12.00),
(7, 32, 1, 15.00, 15.00),
(7, 40, 1, 12.00, 12.00),
(7, 41, 1, 14.00, 14.00),
(7, 42, 1, 10.00, 10.00),
(7, 43, 1, 22.00, 22.00),
(7, 44, 1, 24.00, 24.00),
-- Pedido 8
(8, 45, 1, 12.00, 12.00),
(8, 46, 1, 12.00, 12.00),
(8, 47, 1, 14.00, 14.00),
(8, 48, 1, 10.00, 10.00),
(8, 49, 1, 24.50, 24.50),
-- Pedido 9
(9, 50, 1, 22.00, 22.00),
(9, 51, 1, 12.00, 12.00),
(9, 52, 1, 8.00, 8.00),
(9, 53, 1, 4.00, 4.00),
-- Pedido 10
(10, 54, 1, 1.50, 1.50),
(10, 55, 2, 4.00, 8.00),
(10, 56, 1, 8.00, 8.00),
(10, 57, 1, 10.00, 10.00),
(10, 58, 1, 12.00, 12.00),
(10, 59, 2, 2.50, 5.00);

PRINT 'Banco de dados populado com sucesso!'
PRINT 'Estabelecimentos: 5'
PRINT 'Produtos: 59'
PRINT 'Clientes: 10'
PRINT 'Pedidos: 10'
