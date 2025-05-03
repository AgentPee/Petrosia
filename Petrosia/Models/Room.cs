using System;
using System.Collections.Generic;

namespace Petrosia.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int GuestId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Guest Guest { get; set; } = null!;

    public virtual ICollection<HouseKeeping> HouseKeepings { get; set; } = new List<HouseKeeping>();
}
