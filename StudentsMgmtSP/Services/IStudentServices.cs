using StudentsMgmtSP.Model;

namespace StudentsMgmtSP.Services
{
    public interface IStudentServices
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<Student> GetStudent(int id);
        Task<IEnumerable<Student>> AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);

    
    }
}
