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
    
    public partial class Vybyvshie
    {
        public int Kod { get; set; }
        public Nullable<int> Prichina_vyb { get; set; }
    
        public virtual Lica_v_baze Lica_v_baze { get; set; }
        public virtual Prichiny_vyb Prichiny_vyb { get; set; }
    }
}
