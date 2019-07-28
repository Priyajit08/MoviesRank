namespace MovieRank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    public partial class TComment
    {
        public int CID { get; set; }
        [DisplayName("TV Shows")]
        public int TID { get; set; }
        [DisplayName("Review")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public virtual TVShow TVShow { get; set; }
    }
}