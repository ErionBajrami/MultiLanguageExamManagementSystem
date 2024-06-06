namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LocalizationResourceDto
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
    public LanguageDto Language { get; set; }
}