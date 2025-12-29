using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Movimiento")]
public partial class Movimiento
{
    [Key]
    public int Id { get; set; }

    public int? IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public int? IdAlmacen { get; set; }

    public int? IdMotivo { get; set; }

    public DateOnly? Fecha { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? SerieGuia { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NroGuia { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Detalles { get; set; }

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

    [ForeignKey("IdMotivo")]
    [InverseProperty("Movimientos")]
    public virtual Motivo? IdMotivoNavigation { get; set; }

    [InverseProperty("IdMovNavigation")]
    public virtual ICollection<MovimientoDetalle> MovimientoDetalles { get; set; } = new List<MovimientoDetalle>();
}
