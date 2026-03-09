# PaymentsAPI

Microsserviço responsável por simular o processamento de pagamentos na plataforma FiapCloudGames.

## Responsabilidades
- Consumir `OrderPlacedEvent` do RabbitMQ
- Simular aprovação/rejeição do pagamento (90% aprovação)
- Publicar `PaymentProcessedEvent` com o resultado

## Fluxo
```
OrderPlacedEvent (consumido)
       ↓
Simula pagamento (Approved / Rejected)
       ↓
PaymentProcessedEvent (publicado)
```

## Eventos consumidos
| Evento | Ação |
|--------|------|
| `OrderPlacedEvent` | Processa o pagamento e publica resultado |

## Eventos publicados
| Evento | Quando |
|--------|--------|
| `PaymentProcessedEvent` | Após simulação do pagamento |

## Variáveis de Ambiente

| Variável | Descrição |
|----------|-----------|
| `RabbitMQ__Host` | Host do RabbitMQ |
| `RabbitMQ__Username` | Usuário RabbitMQ |
| `RabbitMQ__Password` | Senha RabbitMQ |

## Health Check

```
GET /health
```
