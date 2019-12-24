using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pixeez.Objects
{
    public interface IHasUser
    {
        User User { get; set; }
    }

}
