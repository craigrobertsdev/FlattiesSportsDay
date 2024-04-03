using System.ComponentModel.DataAnnotations;

namespace SportsDayScoring.Models;

public class Class {
    public int RoomNumber { get; set; }
    public List<House> Houses { get; set; } = [];
}
