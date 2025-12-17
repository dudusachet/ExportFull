using System;
using System.Collections.Generic;
using PLSQLExportFull.Data;
using PLSQLExportFull.Models;

namespace PLSQLExportFull.Business
{
    public class TableManager
    {
        private OracleQueryExecutor _queryExecutor;
        private MetadataRepository _metadataRepository;
        public TableManager(OracleQueryExecutor queryExecutor, MetadataRepository metadataRepository)
        {
            _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            _metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
        }
        public List<TableInfo> GetAllTables()
        {
            return _metadataRepository.GetAllTables();
        }
        public void TruncateTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Nome da tabela não pode ser vazio.", nameof(tableName));
            }

            string command = $"TRUNCATE TABLE {tableName}";
            _queryExecutor.ExecuteNonQuery(command);
        }
        public int TruncateTables(List<string> tableNames)
        {
            if (tableNames == null || tableNames.Count == 0)
            {
                return 0;
            }

            int successCount = 0;
            List<string> errors = new List<string>();

            foreach (string tableName in tableNames)
            {
                try
                {
                    TruncateTable(tableName);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Erro ao truncar tabela {tableName}: {ex.Message}");
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception($"Algumas tabelas não puderam ser truncadas:\n{string.Join("\n", errors)}");
            }

            return successCount;
        }
        public void DeleteAllFromTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Nome da tabela não pode ser vazio.", nameof(tableName));
            }

            string command = $"DELETE FROM {tableName}";
            _queryExecutor.ExecuteNonQuery(command);
        }

        public long GetTableRowCount(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Nome da tabela não pode ser vazio.", nameof(tableName));
            }

            string query = $"SELECT COUNT(*) FROM {tableName}";
            object result = _queryExecutor.ExecuteScalar(query);
            
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt64(result);
            }

            return 0;
        }

        public bool TableExists(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }

            string query = $"SELECT COUNT(*) FROM USER_TABLES WHERE TABLE_NAME = '{tableName.ToUpper()}'";
            object result = _queryExecutor.ExecuteScalar(query);
            
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result) > 0;
            }

            return false;
        }
    }
}
