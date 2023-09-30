Projecto Individual do Modulo 3 eLeilão
# eLeilao - Projeto .NET

Este é um projeto de implementação que visa desenvolver uma aplicação com as seguintes características:

- **Arquitetura MVC** - O projeto segue o padrão de arquitetura Modelo-Visão-Controlador (MVC) para organizar o código de forma eficiente.

- **Web API** - Utilizamos uma Web API para fornecer serviços e funcionalidades para a aplicação.

- **Autorização/autenticação com utilização de tokens** - A aplicação implementa um sistema de autorização e autenticação, onde os usuários são autenticados usando tokens para acessar recursos protegidos.

- **Entity Framework** - O Entity Framework é usado para gerenciar o acesso aos dados do aplicativo por meio de um Data Access Layer.

- **Mecanismo de logging** - Implementamos um mecanismo de registro de eventos para rastrear a atividade do aplicativo e auxiliar na solução de problemas.

## Estrutura do Projeto

O projeto está estruturado da seguinte forma:

- **Data Access Layer**
  - Contém o código relacionado ao acesso a dados, incluindo modelos de dados e configurações do Entity Framework.

- **Web API**
  - Aqui estão os controladores da Web API, que expõem os endpoints para interação com o aplicativo por meio de solicitações HTTP.

- **UI MVC**
  - Contém a parte da interface do usuário do aplicativo, incluindo visões, controladores e recursos estáticos.

## Como Executar o Projeto

Para executar o projeto, siga estas etapas:

1. Clone o repositório para sua máquina local.

2. Certifique-se de ter o ambiente .NET configurado e as dependências instaladas.

3. Abra o projeto em sua IDE preferida.

4. Certifique-se de que o banco de dados esteja configurado e migre as tabelas usando o Entity Framework.

5. Inicie a aplicação e acesse-a em seu navegador.

# Resumo do Projeto - eLeilao

O projeto eLeilao é uma aplicação que tem como objetivo criar uma plataforma de leilões online com funcionalidades de gerenciamento de leilões e participação de usuários. Aqui está um resumo das principais funcionalidades do projeto:

## Usuário Admin

- O administrador tem privilégios especiais e é responsável por toda a gestão da plataforma.
- Ele cria leilões, associa usuários normais a leilões específicos e fornece a esses usuários um email e senha de acesso.
- O administrador também pode fazer CRUD (Criar, Ler, Atualizar e Excluir) de usuários e leilões.

## Usuários Normais

- Os usuários normais não têm a capacidade de se cadastrar na plataforma, pois são criados e gerenciados exclusivamente pelo administrador.
- Cada usuário normal recebe um email e senha de acesso criado pelo administrador.
- Eles só podem acessar os leilões para os quais o administrador associou seus dados de acesso.
- Os usuários normais têm a capacidade de fazer lances (bids) nos leilões aos quais estão associados.
- Cada usuário normal só pode fazer até 3 lances por leilão.

Em resumo, o projeto eLeilao visa criar uma plataforma de leilões online com um sistema de gerenciamento robusto, permitindo que o administrador controle os leilões, crie usuários normais e os associe a leilões específicos. Os usuários normais recebem dados de acesso do administrador e podem participar ativamente dos lances apenas nos leilões aos quais têm acesso. É um sistema de leilões com recursos de autenticação, autorização e limitações de lances para proporcionar uma experiência de leilão segura e eficaz.
