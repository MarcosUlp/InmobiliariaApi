namespace InmobiliariaApi.Dtos
{
    public class CrearInmuebleDto
    {
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public IFormFile? Imagen { get; set; } // Para subir imagen
    }

    public class InmuebleDto
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public bool Habilitado { get; set; }
        public string? Imagen { get; set; }
    }
}
