using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.ModelsStore;

[Table("Configuracion")]
[Index("Codigo", Name = "IX_Configuracion_Codigo", IsUnique = true)]
public partial class Configuracion
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Codigo { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string Valor { get; set; } = null!;

    public bool Activo { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string UsuaCreacion { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaCreacion { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? UsuaModifica { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaModifica { get; set; }
}
