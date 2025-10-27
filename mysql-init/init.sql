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

CREATE TABLE  IF NOT EXISTS movimento (
	Id CHAR(36) PRIMARY KEY, -- identificacao unica do movimento
	IdContaCorrente CHAR(36) NOT NULL, -- identificacao unica da conta corrente
	DataMovimentacao DATE NOT NULL, -- data do movimento no formato DD/MM/YYYY
	TipoMovimento CHAR(1) NOT NULL, -- tipo do movimento. (C = Credito, D = Debito).
	Valor decimal NOT NULL, -- valor do movimento. Usar duas casas decimais.
	FOREIGN KEY(IdContaCorrente) REFERENCES ContaCorrente(Id)
);

CREATE TABLE IF NOT EXISTS idempotencia (
	chave_idempotencia VARCHAR(36) PRIMARY KEY, -- identificacao chave de idempotencia
	requisicao VARCHAR(1000), -- dados de requisicao
	resultado VARCHAR(1000) -- dados de retorno
);

CREATE TABLE IF NOT EXISTS transferencia (
	Id CHAR(36) PRIMARY KEY, -- identificacao unica da transferencia
	IdContaCorrenteOrigem CHAR(36) NOT NULL, -- identificacao unica da conta corrente de origem
	IdContaCorrenteDestino CHAR(36) NOT NULL, -- identificacao unica da conta corrente de destino
	DataMovimentcao DATE NOT NULL, -- data do transferencia no formato DD/MM/YYYY
	Valor decimal NOT NULL, -- valor da transferencia. Usar duas casas decimais.
	FOREIGN KEY(IdContaCorrenteOrigem) REFERENCES ContaCorrente(Id),
	FOREIGN KEY(IdContaCorrenteDestino) REFERENCES ContaCorrente(Id)
);
