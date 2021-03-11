using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace pet_hotel
{
    public enum PetBreedType
    {
        Shepherd,
        Poodle,
        Beagle,
        Bulldog,
        Terrier,
        Boxer,
        Labrador,
        Retriever,

    }
    public enum PetColorType
    {
        White,
        Black,
        Golden,
        Tricolor,
        Spotted

    }
    public class Pet
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public PetBreedType breed { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetColorType color { get; set; }

        public DateTime? checkedInAt { get; set; }

        public PetOwner PetOwner { get; set; }
        [Required]
        [ForeignKey("PetOwners")]
        public int petOwnerid { get; set; }

        public void checkin() {
            this.checkedInAt = DateTime.Now;
        }

        public void checkout() {
            this.checkedInAt = null;
        }
    }
}
