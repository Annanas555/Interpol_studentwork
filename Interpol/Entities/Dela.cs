//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Interpol.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dela
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dela()
        {
            this.Razyskivaemyes = new HashSet<Razyskivaemye>();
        }
    
        public int Kod { get; set; }
        public Nullable<int> Sledovatel { get; set; }
        public Nullable<int> Razyskivaemiy { get; set; }
        public Nullable<int> Statiya { get; set; }
    
        public virtual Sledovateli Sledovateli { get; set; }
        public virtual Statiyi Statiyi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Razyskivaemye> Razyskivaemyes { get; set; }
    }
}