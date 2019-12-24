using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public abstract class HasNextUrl {

        public virtual IEnumerable<object> GetList()
        {
            throw new NotImplementedException();
        }

        [JsonProperty("next_url")]
        public object NextUrl { get; set; }

    }

}
