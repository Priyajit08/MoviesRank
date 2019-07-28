using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRank.Models
{
    public class TVShowsData
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string CastID { get; set; }
        public System.DateTime YearOfRelease { get; set; }
        public int Rating { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public List<TVShowsData> getTVShowsList { get; set; }
    }
}