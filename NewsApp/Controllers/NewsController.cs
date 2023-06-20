using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Dtos;
using NewsApp.Helper;
using NewsApp.Models;
using NewsApp.Services;
using System;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsServices _newsServices;
        private readonly IAuthorServices _authorServices;
        private readonly IHostingEnvironment _hostingEnvironment;
        public NewsController(INewsServices newsServices, IAuthorServices authorServices, IHostingEnvironment hostingEnvironment)
        {
            _newsServices = newsServices;
            _authorServices = authorServices;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            return Ok(await _newsServices.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetNewsById(int id)
        {
            News news =  _newsServices.GetById(id);

            if (news == null)
                return NotFound($"Not found News with id = {id}");

            return Ok(news);
        }

        [HttpGet("GetByAuthorId")]
        public async Task<IActionResult> GetNewsByAuthorId(int authorId)
        {
            Author author = _authorServices.GetById(authorId);
            if (author == null)
                return NotFound($"No Author With Id = {authorId}");
            return Ok(await _newsServices.GetByAuthorId(authorId));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
                //TODO:return Errors
            }

            if (!_authorServices.IsExsists(dto.AuthorId))
                return BadRequest($"No Author found by id {dto.AuthorId}");

            if (!DateValidation.IsValidDateTime(dto.PublicationDate))
                return BadRequest("Date Allowed From Today untill Weak from Today");


            News news = new()
            {
                title = dto.title,
                TheNews = dto.TheNews,
                PublicationDate = dto.PublicationDate,
                AuthorId = dto.AuthorId,
                ImagePath = dto.ImagePath 
            };
            //news.ImagePath = ProcessUploadedFile(dto.Image);
            await _newsServices.Create(news);

            return Ok(news);
                
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateNewsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
                //TODO:Errors
            }
            News news = _newsServices.GetById(id);
            if (news == null)
                return BadRequest($"No News Found By Id = {id}");

            if (!_authorServices.IsExsists(dto.AuthorId))
                return BadRequest($"No Author found by id {dto.AuthorId}");

            
            
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
            if (news == null) return NotFound($"No News With Id = {id}");

            _newsServices.Delete(news);
            return Ok(news);
        }
        private string ProcessUploadedFile(IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "News");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }
       
    }
}
