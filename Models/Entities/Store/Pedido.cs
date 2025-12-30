using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Pedido")]
public partial class Pedido
{
    [Key]
    public int Id { get; set; }

    public int? IdCliente { get; set; }

    public int? IdVendedor { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    [StringLength(300)]
    public string? Cliente { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Detalles { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    public int? Estado { get; set; }

    public int? IdEmpServ { get; set; }

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

    [InverseProperty("IdPedidoNavigation")]
    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
