using Dapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentSysDbContext _dbContext;

        public StudentRepository(StudentSysDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task Add(Student student)
        {
            var connection = _dbContext.CreateConnection();
            var sql = "INSERT INTO Students (Name, Email) VALUES (@Name, @Email)";
            await connection.ExecuteAsync(sql, student);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAll()
        {
            throw new NotImplementedException();
        }

        public Student? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
