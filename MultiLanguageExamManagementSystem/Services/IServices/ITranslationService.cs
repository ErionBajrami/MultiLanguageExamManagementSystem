using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services.IServices;

public interface ITranslationService
{
    Task TranslateForLanguage(Language newLanguage, List<LocalizationResource> localizationResources);
    Task TranslateForResource(LocalizationResource localizationResource, List<Language> languages);

}