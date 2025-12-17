using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PLSQLExportFull.Data;

namespace PLSQLExportFull.Business
{
    public class TableExportData
    {
        public string TableName { get; set; }
        public string WhereClause { get; set; }
        public string MinMax { get; set; }
    }

    public class ExportManager
    {
        // Dependências
        private readonly OracleQueryExecutor _queryExecutor;
        private readonly MetadataRepository _metadataRepository;

        // Controle de IDs para substituição dinâmica no WHERE
        private long minId;
        private long maxId;
        private long minAutorizacao;
        private long maxAutorizacao;

        public ExportManager(OracleQueryExecutor queryExecutor, MetadataRepository metadataRepository)
        {
            _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            _metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
        }

        public void ExportTablesDML(List<TableExportData> tablesToExport, string outputFilePath, string groupname, bool truncate, string servidorInfo)
        {
            // 1. Validações Iniciais
            if (tablesToExport == null || tablesToExport.Count == 0)
                throw new ArgumentException("Nenhuma tabela selecionada.");

            if (string.IsNullOrEmpty(outputFilePath))
                throw new ArgumentException("Caminho inválido.");

            if (string.IsNullOrEmpty(servidorInfo))
                servidorInfo = "N/A";

            // --- CORREÇÃO: RESETAR VARIÁVEIS AQUI ---
            // Isso garante que cada exportação comece "limpa"
            minId = long.MaxValue;
            maxId = 0;
            minAutorizacao = long.MaxValue;
            maxAutorizacao = 0;

            // --- MUDANÇA: StreamWriter (Escreve direto no disco para economizar RAM) ---
            using (StreamWriter sw = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                // 2. Cabeçalho Global do Script
                sw.WriteLine("-- Configurações de Ambiente");
                sw.WriteLine("SET ECHO OFF");
                sw.WriteLine("SET FEEDBACK OFF");
                sw.WriteLine("SET VERIFY OFF");
                sw.WriteLine("SET DEFINE OFF");
                sw.WriteLine("SET HEADING OFF");
                sw.WriteLine("SET SQLBLANKLINES ON");
                sw.WriteLine("SET TIMING OFF");
                sw.WriteLine();
                sw.WriteLine($"-- Origem:    {servidorInfo}");
                sw.WriteLine($"-- Script:    {groupname}");
                sw.WriteLine($"-- Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sw.WriteLine();

                sw.WriteLine("SET TERMOUT ON");
                sw.WriteLine($"SELECT 'Inicio: ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') FROM DUAL;");
                sw.WriteLine("SET TERMOUT OFF");
                sw.WriteLine();

                // 3. Processamento das Tabelas
                foreach (var table in tablesToExport)
                {
                    try
                    {
                        // Opcional: Força limpeza de lixo anterior para evitar pico de memória
                        // GC.Collect(); 

                        // --- Preparação do Filtro (WHERE) ---
                        var novoWhere = table.WhereClause;

                        if (!string.IsNullOrEmpty(novoWhere) && (novoWhere.Contains(":MIN") || novoWhere.Contains(":MAX")))
                        {
                            // --- SANITIZAÇÃO DE IDs (Evita número gigante) ---
                            long safeMinId = (minId == long.MaxValue) ? -1 : minId;
                            long safeMaxId = maxId;
                            long safeMinAuth = (minAutorizacao == long.MaxValue) ? -1 : minAutorizacao;
                            long safeMaxAuth = maxAutorizacao;

                            novoWhere = novoWhere.Replace(":MIN_ID", safeMinId.ToString())
                                                 .Replace(":MAX_ID", safeMaxId.ToString())
                                                 .Replace(":MIN_AUTORIZACAO", safeMinAuth.ToString())
                                                 .Replace(":MAX_AUTORIZACAO", safeMaxAuth.ToString());
                        }

                        string textoFiltro = string.IsNullOrWhiteSpace(novoWhere) ? "Nenhum Filtro" : novoWhere.Trim();

                        // --- Busca de Dados no Repositório ---
                        List<string> insertStatements = _metadataRepository.GetTableDML(
                            table.TableName,
                            novoWhere,
                            table.MinMax,
                            ref minId,
                            ref maxId,
                            ref minAutorizacao,
                            ref maxAutorizacao
                        );

                        // Remove vazios/comentários
                        insertStatements.RemoveAll(s => string.IsNullOrWhiteSpace(s) || s.Trim().StartsWith("--"));

                        int count = insertStatements.Count;

                        // --- Escrita do Cabeçalho da Tabela ---
                        sw.WriteLine("SET TERMOUT ON");
                        sw.WriteLine("prompt --------------------------------------------------");
                        sw.WriteLine($"SELECT 'Processando {table.TableName}...' FROM DUAL;");
                        sw.WriteLine($"prompt Filtro aplicado: {textoFiltro}");

                        if (truncate)
                        {
                            sw.WriteLine($"prompt [!] Truncating {table.TableName}...");
                            sw.WriteLine($"TRUNCATE TABLE {table.TableName};");
                        }

                        sw.WriteLine($"prompt Registros Gerados no Script: {count}");
                        sw.WriteLine("SET TERMOUT OFF");

                        // --- Escrita dos INSERTs (Streaming) ---
                        if (count > 0)
                        {
                            int rowCount = 0;
                            sw.WriteLine();

                            foreach (string insert in insertStatements)
                            {
                                sw.WriteLine(insert);
                                rowCount++;

                                // Commit a cada 100 registros
                                if (rowCount % 100 == 0) sw.WriteLine("commit;");
                            }

                            sw.WriteLine("commit;");
                            sw.WriteLine();

                            // Validação pós-insert
                            sw.WriteLine("SET TERMOUT ON");
                            sw.WriteLine($"SELECT 'VALIDACAO {table.TableName}: Esperado:' || {count} || ' | Encontrado:' || COUNT(1) FROM {table.TableName};");
                            sw.WriteLine("SET TERMOUT OFF");
                        }
                        else
                        {
                            sw.WriteLine($"-- Tabela vazia ou sem dados.");
                            sw.WriteLine("SET TERMOUT ON");
                            sw.WriteLine("prompt Tabela vazia (0 registros gerados).");
                            sw.WriteLine("SET TERMOUT OFF");
                        }

                        sw.WriteLine();

                        // --- LIMPEZA CRÍTICA DE MEMÓRIA ---
                        insertStatements.Clear();
                        insertStatements = null;

                        sw.Flush();
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("SET TERMOUT ON");
                        sw.WriteLine($"prompt ERRO NO C# AO PROCESSAR {table.TableName}: {ex.Message}");
                        sw.WriteLine("SET TERMOUT OFF");
                        sw.WriteLine($"-- Erro Detalhado: {ex}");
                        sw.WriteLine();
                    }
                }

                // 4. Rodapé Global
                sw.WriteLine("SET TERMOUT ON");
                sw.WriteLine("prompt --------------------------------------------------");
                sw.WriteLine("SELECT 'Fim: ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') FROM DUAL;");
                sw.WriteLine("prompt --------------------------------------------------");
            }
        }
    }
}