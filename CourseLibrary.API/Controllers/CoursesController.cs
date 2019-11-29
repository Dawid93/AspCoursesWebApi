using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId)){
                return NotFound();
            }
            var coursesFromRepository = courseLibraryRepository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(coursesFromRepository));
        }

        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();

            var courseFromRepo = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseFromRepo == null)
                return NotFound();

            return Ok(mapper.Map<CourseDto>(courseFromRepo));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();

            var courseEntity = mapper.Map<Entities.Course>(course);
            courseLibraryRepository.AddCourse(authorId, courseEntity);
            courseLibraryRepository.Save();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }
    }
}