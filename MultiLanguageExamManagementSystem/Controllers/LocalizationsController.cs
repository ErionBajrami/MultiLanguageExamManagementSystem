using AutoMapper;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services;
using MultiLanguageExamManagementSystem.Services.IServices;
using Language = Google.Cloud.Translation.V2.Language;

namespace MultiLanguageExamManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalizationsController : ControllerBase
    {
        private readonly ILogger<LocalizationsController> _logger;
        private readonly ICultureService _cultureService;
        private readonly TranslationService _translationService;
        private readonly IMapper _mapper;

        public LocalizationsController(ILogger<LocalizationsController> logger, ICultureService cultureService, TranslationService translationService, IMapper mapper)
        {
            _logger = logger;
            _cultureService = cultureService;
            _translationService = translationService;
            _mapper = mapper;
        }

        // Your code here

        [HttpGet(Name = "GetLocalizationResource")]
        public async Task<IActionResult> GetLocalizationResource([FromQuery] string key)
        {
            // implement the logic that allows us to call the culture service like this, note that the string we are 
            // sending "ne.1" it is of form namespace.key, in this case "ne" is the namespace and "1" is the key
            // so you should return back the localization resource that is having this namespace and key and the
            // language code based in the request header

            //var message = _cultureService["ne.1"];
            //var message = _cultureService.GetString("ne.1").Value; // Implement this too

            //return Ok("hello");

            var languageCode = Request.Headers["Accept-Language"].ToString();
            var message = await _cultureService.LocalizeString(key, languageCode);

            return Ok(message);
        }

        [HttpPost("AddLanguage")]
        public async Task<IActionResult> AddLanguage([FromBody] LanguageDto languageDto)
        {
            var existingLanguage = await _cultureService.GetLanguageByCode(languageDto.LanguageCode);
            if (existingLanguage != null)
            {
                return BadRequest("Language already exists.");
            }

            var localizationResources = await _cultureService.GetAllLocalizationResources();
            foreach (var resource in localizationResources)
            {
                var translation = await _translationService.TranslateText(resource.Value, languageDto.LanguageCode);
                var newResource = new LocalizationResource()
                {
                    Key = resource.Key,
                    Value = translation,
                    LanguageId = languageDto.Id
                };

                _cultureService.AddLocalizationResource(newResource);
            }

            _cultureService.AddLanguage(languageDto);

            return Ok(languageDto);
        }
        
        
        
        
        // var existingLanguage = await _cultureService.GetLanguageByCode(languageDto.LanguageCode);
        // if (existingLanguage != null)
        // {
        //     return BadRequest("Language already exists.");
        // }
        //
        // var localizationResource = await _cultureService.GetAllLocalizationResources();
        //
        // foreach (var resource in localizationResource)
        // {
        //     var translate = await _translationService.TranslateText(resource.Value, languageDto.LanguageCode);
        //     var newRes = new LocalizationResource()
        //     {
        //         Key = resource.Key,
        //         Value = translate,
        //         LanguageId = languageDto.Id
        //     };
        //     
        //     _cultureService.AddLocalizationResource(newRes);
        // }
        //
        // var language = _mapper.Map<Language>(languageDto);
        // _cultureService.AddLanguage(language);
        //
        // return Ok(language);

        // Your code here
        // Implement endpoints for crud operations (no relation to localization needed here, just normal cruds)
        // Except when adding a new language(or localization resource), you should get all the existing localization resources in english
        // prepare and translate them to the new language using translation service which should use this api for translations:
        // https://www.deepl.com/pro-api?cta=header-pro-api, and then seed the new created localizations for the new added language

        // same applies when adding a new localization resource, for example you implement the code for adding a new
        // language resource, then you call the api to prepare the translated resource to all your existing languages
        // and then you add the same resource for all the languages
    }
}
