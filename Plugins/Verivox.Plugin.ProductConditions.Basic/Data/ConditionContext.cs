using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Verivox.Common.Data;
using Verivox.Data;
using Verivox.Domain;
using Verivox.Plugin.ProductConditions.Basic.Domain;

namespace Verivox.Plugin.ProductConditions.Basic.Data
{
    public class ConditionContext : DbContext, IDbContext
    {
        public ConditionContext(DbContextOptions<ConditionContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConditionMap());
            base.OnModelCreating(modelBuilder);
        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = Database.BeginTransaction())
            {
                int result = Database.ExecuteSqlRaw(sql, parameters);
                transaction.Commit();

                return result;
            }
        }

        public string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }


        /// <summary>
        /// Install object context
        /// </summary>
        public void Install()
        {
            //create tables
            this.ExecuteSqlScript(GenerateCreateScript());
        }

        /// <summary>
        /// Uninstall object context
        /// </summary>
        public void Uninstall()
        {
            //drop the table
            this.DropPluginTable(nameof(Condition));
        }

        public List<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class
        {
            throw new NotImplementedException();
        }
    }
}
