using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using AdoNetModule_05_LinkToSql.Model;

namespace AdoNetModule_05_LinkToSql
{
    [Table(Name = "AccessTab")]
    public class AccessTab
    {
        [Column(IsPrimaryKey =true, IsDbGenerated =true)]
        public int intTabID { get; set; }

        //[Column(Name ="strName")]
        //public string StrName { get; set; }

        [Column(Name = "strTabName")]
        public string StrTabName { get; set; }

        [Column(Name ="strTabUrl")]
        public string StrTabUrl { get; set; }

        [Column(Name ="strDescription")]
        public string StrDescription { get; set; }
        
        private string StrTabGroupName;
        [Column(Name = "StrTabGroupName")]

        public string strTabGroupName { get { return StrTabGroupName; }set { StrTabGroupName = value; } }

        [Association(ThisKey = "intTabID", OtherKey = "intTabID")]
        public EntitySet<AccessUser> AccessUsers { get; set; }
    }
}
