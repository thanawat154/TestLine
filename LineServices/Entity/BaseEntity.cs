using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineServices.Entity
{
    public class BaseEntity
    {

        public Object GetPropertyValue(String propertyName)
        {
            System.Type type = this.GetType();
            Object[] objs = null;
            return type.InvokeMember(propertyName, System.Reflection.BindingFlags.GetProperty, null, this, objs);
        }
    }
}
