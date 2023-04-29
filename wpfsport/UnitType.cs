using System;
using System.Collections.Generic;

namespace wpfsport;

public partial class UnitType
{
    public int UnitTypeId { get; set; }

    public string? UnitTypeName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
