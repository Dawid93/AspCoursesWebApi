using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController, Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository)
        {
            this.courseLibraryRepository = courseLibraryRepository;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authors = courseLibraryRepository.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var author = courseLibraryRepository.GetAuthor(authorId);
            if(author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
    }
}
