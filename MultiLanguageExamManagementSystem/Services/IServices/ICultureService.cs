using LifeEcommerce.Helpers;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

//using MultiLanguageExamManagementSystem.Models.Dtos;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface ICultureService
    {
        // Your code here
        // methods for string localization, languages and localization resources

        #region String Localization

        Task<string> LocalizeString(string key, string languageCode);
        
        #endregion

        #region Language

        Task<IEnumerable<Language>> GetAllLanguages();
        Task<Language> GetLanguageById(int id);
        Task<Language> GetLanguageByCode(string languageCode);
        void AddLanguage(LanguageDto languageDto);
        void UpdateLanguage(Language language);
        Task DeleteLanguage(int id);

        #endregion

        #region Localization Resources

        Task<IEnumerable<LocalizationResource>> GetAllLocalizationResources();
        Task<LocalizationResource> GetLocalizationByIdAsync(int id);
        void AddLocalizationResource(LocalizationResource localization);
        void UpdateLocalization(LocalizationResource localization);
        Task DeleteLocalization(int id);

        #endregion
    }
}
