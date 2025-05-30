﻿using System;
using System.Collections.Generic;

namespace Web_Shop_3.Persistence.MySQL.Model;

public partial class Cart
{
    public ulong IdCart { get; set; }

    public ulong IdCustomer { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Customer IdCustomerNavigation { get; set; } = null!;
}
