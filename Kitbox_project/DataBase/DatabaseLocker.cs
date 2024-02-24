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
            
            return new Locker(12,12, 12, "doorColor", new Door("doorColor", "wood", 12, 12), 5.5);
        }
        public override void Save()
        {

        }
        public override void Update()
        {

        }
    }
}
