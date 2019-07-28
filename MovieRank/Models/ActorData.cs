using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRank.Models
{
    public class ActorData
    {

        public int AID { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public System.DateTime DOB { get; set; }
        public string Bio { get; set; }
        public string workedON { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movie> Movies { get; set; }

        public List<ActorData> getActorList { get; set; }
    }
}