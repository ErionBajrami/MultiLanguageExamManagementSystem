using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;

namespace MultiLanguageExamManagementSystem.Services
{
    public class TranslationService
    {
        // Your code here

        // You can make it static, add interface or whatever u want

        //https://www.deepl.com/pro-api?cta=header-pro-api - check the documentation

        private readonly TranslationClient _client;

        public TranslationService(string credentialsPath)
        {
            GoogleCredential credential = GoogleCredential.FromFile(credentialsPath);
            _client = TranslationClient.Create(credential);
        }

        public async Task<string> TranslateText(string text, string targetLanguage)
        {
            var response = await _client.TranslateTextAsync(text, targetLanguage);
            return response.TranslatedText;
        }
    }
}
