//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieRank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class News
    {
        public int NID { get; set; }
        [DisplayName("News")]
        public string News1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd / MMM / yyyy}")]
        public System.DateTime Date { get; set; }
        public string Images { get; set; }
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
