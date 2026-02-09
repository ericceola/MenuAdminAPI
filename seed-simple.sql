-- Script simples para popular dados básicos

-- Inserir Estabelecimentos
INSERT INTO Estabelecimentos (Nome, CNPJ, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, Status, Plano)
VALUES 
('Café Senador', '12.345.678/0001-90', 'contato@cafesenador.com', '(11) 3456-7890', '(11) 96064-6979', 'Rua Senador Flaquer, 282', 'Santo André', 'SP', 'Ativo', 'Premium'),
('Bar do João', '98.765.432/0001-12', 'contato@bardojoao.com', '(11) 2345-6789', '(11) 98765-4321', 'Av. Brasil, 1500', 'São Paulo', 'SP', 'Ativo', 'Profissional'),
('Restaurante Sabor Mineiro', '55.555.555/0001-55', 'contato@sabor-mineiro.com', '(31) 3333-3333', '(31) 99999-9999', 'Rua das Flores, 250', 'Belo Horizonte', 'MG', 'Ativo', 'Premium'),
('Sorveteria Gelato', '77.777.777/0001-77', 'contato@gelato.com', '(21) 7777-7777', '(21) 98888-8888', 'Av. Copacabana, 500', 'Rio de Janeiro', 'RJ', 'Ativo', 'Básico'),
('Padaria do Bairro', '33.333.333/0001-33', 'contato@padaria.com', '(85) 3333-3333', '(85) 99999-9999', 'Rua Principal, 100', 'Fortaleza', 'CE', 'Ativo', 'Profissional');

-- Inserir Usuários
INSERT INTO Usuarios (Nome, Email, Telefone, Funcao, Ativo)
VALUES 
('Admin Master', 'admin@menuplatform.com', '(11) 99999-9999', 'Admin Master', 1),
('João Silva', 'joao@cafesenador.com', '(11) 96064-6979', 'Gerente', 1),
('Maria Santos', 'maria@cafesenador.com', '(11) 96064-6980', 'Operador', 1),
('Pedro Oliveira', 'pedro@bardojoao.com', '(11) 98765-4321', 'Gerente', 1),
('Ana Costa', 'ana@sabor-mineiro.com', '(31) 99999-9999', 'Gerente', 1),
('Carlos Mendes', 'carlos@gelato.com', '(21) 98888-8888', 'Gerente', 1),
('Lucia Ferreira', 'lucia@padaria.com', '(85) 99999-9999', 'Gerente', 1);

-- Inserir Clientes
INSERT INTO Clientes (EstabelecimentoId, Nome, Email, Telefone, WhatsApp, Endereco, Cidade, Estado, Status)
SELECT TOP 1 Id, 'João Silva', 'joao.silva@email.com', '(11) 98765-4321', '(11) 98765-4321', 'Rua A, 100', 'Santo André', 'SP', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Café Senador'
UNION ALL
SELECT TOP 1 Id, 'Maria Santos', 'maria.santos@email.com', '(11) 98765-4322', '(11) 98765-4322', 'Rua B, 200', 'Santo André', 'SP', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Café Senador'
UNION ALL
SELECT TOP 1 Id, 'Pedro Costa', 'pedro.costa@email.com', '(11) 98765-4323', '(11) 98765-4323', 'Rua C, 300', 'São Paulo', 'SP', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Café Senador'
UNION ALL
SELECT TOP 1 Id, 'Ana Oliveira', 'ana.oliveira@email.com', '(11) 98765-4324', '(11) 98765-4324', 'Av. Paulista, 1000', 'São Paulo', 'SP', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Bar do João'
UNION ALL
SELECT TOP 1 Id, 'Carlos Mendes', 'carlos.mendes@email.com', '(11) 98765-4325', '(11) 98765-4325', 'Rua D, 400', 'São Paulo', 'SP', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Bar do João'
UNION ALL
SELECT TOP 1 Id, 'Lucia Ferreira', 'lucia.ferreira@email.com', '(31) 98765-4326', '(31) 98765-4326', 'Rua E, 500', 'Belo Horizonte', 'MG', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Restaurante Sabor Mineiro'
UNION ALL
SELECT TOP 1 Id, 'Roberto Alves', 'roberto.alves@email.com', '(31) 98765-4327', '(31) 98765-4327', 'Rua F, 600', 'Belo Horizonte', 'MG', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Restaurante Sabor Mineiro'
UNION ALL
SELECT TOP 1 Id, 'Fernanda Dias', 'fernanda.dias@email.com', '(21) 98765-4328', '(21) 98765-4328', 'Rua G, 700', 'Rio de Janeiro', 'RJ', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Sorveteria Gelato'
UNION ALL
SELECT TOP 1 Id, 'Gustavo Rocha', 'gustavo.rocha@email.com', '(21) 98765-4329', '(21) 98765-4329', 'Rua H, 800', 'Rio de Janeiro', 'RJ', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Sorveteria Gelato'
UNION ALL
SELECT TOP 1 Id, 'Helena Lima', 'helena.lima@email.com', '(85) 98765-4330', '(85) 98765-4330', 'Rua I, 900', 'Fortaleza', 'CE', 'Ativo' FROM Estabelecimentos WHERE Nome = 'Padaria do Bairro';

PRINT 'Banco de dados populado com sucesso!'
PRINT 'Estabelecimentos: 5'
PRINT 'Usuários: 7'
PRINT 'Clientes: 10'
