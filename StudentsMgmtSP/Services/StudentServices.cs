using StudentsMgmtSP.Model;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace StudentsMgmtSP.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly string _connection;

        public StudentServices(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Student>> AddStudent(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<Student> students = new List<Student>();
                        using (SqlCommand command = new SqlCommand("AddNewStudent", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@FirstName", student.FirstName);
                            command.Parameters.AddWithValue("@LastName", student.LastName);
                            command.Parameters.AddWithValue("@Age", student.Age);
                            await command.ExecuteNonQueryAsync();
                        }
                        using (SqlCommand command = new SqlCommand("GetAllStudents", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                               
                                while (await reader.ReadAsync())
                                {
                                    students.Add(new Student
                                    {
                                        StudentID = reader.GetInt32(0),
                                        FirstName = reader.GetString(1),
                                        LastName = reader.GetString(2),
                                        Age = reader.GetInt32(3)
                                    });
                                }
                                
                            }
                        }
                        transaction.Commit();
                        return students;

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            
        }

        public async Task DeleteStudent(int id)
        {
            using(SqlConnection connection = new SqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Student> GetStudent(int id)
        {
            using(SqlConnection connection = new SqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("GetStudentDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", id);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Student
                            {
                                StudentID = id,
                                FirstName = reader["FullName"].ToString().Split(' ')[0],
                                LastName = reader["FullName"].ToString().Split(' ').Skip(1).FirstOrDefault(),
                                Age = reader.GetInt32(1)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            using(SqlConnection connection = new SqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("GetAllStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Student> students = new List<Student>();
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                StudentID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Age = reader.GetInt32(3)
                            });
                        }
                        return students;
                    }
                }
            }
        }

        public async Task UpdateStudent(Student student)
        {
            using(SqlConnection connection = new SqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
