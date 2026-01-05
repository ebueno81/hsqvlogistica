using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

public partial class Garantium
{
    [Key]
    public int Id { get; set; }

    public int IdCliente { get; set; }

    [StringLength(300)]
    public string? Cliente { get; set; }

    public int IdEmpServ { get; set; }

    [StringLength(200)]
    public string? EmpServ { get; set; }

    public DateOnly? FechaDespacho { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    [StringLength(10)]
    public string? NroSerie { get; set; }

    [StringLength(10)]
    public string? NroGuia { get; set; }

    public bool Estado { get; set; }

    [StringLength(250)]
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

    [InverseProperty("IdGarantiaNavigation")]
    public virtual ICollection<GarantiaDetalle> GarantiaDetalles { get; set; } = new List<GarantiaDetalle>();
}
