using Microsoft.AspNetCore.Mvc;
using NewsWebMVC.Models;
using NewsWebMVC.ViewModels;
using Newtonsoft.Json;

namespace NewsWebMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;
        public static string _serverLink = "https://localhost:7087/";
        public static string ImagesPath = _serverLink + "/Images/News";
        //TODO:Handle Errors
        public NewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/News/{id}");
            if (response.IsSuccessStatusCode)
            {
                News news = await response.Content.ReadAsAsync<News>();
                return View(news);
            }
            return View();
        }
        [HttpGet]
        public async Task< IActionResult> Create()
        {
            CreateNewsViewModel viewModel = new();
            HttpResponseMessage response = await _httpClient.GetAsync(_serverLink + $"api/Author");
            if(response.IsSuccessStatusCode) 
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
                response = await _httpClient.GetAsync(_serverLink + $"api/Author");
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Authors = await response.Content.ReadAsAsync<List<Author>>();
                    return View(viewModel);
                }
                return View();//Handl erorr 
            }

            
            News news = new()
            {
                title = viewModel.title,
                AuthorId = viewModel.AuthorId,
                TheNews = viewModel.TheNews,
                PublicationDate = viewModel.PublicationDate

            };
            var image = Request.Form.Files;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_serverLink+"api/News");

                var formData = new MultipartFormDataContent();

                // Add the image file to the form data
                var imageContent = new StreamContent(imageFile.OpenReadStream());
                formData.Add(imageContent, "image", imageFile.FileName);

                // Add other data fields to the form data
                formData.Add(new StringContent(viewModel.title), "title");
                formData.Add(new Int32(viewModel.AuthorId), "AuthorId");

                // Send the request to the API endpoint
                var response = await client.PostAsync("api/SomeEndpoint", formData);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Process the successful response
                    // ...
                }
                else
                {
                    // Process the error response
                    // ...
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

            // Handle error case appropriately
            return View();
        }
    }
}
