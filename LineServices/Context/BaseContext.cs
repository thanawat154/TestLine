using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LineServices.Entity;
using System.Data.Entity;
using System.Data;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Configuration;

namespace LineServices.Context
{
    public class BaseContext : DbContext
    {

        public BaseContext() : base("name=DBConnect") { }
        public BaseContext(String connectionString) : base(connectionString) { }

        protected List<BaseEntity> ExecuteStore(String stored, params Object[] para)
        {                                
            return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<BaseEntity>(stored, para).ToList<BaseEntity>();
        }

        ~BaseContext()
        {
            this.Dispose();
        }
    }
}