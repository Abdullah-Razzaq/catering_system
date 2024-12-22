using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Catering_Db_system.Types
{
    public class Customer
    {

        public int ID;
        public string Name;
        public override string ToString()
        {
            return Name; // This will return the Name property of the customer
        }

    }

    
}
