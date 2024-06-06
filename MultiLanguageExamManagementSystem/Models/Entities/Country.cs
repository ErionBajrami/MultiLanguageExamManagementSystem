using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class Country
    {
        // Your code here
        // Caountry will have Id, Name and Code
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<Language> Languages { get; set; }
    }
}
