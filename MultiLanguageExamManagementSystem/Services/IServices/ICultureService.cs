using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface ICultureService
    {
        LocalizationResource this[string locator] { get; }

        LocalizationResource GetString(string locator);

        string GetLocator(string @namespace, string key);


        Task<List<LanguageRequestDTO>> GetLanguages();

        Task<LanguageRequestDTO> GetLanguageById(int id);
        Task CreateLanguage(LanguageResponseDTO languageResponseDto);
        Task UpdateLanguage(int id, LanguageResponseDTO languageResponseDto);
        Task DeleteLanguage(int id);


        Task<List<LocalizationResourceRequestDTO>> GetLocalizationResources();
        Task<LocalizationResourceRequestDTO> GetLocalizationResourceById(int id);
        Task<List<LocalizationResourceRequestDTO>> GetLocalizationResourcesByLanguageId(int languageId);
        Task CreateLocalizationResource(LocalizationResourceResponseDTO resourceResponseDto);
        Task UpdateLocalizationResource(int id, LocalizationResourceResponseDTO resourceResponseDto);
        Task DeleteLocalizationResource(string @namespace, string key);

        // Your code here
        // methods for string localization, languages and localization resources
    }
}
