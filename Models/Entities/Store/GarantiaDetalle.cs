using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("GarantiaDetalle")]
public partial class GarantiaDetalle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    public int IdGarantia { get; set; }

    public int IdArticulo { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Cantidad { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Detalles { get; set; }

    public bool? Activo { get; set; }

    [ForeignKey("IdArticulo")]
    [InverseProperty("GarantiaDetalles")]
    public virtual Articulo IdArticuloNavigation { get; set; } = null!;

    [ForeignKey("IdGarantia")]
    [InverseProperty("GarantiaDetalles")]
    public virtual Garantium IdGarantiaNavigation { get; set; } = null!;
}
