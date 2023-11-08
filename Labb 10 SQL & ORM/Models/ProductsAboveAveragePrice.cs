using System;
using System.Collections.Generic;

namespace Labb_10_SQL___ORM.Models;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
