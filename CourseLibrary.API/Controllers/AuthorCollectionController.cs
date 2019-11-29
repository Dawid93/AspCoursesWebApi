using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorCollection")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public AuthorCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute, ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            var authorEntities = courseLibraryRepository.GetAuthors(ids);
            if(ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreateDto> authors)
        {
            var entities = mapper.Map<IEnumerable<Author>>(authors);
            foreach (var elem in entities)
            {
                courseLibraryRepository.AddAuthor(elem);
            }
            courseLibraryRepository.Save();

            var authorCollectionToReturn = mapper.Map<IEnumerable<AuthorDto>>(entities);
            var ids = string.Join(",", authorCollectionToReturn.Select(x => x.Id));
            return CreatedAtRoute("GetAuthorsCollection", new { ids = ids }, authorCollectionToReturn);
        }
    }
}
