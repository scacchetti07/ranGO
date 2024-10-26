# ranGO! - Sistema de Controle e Gerenciamento para Restaurantes

Bem-vindo ao repositório da **ranGO!**, uma solução completa para o gerenciamento de restaurantes que inclui controle de estoque, gerenciamento de fornecedores, acompanhamento de pedidos e um dashboard interativo para visão geral do sistema. Desenvolvido pela startup **ranGO!**, nossa plataforma oferece um sistema intuitivo e eficiente que atende às necessidades operacionais dos restaurantes de maneira prática e escalável.

## 📖 Sobre o Produto
A **ranGO!** é uma empresa de tecnologia focada no desenvolvimento de sistemas de gestão para o setor gastronômico. Nosso produto principal é um aplicativo para restaurantes que permite escolher entre três planos, com funcionalidades ajustáveis de acordo com a necessidade de cada estabelecimento. As principais funcionalidades incluem:

- **Controle de Estoque Virtual**
- **Gerenciamento de Fornecedores**
- **Monitoramento de Pedidos**
- **Dashboard para Visão Geral**

## ✨ Funcionalidades

### 1. Controle de Estoque Virtual 📦
Gerencie os produtos no estoque de forma eficiente e intuitiva:
- Cadastro e armazenamento de dados dos produtos, como:
  - **Nome e Descrição** do produto
  - **Fornecedor** e **Código de Identificação**
  - **Preço Unitário**, **Unidade de Medida** e **Quantidade Atual**
  - **Data de Validade** e **Imagem do Produto**
  - **Quantidade Máxima e Mínima** de suporte no estoque

- **Gerenciamento do Estoque**:
  - A cada movimento, é possível atualizar a quantidade do produto manualmente.
  - Alarmes automáticos são ativados quando o produto atinge a quantidade mínima ou quando a data de validade se aproxima, garantindo um controle constante e seguro dos insumos.

### 2. Gerenciamento de Fornecedores 🤝
Adicione e gerencie seus fornecedores favoritos para um abastecimento mais eficiente:
- Cadastro de fornecedores com informações detalhadas:
  - **Nome do Fornecedor**
  - **Produtos Fornecidos**
  - **Endereço e CEP**
  - **Telefone, E-mail e CNPJ**
  - **Prazo de Entrega**

- **Opções de Gerenciamento**:
  - Contate os fornecedores diretamente pela plataforma.
  - Edite ou exclua informações quando necessário para manter o cadastro atualizado.

### 3. Monitoramento de Pedidos 📋
Controle os pedidos em tempo real, do preparo até a entrega, com opção de integração futura para dispositivos móveis:
- Cadastro e edição de novos pratos.
- **Gerenciamento de Pedidos**:
  - Criação, edição, exclusão e finalização de pedidos.
  - Filtragem de pedidos por status: **Novos**, **Em Preparação** e **Concluídos**.
  - Visualização do tempo de preparo, preço e nome do atendente responsável.
  - Quando concluído, o sistema atualiza o estoque automaticamente com a baixa dos ingredientes utilizados.

### 4. Dashboard 📊
Acompanhe métricas e informações essenciais de maneira visual:
- Gráficos e indicadores sobre:
  - **Capacidade do Estoque**
  - **Produtos mais Demandados**
  - **Dias com Maior Volume de Pedidos**
  - **Média de Vendas**
  - **Tempo de Entrega dos Fornecedores**

Este dashboard oferece uma visão completa para tomada de decisão rápida e informada.

## 🛠️ Tecnologias Utilizadas

- **Backend**: [Linguagem C#.NET](https://learn.microsoft.com/pt-br/dotnet/csharp/tour-of-csharp/) e [Asp.NET](https://dotnet.microsoft.com/pt-br/apps/aspnet) 
- **Frontend**: [Avalonia UI Framework](https://avaloniaui.net/)
- **Banco de Dados**: [MongoDB](https://www.mongodb.com/pt-br/docs/drivers/csharp/current/)
- **Interface de Usuário (UI)**: [Figma para design](https://www.figma.com) [XAML para design do sistema](https://docs.avaloniaui.net/docs/basics/user-interface/introduction-to-xaml)

## 🚀 Como Instalar

1. Clone o repositório:
```bash
 git clone https://github.com/scacchetti07/ranGO.git
```
2. Instale as dependências:
```bash
 cd rango
 npm install
```
3. Inicie o sistema:
```bash
 npm start
```
