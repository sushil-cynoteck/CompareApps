//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompareApps.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public System.Guid FromUserRef { get; set; }
        public System.Guid ToUserRef { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int ReplyToMessageId { get; set; }
        public bool IsReplied { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ReadOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
}
