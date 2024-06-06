using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class Language
    {
        // Your code here

        // Language will have Id, Name, LanguageCode, CountryId (IsDefault and IsSelected are optional properties)
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public int CountryId { get; set; }
        
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        // Optional properties
        public bool IsDefault { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<LocalizationResource>? LocalizationResources { get; set; }
    }
}
