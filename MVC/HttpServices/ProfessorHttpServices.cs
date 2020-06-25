using Domain.Models.Interfaces.Services;
using Domain.Models.Models;
using Domain.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication14.HttpServices
{
    public class ProfessorHttpServices : IProfessorHttpServices
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<ProfessorHttpOptions> _professorHttpOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfessorHttpServices(
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<ProfessorHttpOptions> professorHttpOptions,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager)
        {
            _professorHttpOptions = professorHttpOptions ?? throw new ArgumentNullException(nameof(professorHttpOptions));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _signInManager = signInManager;
            ;

            _httpClient = httpClientFactory?.CreateClient(professorHttpOptions.CurrentValue.Name) ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient.Timeout = TimeSpan.FromMinutes(_professorHttpOptions.CurrentValue.Timeout);
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync(_professorHttpOptions.CurrentValue.ProfessorPath);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                await _signInManager.SignOutAsync();
                return null;
            }

            return JsonConvert.DeserializeObject<List<Professor>>(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task<Professor> GetByIdAsync(int id)
        {
            var pathWithId = $"{_professorHttpOptions.CurrentValue.ProfessorPath}/{id}";
            var httpResponseMessage = await _httpClient.GetAsync(pathWithId);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                //await _signInManager.SignOutAsync();
                return null;
            }

            return JsonConvert.DeserializeObject<Professor>(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public async Task<HttpResponseMessage> GetByIdHttpAsync(int id)
        {
            var pathWithId = $"{_professorHttpOptions.CurrentValue.ProfessorPath}/{id}";
            var httpResponseMessage = await _httpClient.GetAsync(pathWithId);

            return httpResponseMessage;
        }

        public async Task InsertAsync(Professor insertedEntity)
        {
            var uriPath = $"{_professorHttpOptions.CurrentValue.ProfessorPath}";

            var httpContent = new StringContent(JsonConvert.SerializeObject(insertedEntity), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient.PostAsync(uriPath, httpContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                await _signInManager.SignOutAsync();
            }
        }

        public async Task UpdateAsync(Professor updatedEntity)
        {
            var pathWithId = $"{_professorHttpOptions.CurrentValue.ProfessorPath}/{updatedEntity.Id}";

            var httpContent = new StringContent(JsonConvert.SerializeObject(updatedEntity), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient.PutAsync(pathWithId, httpContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                await _signInManager.SignOutAsync();
            }
        }

        public async Task DeleteAsync(Professor professor)
        {
            var pathWithId = $"{_professorHttpOptions.CurrentValue.ProfessorPath}/{professor.Id}";
            var httpResponseMessage = await _httpClient.DeleteAsync(pathWithId);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                await _signInManager.SignOutAsync();
            }
        }
    }
}
