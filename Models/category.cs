//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FluffNStuff.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            this.sub_category = new HashSet<sub_category>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_image { get; set; }
        public Nullable<int> admin_id_fk { get; set; }
        public Nullable<int> cat_status { get; set; }
    
        public virtual admintable admintable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sub_category> sub_category { get; set; }
    }
}