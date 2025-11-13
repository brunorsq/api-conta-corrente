# API Conta Corrente

Aplicação para gerenciamento de Contas Correntes e suas Movimentações.

## Rodando a aplicação

- Acessar a parta raiz pelo terminal;
- Executar "docker compose up";
- Acessar localhost:5000/swagger/index.html;
- Utilizar o endpoint de Autorização utilizando usuário "admin" e senha "123" para obter o bearer token e poder testar o restante dos endpoints.

## Funcionalidades implementadas

- Autenticação.
- Cadastrar Conta Correte.
- Efetuar Login.
- Inativar Conta Corrente.
- Adicionar Movimentações na Conta Corrente
- Buscar Saldo da Conta Correte

## Funcionalidades pendentes / melhorias

- **Encriptação de CPF**: Atualmente os CPFs são armazenados em texto puro. É necessário implementar encriptação para aumentar a segurança dos dados sensíveis.
- **Leitura de fila Kafka**: O processamento atual não consome mensagens da fila Kafka. A implementação dessa funcionalidade é necessária para o processamento assíncrono de eventos.
