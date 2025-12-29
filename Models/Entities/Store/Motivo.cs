using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Motivo")]
public partial class Motivo
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? TipoMov { get; set; }

    public bool? Activo { get; set; }

    [InverseProperty("IdMotivoNavigation")]
    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
