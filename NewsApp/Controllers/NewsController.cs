using Microsoft.AspNetCore.Mvc;
using NewsApp.Dtos;
using NewsApp.Helper;
using NewsApp.Models;
using NewsApp.Services;
using System.Net;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsServices _newsServices;
        private readonly IAuthorServices _authorServices;
        public NewsController(INewsServices newsServices, IAuthorServices authorServices)
        {
            _newsServices = newsServices;
            _authorServices = authorServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            return Ok(await _newsServices.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetNewsById(int id)
        {
            News news = _newsServices.GetById(id);

            if (news == null)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "Id", new[] { $"Not found News with id = {id}" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }

            return Ok(news);
        }
        [HttpGet("GetByAuthorId")]
        public async Task<IActionResult> GetNewsByAuthorId(int authorId)
        {
            Author author = _authorServices.GetById(authorId);
            if (author == null)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "Id", new[] { $"No Author With Id = {authorId}" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }
            return Ok(await _newsServices.GetByAuthorId(authorId));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewsDto dto)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                return BadRequest(errors);
            }

            if (!_authorServices.IsExsists(dto.AuthorId))
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "AuthorId", new[] { $"No Author found by id {dto.AuthorId}" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }

            if (!DateValidation.IsValidDateTime(dto.PublicationDate))
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "PublicationDate", new[] { "Date Allowed From Today untill Weak from Today" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }
            News news = new()
            {
                title = dto.title,
                TheNews = dto.TheNews,
                PublicationDate = dto.PublicationDate,
                AuthorId = dto.AuthorId,
                ImagePath = dto.ImagePath
            };
            await _newsServices.Create(news);
            return Ok(news);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNewsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
                //TODO:Errors
            }
            News news = _newsServices.GetById(id);
            if (news == null)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "Id", new[] { $"Not found News with id = {id}" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }

            if (!_authorServices.IsExsists(dto.AuthorId))
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "AuthorId", new[] { $"No Author found by id {dto.AuthorId}" } }
                      }
                };

                return new BadRequestObjectResult(validationProblemDetails);
            }



            news.title = dto.title;
            news.TheNews = dto.TheNews;
            news.AuthorId = dto.AuthorId;
            news.PublicationDate = dto.PublicationDate;
            news.ImagePath = dto.ImagePath;

            _newsServices.Update(news);
            return Ok(news);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            News news = _newsServices.GetById(id);
            if (news == null)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = {
                   { "AuthorId", new[] { $"No News found by id {id}" } }
                   }
                };

                return new BadRequestObjectResult(validationProblemDetails);

            }

            _newsServices.Delete(news);
            return Ok(news);
        }
    }
}
