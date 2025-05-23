using System;
using System.Collections.Generic;

namespace Web_Shop_3.Persistence.MySQL.Model;

public partial class CartItem
{
    public ulong IdCartItem { get; set; }

    public ulong IdCart { get; set; }

    public ulong IdProduct { get; set; }

    public virtual Cart IdCartNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
