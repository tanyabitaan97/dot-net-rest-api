using System.ComponentModel.DataAnnotations;
public class Student
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be 3-50 characters")]
    public required string Name { get; set; }

    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
    public int Age { get; set; }
}
