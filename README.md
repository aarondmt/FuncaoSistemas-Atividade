# FuncaoSistemas-Atividade
 Atividade de teste para processo seletivo na empresa Função Sistemas

## Foi implementado as seguintes funções
1. Inclusão do campo CPF nas telas de inclusão e alteração do cliente.
2. Criação de um botão denominado "Beneficiários" nas telas de inclusão e alteração.
   - (Tela de Inclsão do Cliente) Ao clicar no botão é aberto um pop-up (modal) para efetuar a inclusão dos beneficiários do cliente. Quando incluido, os dados são demonstrados em uma grid no próprio modal que permite a exclusão ou alteração do mesmo.
   - (Tela de Alteração do Cliente) Quando carregada as informações, os beneficiarios são carregados juntamente, gerando a grid no modal. Ao clicar no botão "Beneficiários" o modal é aberto já com os beneficiários que o cliente possui permitindo a alteração ou exclusão deles e a inclusão de novos.

## Implementação
Para implementar as funções sitadas acima, foi necessário a alteração no Banco de Dados da tabela CLIENTES para adicionar o campo CPF e criação da Tabela BENEFICIARIOS com os campos solicitados.
Além disso foram criadas Stored Procedures (sendo o padrão de implementação do sistema) para **Inserção, Alteração, Exclusão e Consulta** para as ações relacionadas a tabela BENEFICIARIOS e alterada algumas Sotred Procedures relacionadas a tabela CLIENTES para atender as alterações do sistema.
