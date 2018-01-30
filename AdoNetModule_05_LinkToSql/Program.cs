using AdoNetModule_05_LinkToSql.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace AdoNetModule_05_LinkToSql
{
    class Program
    {
        static MCSModel1 db = new MCSModel1(); 
        static void Main(string[] args)
        {
            //Exampl01();

            //Exampl02();

            //Example03();

            //Example04(db);

            //Example05();

            Example06();
        }
        static void Exampl01()
        {
            try
            {

                db.CommandTimeout = 30;

                Table<AccessTab> accessTables = db.GetTable<AccessTab>();
                AccessTab tab = accessTables.FirstOrDefault(f => f.intTabID == 56);
                tab.StrDescription = "some descr";

                db.SubmitChanges(ConflictMode.FailOnFirstConflict);

            }
            catch (ChangeConflictException ex)
            {
                Console.WriteLine(ex.Message);
                foreach (ObjectChangeConflict item in db.ChangeConflicts)
                {
                    MetaTable metatab = db.Mapping.GetTable(item.Object.GetType());

                    AdoNetModule_05_LinkToSql.AccessTab en = (AdoNetModule_05_LinkToSql.AccessTab)item.Object;

                    Console.WriteLine("Table name {0}: " + metatab.TableName);
                }
            }

            //foreach(AccessTab tab in accessTables)
            //{
            //    Console.WriteLine("Tab Name: " + tab.StrTabName);
            //}
        }


        //OTLOZHENNOE VYPOLNNIE ZAPROSOV
        static void Exampl02()
        {
            Table<AccessUser> users = db.GetTable<AccessUser>();

            var query = from u in users
                        where u.intUserId == 1
                        select
                        from t in u.AccessTabs
                        select new { u.intUserId, t.StrTabName };

            Console.WriteLine("-------------------------");
            db.Log = Console.Out;
            Console.WriteLine("-------------------------");
            MetaModel mm = db.Mapping;
            Console.WriteLine("-------------------------");

            //1 variant
            foreach (var item in query)
            {
                foreach (var item2 in item)
                {
                    Console.WriteLine(item2.intUserId + " - " + item2.StrTabName);
                }
            }

            //2 variant
            //foreach (var user in users)
            //{
            //    //peresylka dannyh tuda i obratno
            //    foreach(var tab in user.AccessTabs)
            //    {
            //        Console.WriteLine(user.intUserId + " - " + tab.StrTabName);
            //    }
            //}
        }


        //db connection
        static void Example03()
        {

            Console.WriteLine("connection: {0}", db.Connection);
            Console.WriteLine("connection: {0}", db.Connection.ConnectionString);
            Console.WriteLine("connection: {0}", db.Connection.ConnectionTimeout);
            Console.WriteLine("connection: {0}", db.Connection.Database);
            Console.WriteLine("connection: {0}", db.Connection.DataSource);

            
        }

        static void Example04(MCSModel1 dataContext)
        {
            Table<AccessTab> tabs = dataContext.GetTable<AccessTab>();
            Table<AccessUser> users = dataContext.GetTable<AccessUser>();

            db.Refresh(RefreshMode.KeepChanges);

            AccessTab a = tabs.OrderBy(o => o.StrTabName).First(f => f.intTabID == 56);
        }

        static void Example05()
        {
            Table<AccessTab> tabs = db.GetTable<AccessTab>();
            AccessTab aTab = tabs.FirstOrDefault(f => f.intTabID == 1);
            aTab.StrDescription = "Test 005";

            Table<AccessUser> accessUsers = db.GetTable<AccessUser>();
            AccessUser aUser = accessUsers.FirstOrDefault(f => f.intAccessId == 6822);
            aUser.dCreated = DateTime.Now;
            //aUser.intTabID

            try
            {
                using (System.Transactions.TransactionScope scope =
                    new System.Transactions.TransactionScope())
                {
                    db.SubmitChanges();
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                db.Refresh(RefreshMode.OverwriteCurrentValues, tabs);
                Console.WriteLine("StrDescription: {0}", aTab.StrDescription);

                db.Refresh(RefreshMode.OverwriteCurrentValues, accessUsers);
                Console.WriteLine("dCreated: {0}", aUser.dCreated);
            }

            
        }

        static void Example06()
        {
            IEnumerable<AccessTab> data = db.ExecuteQuery<AccessTab>("select * from AccessTab where strTabName = {0}","Отчет");

            IEnumerable<AccessTab> data1 = db.ExecuteQuery<AccessTab>("UPDATE AccessTab set strTabName ={0} where strTabName = {1}", "НеОтчет", "Отчеты");

            foreach (var item in data)
            {
                Console.Write(item.intTabID);
            }
        }
    }
}
