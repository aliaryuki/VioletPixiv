using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VioletPixiv
{
    public class Locker
    {
        // Default is UnLock
        private bool _IsLock = false;
        public bool IsLock
        {
            get
            {
                return _IsLock;
            }

            private set
            {
                _IsLock = value;
            }
        }

        public void Lock()
        {
            this.IsLock = true;
        }

        public void UnLock()
        {
            this.IsLock = false;
        }
    }
}
