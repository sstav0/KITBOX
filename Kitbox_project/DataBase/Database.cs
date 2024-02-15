using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.DataBase
{
    internal abstract class Database<T>
    {
        protected Database() { }

        protected string user = "root";
        protected string password = "root";
        protected string tableDoor = "Door";
        protected string tableCustomer = "Customer";
        protected string tableLocker = "Locker";
        protected string tableCabinet = "Cabinet";


        public abstract IEnumerable<T> GetList();
        public abstract T GetById();
        public abstract void Save();
        public abstract void Update();


    }
}
