using Humanizer;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Dtos;
using NewsApp.Models;
using NewsApp.Services;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorServices _serviceType;

        public AuthorController(IAuthorServices serviceType)
        {
            _serviceType = serviceType;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            return Ok(await _serviceType.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            Author author = _serviceType.GetById(id);

            if (author == null)
                return NotFound($"No Authors found by ID = {id}");

            return Ok(author);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AuthorDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(dto);

            Author author = new() { Name = dto.Name };
            await _serviceType.Create(author);
            return Ok(author);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(dto);

            Author author =_serviceType.GetById(id);

            if (author == null)
                return NotFound($"No Authors found by ID = {id}");

            author.Name = dto.Name;

            _serviceType.Update(author);

            return Ok(author);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            Author author = _serviceType.GetById(id);

            if (author == null)
                return NotFound($"No Authors found by ID = {id}");

            author.Name = author.Name;

            _serviceType.Delete(author);

            return Ok(author);

        }
    }
}
