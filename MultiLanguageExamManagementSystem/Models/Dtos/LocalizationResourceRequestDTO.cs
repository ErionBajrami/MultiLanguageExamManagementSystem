using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LocalizationResourceRequestDTO
{
    public int Id { get; set; }
    [MaxLength(200)]
    public string Key { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
    [MaxLength(200)]
    public string Namespace { get; set; }
    public string BeautifiedNamespace { get; set; }
}