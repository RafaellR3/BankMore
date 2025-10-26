CREATE TABLE IF NOT EXISTS Usuario (
    Id CHAR(36) NOT NULL,     
    Nome VARCHAR(200) NOT NULL,
    Cpf VARCHAR(11) NOT NULL,
    Senha varchar(255) not null,
    PRIMARY KEY (Id),
    UNIQUE KEY (Cpf)         
);

CREATE TABLE IF NOT EXISTS ContaCorrente (
	Id CHAR(36) PRIMARY KEY, -- id da conta corrente
	Numero INTEGER(10) NOT NULL UNIQUE, -- numero da conta corrente
	Cpf VARCHAR(11) not null UNIQUE,
	Ativo INTEGER(1) NOT NULL default 0, -- indicativo se a conta esta ativa. (0 = inativa, 1 = ativa).
	Senha VARCHAR(100) NOT NULL,
	Salt VARCHAR(100) NOT NULL,
	CHECK (Ativo in (0,1))
);
/*
CREATE TABLE movimento (
	idmovimento CHAR(36) PRIMARY KEY, -- identificacao unica do movimento
	idcontacorrente TEXT(37) NOT NULL, -- identificacao unica da conta corrente
	datamovimento TEXT(25) NOT NULL, -- data do movimento no formato DD/MM/YYYY
	tipomovimento TEXT(1) NOT NULL, -- tipo do movimento. (C = Credito, D = Debito).
	valor REAL NOT NULL, -- valor do movimento. Usar duas casas decimais.
	CHECK (tipomovimento in ('C','D')),
	FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente)
);

CREATE TABLE idempotencia (
	chave_idempotencia CHAR(36) PRIMARY KEY, -- identificacao chave de idempotencia
	requisicao TEXT(1000), -- dados de requisicao
	resultado TEXT(1000) -- dados de retorno
);\*