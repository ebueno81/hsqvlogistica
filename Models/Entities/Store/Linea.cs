using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Linea")]
public partial class Linea
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    [InverseProperty("IdLineaNavigation")]
    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}
