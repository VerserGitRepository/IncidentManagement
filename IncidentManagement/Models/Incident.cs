//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IncidentManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Incident
    {
        public int INC { get; set; }
        public string Enviroment { get; set; }
        public string TestCategory { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Location { get; set; }
        public string SoftwareVersion { get; set; }
        public string TestCase { get; set; }
        public string FeedBackSource { get; set; }
        [Required]
        public string Priority { get; set; }
      
        [DataType(DataType.MultilineText)]
        [Required]
        public string TestDetails { get; set; }
        public string Status { get; set; }
        public string TesterNote { get; set; }
        [DataType(DataType.MultilineText)]
        public string Investigation { get; set; }
        [DataType(DataType.MultilineText)]
        public string DevNotes { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string FilePath { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string RequestType { get; set; }
        public bool IsActive { get; set; }
    }
}