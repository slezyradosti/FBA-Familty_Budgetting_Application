﻿using Dapper;
using FamilyBudgeting.Application.Configuration;
using FamilyBudgeting.Application.DTOs;
using FamilyBudgeting.Application.Interfaces;
using FamilyBudgeting.Infrastructure.Utilities;

namespace FamilyBudgeting.Infrastructure.Queries
{
    public class UserLedgerRoleQueryService : IUserLedgerRoleQueryService
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserLedgerRoleQueryService(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UserLedgerRoleDto>> GetUserLedgerRolesAsync()
        {
            string query = @"
                SELECT Id, Title
                FROM UserLedgerRole
                ORDER BY Id
                ";

            QueryLogger.LogQuery(query, null);

            using (var conn = _connectionFactory.GetOpenConnection())
            {
                return await conn.QueryAsync<UserLedgerRoleDto>(query);
            }
        }

        public async Task<UserLedgerRoleDto?> GetUserLedgerRoleByTitleAsync(string title)
        {
            string query = @"
                SELECT Id, Title
                FROM UserLedgerRole
                WHERE Title COLLATE SQL_Latin1_General_CP1_CI_AS LIKE @Title
                ORDER BY Id
                ";

            using (var conn = _connectionFactory.GetOpenConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<UserLedgerRoleDto?>(query,
                    new
                    {
                        Title = title
                    }
                );
            }
        }
    }
}
