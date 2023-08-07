using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Models;
public class Category
{
    public Category()
    {
    }
    [Required]
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
    public string Name { get; set; } = string.Empty;
    [DisplayName("Display Order")]
    public int? DisplayOrder { get; set; }

}