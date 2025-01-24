using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsDayScoring.Models;

public class Room {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public List<House> Houses { get; set; } = [];
}
