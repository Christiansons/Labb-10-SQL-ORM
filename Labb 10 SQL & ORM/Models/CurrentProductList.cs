using System;
using System.Collections.Generic;

namespace Labb_10_SQL___ORM.Models;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
}
