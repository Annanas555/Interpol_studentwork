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
    
    public partial class Lica_v_baze
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lica_v_baze()
        {
            this.Messages = new HashSet<Message>();
        }
    
        public int Kod { get; set; }
        public Nullable<int> Status { get; set; }
        public string F { get; set; }
        public string I { get; set; }
        public string O { get; set; }
        public string Pol { get; set; }
        public string Volosy { get; set; }
        public string Glaza { get; set; }
        public Nullable<int> Rost { get; set; }
        public Nullable<int> Ves { get; set; }
        public string Nazion { get; set; }
        public Nullable<System.DateTime> DR { get; set; }
        public string Mest_rozhd { get; set; }
        public byte[] ImageData { get; set; }
    
        public virtual Nation Nation { get; set; }
        public virtual Status Status1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
        public virtual Prestupniki Prestupniki { get; set; }
        public virtual Razyskivaemye Razyskivaemye { get; set; }
        public virtual Vybyvshie Vybyvshie { get; set; }
    }
}