using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("PedidoDetalle")]
public partial class PedidoDetalle
{
    [Key]
    public int Id { get; set; }

    public int? IdPedido { get; set; }

    public int? IdArticulo { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PrecioMn { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PrecioUs { get; set; }

    public bool? Activo { get; set; }

    [ForeignKey("IdArticulo")]
    [InverseProperty("PedidoDetalles")]
    public virtual Articulo? IdArticuloNavigation { get; set; }

    [ForeignKey("IdPedido")]
    [InverseProperty("PedidoDetalles")]
    public virtual Pedido? IdPedidoNavigation { get; set; }
}
