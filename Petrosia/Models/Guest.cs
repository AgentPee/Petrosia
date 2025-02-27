using System;
using System.Collections.Generic;

namespace Petrosia.Models;

public partial class Guest
{
    public int GuestId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string IdType { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
