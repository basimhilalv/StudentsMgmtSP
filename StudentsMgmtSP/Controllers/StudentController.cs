using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsMgmtSP.Model;
using StudentsMgmtSP.Services;

namespace StudentsMgmtSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;
        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }
        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _studentServices.GetStudents();
        }
        [HttpGet("{id}")]
        public async Task<Student> GetStudent(int id)
        {
            return await _studentServices.GetStudent(id);
        }
        [HttpPost]
        public async Task<IEnumerable<Student>> AddStudent(Student student)
        {
            return await _studentServices.AddStudent(student);
        }
        [HttpPut]
        public async Task UpdateStudent(Student student)
        {
            await _studentServices.UpdateStudent(student);
        }
        [HttpDelete("{id}")]
        public async Task DeleteStudent(int id)
        {
            await _studentServices.DeleteStudent(id);
        }
    }
}
