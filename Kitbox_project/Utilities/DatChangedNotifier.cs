using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Utilities
{
    public class DataChangedNotifier
    {
        public static event Action OnDataChanged;

        public static void NotifyDataChanged()
        {
            OnDataChanged?.Invoke();
        }
    }
}
