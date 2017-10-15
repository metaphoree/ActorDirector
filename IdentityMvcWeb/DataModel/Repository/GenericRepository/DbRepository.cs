using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kloud21.ADODAL;
using Kloud21.DataModel;


namespace Kloud21.GenericRepository
{
    public class DbRepository<TKey, TEntity> : IRepository<TKey, TEntity>, IDisposable 
        where TEntity : class, IIdentityKey<TKey>, new()
    {
        #region [    Protected Members    ]

        protected readonly IDbSession DbSession;
        private string schemaName;
        protected string SchemaName {

            get { return schemaName; }

            set { schemaName = (value == null) ? "dbo" : value; }
        }

        protected virtual string TableName
        {
            get { return typeof(TEntity).Name; }
        }

        protected virtual string StoreProcPrefix
        {
            get { return SchemaName + ".qsp_"; }
        }

        protected IList<TEntity> ExecuteStoreProcedureReader(string spName, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
            return DbSession.ExecuteReader<TEntity>(CommandType.StoredProcedure, spName, parameters, outputParameters);
        }
      
        protected void ExecuteStoreProcedureScalar<T>(string spName, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
             DbSession.ExecuteScalar<T>(CommandType.StoredProcedure, spName, parameters, outputParameters);
        }

        protected int ExecuteStoreProcedureNonQuery(string spName, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
            return DbSession.ExecuteNonQuery(CommandType.StoredProcedure, spName, parameters, outputParameters);
        }

        protected IDictionary<string, object> GetParametersFromEntity(IIdentityKey<TKey> key, params string[] excluded)
        {
            var result = new Dictionary<string, object>();
            var properties = key.GetType().GetProperties().Where(x => x.CanRead && !excluded.Contains(x.Name));
            foreach (var property in properties)
            {
                var propKey = property.Name;
                var propValue = property.GetValue(key, null);

                if (typeof(IIdentityKey<TKey>).IsAssignableFrom(property.PropertyType))
                {
                    propKey = property.PropertyType.Name + "Id";
                    var asIkey = propValue as IIdentityKey<TKey>;
                    propValue = (asIkey != null) ? (object) asIkey.Id : null;
                }

                result.Add(propKey, propValue);
            }
            return result;
        }

        protected string Pluralize(string name)
        {
            if (name.EndsWith("y"))
            {
                return name.Substring(0, name.Length - 1) + "ies";
            }
            
            if (name.EndsWith("s"))
            { return name; }

            return name + "s";
        }

        #endregion

        public DbRepository(IDbSession dbSession)
        {
            DbSession = dbSession;
        }

        public virtual TEntity FindBy(TKey id)
        {
            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = TableName.ToLower() + "Id";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, id } });
            return result.FirstOrDefault();
        }
        
        public virtual void Add(TEntity entity)
        {
            var spName =  StoreProcPrefix + TableName + "_Create";
            var rparams = GetParametersFromEntity(entity, "Id");
             ExecuteStoreProcedureScalar<TKey>(spName, rparams);

            // var entityId = ExecuteStoreProcedureScalar<TKey>(spName, rparams);
            //return FindBy(entityId);
        }
        public virtual TEntity AddProfile(TEntity entity)
        {
            //var spName = StoreProcPrefix + TableName + "_Create";
            //var rparams = GetParametersFromEntity(entity);

            //            var entityId = ExecuteStoreProcedureScalar<TKey>(spName, rparams);
            //           return FindBy(entityId);
            return entity;
        }
        public virtual void Update(TEntity entity)
        {
            var spName = StoreProcPrefix + TableName + "_Update";
            var rparams = GetParametersFromEntity(entity);

            ExecuteStoreProcedureNonQuery(spName, rparams);
        }
        public virtual void UpdateStringColumn(long id, string colname, string colvalue)
        {
            var spName = StoreProcPrefix + "Update_string_column";
            var rparams = new Dictionary<string, object> { { "Id", id }, {"tablename", TableName}, { "colname", colname }, { "colvalue", colvalue } };

            ExecuteStoreProcedureNonQuery(spName, rparams);
        }
        public virtual TEntity UpdateProfile(TEntity entity)
        {
            //var spName = StoreProcPrefix + TableName + "_Update";
            //var rparams = GetParametersFromEntity(entity);
            //var entityId = ExecuteStoreProcedureScalar<TKey>(spName, rparams);
            //return FindBy(entityId);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            var spName = StoreProcPrefix + TableName + "_Delete";
            var paramId = TableName.ToLower() + "Id";
            ExecuteStoreProcedureNonQuery(spName, new Dictionary<string, object> { { paramId, entity.Id } });
        }

        public virtual IList<TEntity> All()
        {
            var spName = StoreProcPrefix + Pluralize(TableName) + "_List";
            var result = ExecuteStoreProcedureReader(spName, null);
            return result;
        }

        public virtual IList<TEntity> Search(string Searchtxt, string sortOrder)
        {
            var spName = StoreProcPrefix + Pluralize(TableName) + "_Search";
            
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { {  "@Searchtxt", (string.IsNullOrEmpty(Searchtxt) ? "" : Searchtxt) }, { "@sortOrder", (string.IsNullOrEmpty(sortOrder) ? "DESC" : sortOrder) } });
            return result;
        }
        public virtual IList<TEntity> SearchByDocClinic(string Searchtxt, string sortOrder, long docid, long clinicid)
        {
            var spName = StoreProcPrefix + Pluralize(TableName) + "_byDocbyClinic";

            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> {
                { "@Searchtxt", (string.IsNullOrEmpty(Searchtxt) ? "" : Searchtxt) },
                { "@sortOrder", (string.IsNullOrEmpty(sortOrder) ? "DESC" : sortOrder) },
                { "@doctorid" , docid}, { "@clinicid" , clinicid}
            });
            return result;
        }
        public void Dispose()
        {
            DbSession.CommitTransaction();
            DbSession.Dispose();
        }
    }
}
