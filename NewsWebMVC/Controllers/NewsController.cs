using Microsoft.AspNetCore.Mvc;
using NewsWebMVC.Helper;
using NewsWebMVC.Models;
using NewsWebMVC.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;

namespace NewsWebMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly SotoringAndDeleteFiles _files;

        public static string _serverLink = "https://localhost:7087/";
        public static string ImagesPath = "/Images/News";
        //TODO:Handle Errors
        public NewsController(HttpClient httpClient, SotoringAndDeleteFiles files)
        {
            _httpClient = httpClient;
            _files = files;
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + "api/News");
            if (response.IsSuccessStatusCode)
            {
                List<News> news = await response.Content.ReadAsAsync<List<News>>();
                return View(news);
            }

            return View();
        }
        public async Task<IActionResult> FilterByAuthor(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/News/GetByAuthorId?authorId={id}");
            if (response.IsSuccessStatusCode)
            {
                List<News> news = await response.Content.ReadAsAsync<List<News>>();
                return View(nameof(Index), news);
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            List<string> errors = new();
            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    errors.Add(errorMessage);
                }
            }
            return View("Error", errors);
        }


        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/News/{id}");
            News news;
            if (response.IsSuccessStatusCode)
            {
                news = await response.Content.ReadAsAsync<News>();
                return View(news);
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            List<string> errors = new();
            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    errors.Add(errorMessage);
                }
            }
            return View("Error", errors);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateNewsViewModel viewModel = new();
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/Author");
            if (response.IsSuccessStatusCode)
            {
                viewModel.Authors = await response.Content.ReadAsAsync<List<Author>>();
                return View(viewModel);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsViewModel viewModel)
        {
            HttpResponseMessage response;
            if (!ModelState.IsValid)
            {
                viewModel.Authors = await GetAllAuthors();
                return View(viewModel);
            }

            News news = new()
            {
                title = viewModel.title,
                AuthorId = viewModel.AuthorId,
                TheNews = viewModel.TheNews,
                PublicationDate = viewModel.PublicationDate
            };

            if (!ImageValidation.IsValidSize(viewModel.Image))
            {
                ModelState.AddModelError("Image", "Max Allowed Image Size is 1MB");
                viewModel.Authors = await GetAllAuthors();
                return View(viewModel);
            }

            if (!ImageValidation.IsAllowedExtention(viewModel.Image))
            {
                ModelState.AddModelError("Image", "Only Allowed Extensions Are .PNG & .JPG");
                viewModel.Authors = await GetAllAuthors();
                return View(viewModel);
            }

            news.ImagePath = _files.ProcessUploadedFile(viewModel.Image);

            response = await _httpClient.PostAsJsonAsync(_serverLink + "api/News", news);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

                ModelState.Clear();
                foreach (var error in errorObject.Errors)
                {
                    foreach (var errorMessage in error.Value)
                    {
                        ModelState.AddModelError(error.Key, errorMessage);
                    }
                }
                viewModel.Authors = await GetAllAuthors();
                return View(viewModel);
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/News/{id}");

            if (response.IsSuccessStatusCode)
            {
                News news = await response.Content.ReadAsAsync<News>();
                EditNewsViewModel viewModel = new()
                {
                    title = news.title,
                    AuthorId = news.AuthorId,
                    TheNews = news.TheNews,
                    ImagePath = news.ImagePath,
                    PublicationDate = news.PublicationDate,
                };
                response = await _httpClient.GetAsync(_serverLink + $"api/Author");
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Authors = await response.Content.ReadAsAsync<List<Author>>();
                    return View(viewModel);
                }

                return View(viewModel);//TODO:

            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            List<string> errors = new();
            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    errors.Add(errorMessage);
                }
            }
            return View("Error", errors);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditNewsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
                //TODO:Errors
            }
            News news = new();


            if (viewModel.Image != null)
            {
                if (!ImageValidation.IsValidSize(viewModel.Image))
                    return View("Max Allowed Image Size is 1MB");
                if (!ImageValidation.IsAllowedExtention(viewModel.Image))
                    return View("Only Allowed Extentions Are .PNG & .JPG");
                Console.WriteLine(viewModel.ImagePath);
                _files.DeleteImage(viewModel.ImagePath);
                viewModel.ImagePath = _files.ProcessUploadedFile(viewModel.Image);
            }
            news.ImagePath = viewModel.ImagePath;
            news.title = viewModel.title;
            news.TheNews = viewModel.TheNews;
            news.AuthorId = viewModel.AuthorId;
            news.PublicationDate = viewModel.PublicationDate;

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_serverLink + $"api/News/{id}", news);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            string errorContent = await response.Content.ReadAsStringAsync();
            ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

            foreach (var error in errorResponse.Errors)
            {
                string fieldName = error.Key;
                List<string> errorMessages = error.Value;

                foreach (var errorMessage in errorMessages)
                {
                    ModelState.AddModelError(fieldName, errorMessage);
                }
            }
            response = await _httpClient.GetAsync(_serverLink + $"api/Author");
            if (response.IsSuccessStatusCode)
            {
                viewModel.Authors = await response.Content.ReadAsAsync<List<Author>>();
                return View(viewModel);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_serverLink + $"api/News/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);

            List<string> errors = new();
            foreach (var error in errorObject.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    errors.Add(errorMessage);
                }
            }
            return View("Error", errors);
        }
        private async Task<IEnumerable<Author>> GetAllAuthors()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + "api/Author");
            return (await response.Content.ReadAsAsync<List<Author>>());
        }

    }
}