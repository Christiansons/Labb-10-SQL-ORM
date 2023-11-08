using System;
using System.Collections.Generic;

namespace Labb_10_SQL___ORM.Models;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
