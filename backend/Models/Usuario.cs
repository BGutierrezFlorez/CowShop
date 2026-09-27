using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CowShop.Models
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80, ErrorMessage = "El nombre no puede exceder 80 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(20, ErrorMessage = "La cédula no puede exceder 20 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(320, ErrorMessage = "El correo no puede exceder 320 caracteres")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio")]
        [StringLength(10, ErrorMessage = "El celular debe tener 10 caracteres", MinimumLength = 10)]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es obligatorio")]
        [StringLength(20, ErrorMessage = "El tipo de usuario no puede exceder 20 caracteres")]
        public string Tipo_Usuario { get; set; }

        [Required(ErrorMessage = "La membresía es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una membresía válida")]
        public int ID_Membresia { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder 100 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un rol válido")]
        public int ID_Rol { get; set; }

        // Campos de auditoría
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaModificacion { get; set; }

        public bool Estado { get; set; }

        // Propiedades de solo lectura para visualización
        [Display(Name = "Estado")]
        public string EstadoDisplay => Estado ? "Activo" : "Inactivo";

        [Display(Name = "Fecha de Creación")]
        public string FechaCreacionDisplay => FechaCreacion.ToString("dd/MM/yyyy HH:mm");

        [Display(Name = "Fecha de Modificación")]
        public string FechaModificacionDisplay => FechaModificacion.ToString("dd/MM/yyyy HH:mm");

        // Constructor para valores por defecto
        public Usuario()
        {
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
            Estado = true;
            ID_Membresia = 1; // Membresía básica por defecto
            ID_Rol = 2; // Rol de Usuario estándar por defecto
            Tipo_Usuario = "Comprador"; // Valor por defecto
        }

        // Método para actualizar fecha de modificación
        public void ActualizarFechaModificacion()
        {
            FechaModificacion = DateTime.Now;
        }

        // Método para desactivar usuario (soft delete)
        public void Desactivar()
        {
            Estado = false;
            ActualizarFechaModificacion();
        }

        // Método para activar usuario
        public void Activar()
        {
            Estado = true;
            ActualizarFechaModificacion();
        }
    }
}