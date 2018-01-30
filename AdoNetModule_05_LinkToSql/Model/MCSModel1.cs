using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetModule_05_LinkToSql.Model
{
    class MCSModel1 : DataContext
    {
        public MCSModel1():base("Data Source=192.168.111.107;Initial Catalog=MCS; User ID=sa; Password=Mc123456")
        {
            
        }

        public Table<AccessTab> AccessTab { get; set; }
        public Table<AccessUser> AccessUser { get; set; }
    }
}
