using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ImageApplication.Domain.Models
{
    //Classe de la BD 
    public class ImageRecord
    {
        [Key]
        [JsonProperty("id")]
        public Guid Id { get; set; } 
        public string Caption { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string CreatedBy { get; set; } //juste un string pour le moment
        
    }
}