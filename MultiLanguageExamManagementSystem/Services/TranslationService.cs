using System.Collections.Concurrent;
using Google.Cloud.Translation.V2;
using LifeEcommerce.Helpers.Models;
using Microsoft.IdentityModel.Tokens;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Helpers;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using Newtonsoft.Json;

namespace MultiLanguageExamManagementSystem.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TranslationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task TranslateForLanguage(Models.Entities.Language newLanguage, List<LocalizationResource> localizationResources)
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            var concurrentBag = new ConcurrentBag<LocalizationResource>();

            await Parallel.ForEachAsync(localizationResources, options,
                async (localizationResource, cancellationToken) => await AddResourceToLanguage(newLanguage, localizationResource, concurrentBag));
            foreach (var localizationResource in concurrentBag)
            {
                _unitOfWork.Repository<LocalizationResource>().Create(localizationResource);
            }
            _unitOfWork.Complete();
        }

        public async Task TranslateForResource(LocalizationResource localizationResource, List<Models.Entities.Language> language)
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            var concurrentBag = new ConcurrentBag<LocalizationResource>();
            await Parallel.ForEachAsync(language, options,
                async (language, cancellationToken) => await AddResourceToLanguage(language, localizationResource, concurrentBag));
            foreach (var localization in concurrentBag)
            {
                _unitOfWork.Repository<LocalizationResource>().Create(localization);
            }
            _unitOfWork.Complete();
        }
        private async Task AddResourceToLanguage(Models.Entities.Language language, LocalizationResource localizationResource, ConcurrentBag<LocalizationResource> bag)
        {
            var client = new HttpClient();
            var requestData = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("q", localizationResource.Value),
                new KeyValuePair<string, string>("source", LocalizationConstants.DefaultLanguageCode),
                new KeyValuePair<string, string>("target", language.LanguageCode)
            ]);

            var response = await client.PostAsync(LocalizationConstants.TranslateUrl, requestData);

            var responseData = await response.Content.ReadAsStringAsync();

            var translation = JsonConvert.DeserializeObject<TranslationResponse>(responseData);

            if (translation is null || string.IsNullOrEmpty(translation.TranslatedText))
            {
                var errorResponse = JsonConvert.DeserializeObject<TranslationErrorResponse>(responseData);
                throw new Exception(errorResponse?.Error);
            }

            var newLocalizationResource = new LocalizationResource
            {
                Key = localizationResource.Key,
                Namespace = localizationResource.Namespace,
                BeautifiedNamespace = localizationResource.BeautifiedNamespace,
                Value = translation.TranslatedText,
                Language = language,
            };

            bag.Add(newLocalizationResource);
        }


    }

}
