using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdressApp.AppData;

namespace AdressApp.AppData
{
    public partial class KLADR
    {
        public string FullName
        {
            get
            {
                return $"{this.SOCR} {this.NAME}";
            }
        }
    }
    
    public partial class STREET
    {
        public string FullName
        {
            get
            {
                return $"{this.SOCR} {this.NAME}";
            }
        }
    }

    public partial class DOMA
    {
        public string FullName
        {
            get
            {
                return $"{this.SOCR} {this.NAME}";
            }
        }
    }
}
