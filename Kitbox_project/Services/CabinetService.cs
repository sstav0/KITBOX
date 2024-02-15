using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Je l'avais créé au cas où mais je l'ai pas utilisé en fait. A voir si on veut faire un truc clean 
//et utiliser des services et interfaces ou si balec on mets tous dans nos views/viewmodels
//Bref pour l'instant ce fichier est pas utilisé. On peut le supp s'il faut, je le garde au cas où


namespace Kitbox_project.Services
{
    public class CabinetService
    {
        public static bool AddLockerToCabinet(Locker locker, Cabinet cabinet)
        {
            // Check si on a pas plus de 7 lockers
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

