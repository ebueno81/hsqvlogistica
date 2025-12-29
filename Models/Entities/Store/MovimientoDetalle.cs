using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("MovimientoDetalle")]
public partial class MovimientoDetalle
{
    [Key]
    public int Id { get; set; }

    public int? IdMov { get; set; }

    public int? IdArticulo { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PrecioMn { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PrecioUs { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Cantidad { get; set; }

    public bool? Activo { get; set; }

    [ForeignKey("IdArticulo")]
    [InverseProperty("MovimientoDetalles")]
    public virtual Articulo? IdArticuloNavigation { get; set; }

    [ForeignKey("IdMov")]
    [InverseProperty("MovimientoDetalles")]
    public virtual Movimiento? IdMovNavigation { get; set; }
}
