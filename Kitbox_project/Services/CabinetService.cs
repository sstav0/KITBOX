using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kitbox_project.Services
{
    public class CabinetService
    {
        public static bool AddLockerToCabinet(Locker locker, Cabinet cabinet)
        {
            // Check if adding the locker exceeds the maximum limit
            if (cabinet.lockers.Count < 7)
            {
                // Add the locker to the cabinet
                cabinet.lockers.Add(locker);
                return true; // Added successfully
            }
            else
            {
                return false; // Exceeds the maximum limit
            }
        }
    }
}

