namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LanguageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int CountryId { get; set; }
    public CountryDto Country { get; set; }
    public List<LocalizationResourceDto> LocalizationResources { get; set; }
}