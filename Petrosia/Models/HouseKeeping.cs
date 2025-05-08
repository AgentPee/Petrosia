/*houseKeeping.cs*/


using System;
using System.Collections.Generic;

namespace Petrosia.Models;

public partial class HouseKeeping
{
    public int EmployeeId { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;
}
