using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.DataBase
{
    internal class DatabaseDoor : Database<Door>
    {
        public DatabaseDoor() { }
        public override List<Door> GetList()
        {
            return new List<Door>();
        }
        public override Door GetById()
        {
            Color doorColor  = new Color();
            
            return new Door("doorColor", "wood", 12, 12);
        }
        public override void Save()
        {

        }
        public override void Update()
        {

        }
    }
}

