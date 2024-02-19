using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.DataBase
{
    internal class DatabaseLocker : Database<Locker>
    {
        public DatabaseLocker() { }
        public override List<Locker> GetList()
        {
            return new List<Locker>();
        }
        public override Locker GetById()
        {
            Color doorColor = new Color();

            ;
            return new Locker(12, doorColor, new Door(doorColor, "wood"), 5.5);
        }
        public override void Save()
        {

        }
        public override void Update()
        {

        }
    }
}
