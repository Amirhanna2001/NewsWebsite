using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsWebMVC.Models;
using NewsWebMVC.ViewModels;
using Newtonsoft.Json;

namespace NewsWebMVC.Controllers
{
    public class AuthorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<IdentityUser> _userManager;
        private string _ServireLink = "https://localhost:7087/";
        public AuthorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_ServireLink + "api/Author");

            if (response.IsSuccessStatusCode)
            {
                var authors = await response.Content.ReadAsAsync<List<Author>>();
                return View(authors);
            }

            return View("Error"); 
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthor(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_ServireLink + $"api/Author/{id}");

            if (response.IsSuccessStatusCode)
            {
                Author author = await response.Content.ReadAsAsync<Author>();
                return View(author);
            }

            string errorResponse = await response.Content.ReadAsStringAsync();
            List<string> errors = new()
            {
                errorResponse
            };
           
            return View("Error", errors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorViewModel viewModel)
        {
            Author author = new()
            {
                Name = viewModel.Name
            };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_ServireLink + "api/Author", author);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    ModelState.AddModelError("Name", errorMessage);
                }
            }
            return View(author);
        }
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_ServireLink + $"api/Author/{id}");

            if (response.IsSuccessStatusCode)
            {
                var author = await response.Content.ReadAsAsync<Author>();
                return View(author);
            }

           return  View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_ServireLink + $"api/Author/{id}", author);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    ModelState.AddModelError("Name", errorMessage);
                }
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_ServireLink + $"api/Author/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            string errorResponse = await response.Content.ReadAsStringAsync();

            List<string> errors = new() { errorResponse};
           
            return View("Error", errors);
        }

    }
}
