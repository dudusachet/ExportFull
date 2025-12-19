# PLSQLExportFull 🚀

**PLSQLExportFull** é uma ferramenta robusta e moderna desenvolvida em Windows Forms (.NET) para facilitar a exportação de dados (DML) de bancos de dados **Oracle**. 

Com foco em agilidade, permite selecionar tabelas, aplicar filtros `WHERE` personalizados e gerar scripts `.sql` (INSERTs) prontos para migração ou backup.

![Status do Projeto](https://img.shields.io/badge/Status-Estável-green) ![Platform](https://img.shields.io/badge/Plataforma-Windows-blue) ![Database](https://img.shields.io/badge/Oracle-Database-red)

## ✨ Funcionalidades Principais

* **Conexão Flexível:** Suporta conexão via TNS Full (String completa) ou simplificada (User/Pass@Host:Port/Service).
* **Auto-Connect:** Reconhece e conecta automaticamente ao colar strings de conexão ou carregar arquivos `.config`.
* **Seleção de Tabelas:** Visualização clara das tabelas com contagem de linhas.
* **Filtros Inteligentes:** Edição da cláusula `WHERE` diretamente na grid para exportar apenas os dados necessários.
* **Opções de Exportação:**
    * ✅ **Add Truncate:** Adiciona comando `TRUNCATE TABLE` antes dos inserts.
    * 📦 **Zip Output:** Compacta o arquivo final automaticamente (integração com 7-Zip).
* **Interface Moderna:** Design limpo (Flat UI) com indicadores visuais de status e rodapé informativo.
* **Grupos de Tabelas:** Filtragem rápida de tabelas baseada em grupos pré-definidos (`TableGroups.json`).

## 🎨 Personalização (Temas)

O aplicativo conta com um sistema de temas embutido.
* **Atalho Secreto:** Pressione `Ctrl + Alt + G` para alternar entre os temas:
    * 🔴 **Red Enterprise** (Padrão)
    * 🔵 **Blue Ocean**
* *Nota:* A preferência de cor é salva automaticamente e lembrada na próxima execução.

## 🛠️ Instalação e Requisitos

1.  **Requisitos:**
    * Windows 10/11
    * .NET Framework 4.0
    * Cliente Oracle ou DLLs necessárias (Oracle.ManagedDataAccess).
    * *(Opcional)* `7za.exe` na pasta raiz para funcionalidade de compactação.

2.  **Como Usar:**
    * Execute `PLSQLExportFull.exe`.
    * Preencha os dados de conexão ou clique em **"Colar String"**.
    * Selecione o grupo de tabelas ou pesquise manualmente.
    * Marque as tabelas desejadas (Checkboxes).
    * (Opcional) Edite a coluna "Condição (Where)".
    * Clique em **Exportar** e escolha o local de salvamento.

## ⚙️ Arquivo de Configuração (.config)

O sistema suporta importação automática de configurações. O arquivo deve seguir o padrão XML abaixo:

```xml
<?xml version="1.0"?>
<configuration>
    <appSettings>
        <add key="strConexaoBD" value="data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.25.100.205)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=usuario;Password=senha;"/>
    </appSettings>
</configuration>
