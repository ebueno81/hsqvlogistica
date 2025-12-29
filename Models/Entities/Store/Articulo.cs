using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Articulo")]
public partial class Articulo
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Codigo { get; set; }

    public int? IdLinea { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    public int? Stock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrecioMn { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrecioUs { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Detalles { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? RutaImagen { get; set; }

    public bool? Activo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UsuaCreacion { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UsuaModifica { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaModifica { get; set; }

    [InverseProperty("IdArticuloNavigation")]
    public virtual ICollection<GarantiaDetalle> GarantiaDetalles { get; set; } = new List<GarantiaDetalle>();

    [ForeignKey("IdLinea")]
    [InverseProperty("Articulos")]
    public virtual Linea? IdLineaNavigation { get; set; }

    [InverseProperty("IdArticuloNavigation")]
    public virtual ICollection<MovimientoDetalle> MovimientoDetalles { get; set; } = new List<MovimientoDetalle>();

    [InverseProperty("IdArticuloNavigation")]
    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
