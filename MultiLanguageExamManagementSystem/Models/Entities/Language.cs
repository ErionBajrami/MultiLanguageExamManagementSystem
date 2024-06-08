namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public int CountryId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSelected { get; set; }
        public Country Country { get; set; }
    }
}
