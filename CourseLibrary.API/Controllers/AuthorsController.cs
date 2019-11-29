using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
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
        private readonly IMapper mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = courseLibraryRepository.GetAuthors(authorsResourceParameters);
   
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorsFromRepo = courseLibraryRepository.GetAuthor(authorId);
            if(authorsFromRepo == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuthorDto>(authorsFromRepo));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreateDto author)
        {
            var authorEntity = mapper.Map<Entities.Author>(author);
            courseLibraryRepository.AddAuthor(authorEntity);
            courseLibraryRepository.Save();
            var authorToReturn = mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }
    }
}
