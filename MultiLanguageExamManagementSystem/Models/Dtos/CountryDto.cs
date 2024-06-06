namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<LanguageDto> Languages { get; set; }
}