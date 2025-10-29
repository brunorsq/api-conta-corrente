# API Conta Corrente

Aplicação para gerenciamento de Contas Correntes e suas Movimentações.

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
