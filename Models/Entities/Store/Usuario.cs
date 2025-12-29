using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Models.Entities.Store;

[Table("Usuario")]
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    [Column("Usuario")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Usuario1 { get; set; }

    [StringLength(100)]
    public string? Nombres { get; set; }

    [StringLength(50)]
    public string? Correo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Clave { get; set; }

    public int? IdTipo { get; set; }

    public bool? Activo { get; set; }

    [ForeignKey("IdTipo")]
    [InverseProperty("Usuarios")]
    public virtual TipoUsuario? IdTipoNavigation { get; set; }
}
