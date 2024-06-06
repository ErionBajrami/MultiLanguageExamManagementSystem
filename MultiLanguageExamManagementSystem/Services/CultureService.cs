using LifeEcommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
//using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using System.Globalization;
using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;

namespace MultiLanguageExamManagementSystem.Services
{
    public class CultureService : ICultureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CultureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Your code here

        #region String Localization

        // String localization methods implementation here
        public async Task<string> LocalizeString(string key, string languageCode)
        {
            var localization = await _unitOfWork.Repository<LocalizationResource>()
                .GetByCondition(l => l.Key == key && l.Language.LanguageCode == languageCode)
                .Include(x => x.Language)
                .FirstOrDefaultAsync();

            return localization?.Value ?? key;
        }

        #endregion

        #region Languages

        // language methods implementation here

        public async Task<IEnumerable<Language>> GetAllLanguages()
        {
            return await _unitOfWork.Repository<Language>().GetAll().ToListAsync();
        }
        
        public async Task<Language> GetLanguageById(int id)
        {
            return await _unitOfWork.Repository<Language>().GetById(x => x.Id == id).FirstOrDefaultAsync();
        }
        
        public async Task<Language> GetLanguageByCode(string languageCode)
        {
            return await _unitOfWork.Repository<Language>()
                .GetByCondition(l => l.LanguageCode == languageCode)
                .FirstOrDefaultAsync();
        }
         
        public void AddLanguage(LanguageDto languageDto)
        {
            
            var language = _mapper.Map<Language>(languageDto);
            _unitOfWork.Repository<Language>().Create(language);
            _unitOfWork.Complete();
                // _unitOfWork.Repository<Language>().Create(language);
                // _unitOfWork.Complete();
        }

        public void UpdateLanguage(Language language)
        {
            _unitOfWork.Repository<Language>().Update(language);
            _unitOfWork.Complete();
        }

        public async Task DeleteLanguage(int id)
        {
            var existingLanguage = await GetLanguageById(id);

            if (existingLanguage != null)
            {
                var resource = await _unitOfWork.Repository<LocalizationResource>()
                    .GetByCondition(x => x.LanguageId == existingLanguage.Id)
                    .ToListAsync();
                
                _unitOfWork.Repository<LocalizationResource>().DeleteRange(resource);
                _unitOfWork.Repository<Language>().Delete(existingLanguage);
                _unitOfWork.Complete();
            }
        }
        
        #endregion

        #region Localization Resources

        // localization resource methods implementation here

        public async Task<IEnumerable<LocalizationResource>> GetAllLocalizationResources()
        {
            return await _unitOfWork.Repository<LocalizationResource>().GetAll().ToListAsync();
        }

        public async Task<LocalizationResource> GetLocalizationByIdAsync(int id)
        {
            return await _unitOfWork.Repository<LocalizationResource>().GetById(x => x.Id == id).FirstOrDefaultAsync();
        }

        public void AddLocalizationResource(LocalizationResource localization)
        {
            _unitOfWork.Repository<LocalizationResource>().Create(localization);
            _unitOfWork.Complete();
        }

        public void UpdateLocalization(LocalizationResource localization)
        {
            _unitOfWork.Repository<LocalizationResource>().Update(localization);
            _unitOfWork.Complete();
        }

        public async Task DeleteLocalization(int id)
        {
            var localization = await GetLocalizationByIdAsync(id);

            if (localization != null)
            {
                _unitOfWork.Repository<LocalizationResource>().Delete(localization);
                _unitOfWork.Complete();
            }
        }
            
        #endregion
    }
}
