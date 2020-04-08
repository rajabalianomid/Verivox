using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Verivox.Common.Data;
using Verivox.Domain;

namespace Verivox.Data
{
    public partial class Context : DbContext, IDbContext
    {
        #region Ctor

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<Type> typeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                    || type.BaseType.GetGenericTypeDefinition() == typeof(QueryTypeConfiguration<>)));

            foreach (Type typeConfiguration in typeConfigurations)
            {
                IMappingConfiguration configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>

        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (int i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                {
                    continue;
                }

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                {
                    sql = $"{sql} output";
                }
            }

            return sql;
        }

        #endregion

        #region Methods

        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> entityEntry = Entry(entity);
            if (entityEntry == null)
            {
                return;
            }

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            return Set<TEntity>().FromSqlRaw(CreateSqlWithParameters(sql, parameters), parameters);
        }

        public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            int? previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            int result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = Database.BeginTransaction())
                {
                    result = Database.ExecuteSqlRaw(sql, parameters);
                    transaction.Commit();
                }
            }
            else
            {
                result = Database.ExecuteSqlRaw(sql, parameters);
            }

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        public virtual string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        public virtual List<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class
        {
            using (DbCommand command = Database.GetDbConnection().CreateCommand())
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                using (DbDataReader reader = command.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    return table.ToList<TQuery>();
                }
            }
        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}
