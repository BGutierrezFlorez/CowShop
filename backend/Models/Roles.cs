using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CowShop.Models
{
    [Table("Roles")] // Nombre de la tabla en la base de datos
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID del Rol")]
        public int ID_Rol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Rol")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s0-9\-]+$", ErrorMessage = "El nombre contiene caracteres no válidos")]
        [NoDateValidation(ErrorMessage = "El nombre no puede ser una fecha")]
        public string NombreRol { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string DescripcionRol { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Modificación")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaModificacion { get; set; }

        // Propiedad de navegación para la relación con usuarios (si existe)
        public virtual ICollection<Usuario> Usuarios { get; set; }

    
        // Método para validar que el nombre no sea una fecha
        public bool EsNombreValido()
        {
            return !EsFecha(NombreRol);
        }

        // Método auxiliar para detectar formatos de fecha
        private bool EsFecha(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return false;

            // Patrones comunes de fecha
            string[] formatosFecha = {
                "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
                "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd", "yyyy-M-d",
                "dd/MM/yy", "d/M/yy", "dd-MM-yy", "d-M-yy"
            };

            return DateTime.TryParseExact(valor, formatosFecha,
                       System.Globalization.CultureInfo.InvariantCulture,
                       System.Globalization.DateTimeStyles.None, out _);
        }

        // Método para actualizar la fecha de modificación
        public void ActualizarFechaModificacion()
        {
            FechaModificacion = DateTime.Now;
        }

        // Override del método ToString para representación en texto
        public override string ToString()
        {
            return $"{NombreRol}  - {(Estado ? "Activo" : "Inactivo")}";
        }
    }

    // Atributo personalizado para validación de no fecha
    public class NoDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            string valor = value.ToString();

            // Verificar si es una fecha
            string[] formatosFecha = {
                "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
                "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd", "yyyy-M-d",
                "dd/MM/yy", "d/M/yy", "dd-MM-yy", "d-M-yy"
            };

            if (DateTime.TryParseExact(valor, formatosFecha,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _))
            {
                return new ValidationResult(ErrorMessage ?? "El campo no puede ser una fecha");
            }

            return ValidationResult.Success;
        }
    }

}