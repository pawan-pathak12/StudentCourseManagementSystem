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

        #region CURD Operation
        public async Task Add(Student student)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"INSERT INTO Students(Name, Email, DOB, Number, IsActive, Gender, Address)
                    VALUES (@Name, @Email, @DOB, @Number, @IsActive, @Gender, @Address)";

            await connection.ExecuteAsync(sql, student);
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "Update Students set IsActive=1 where StudentId=@Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<List<Student>> GetAll()
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Students";
            var result = await connection.QueryAsync<Student>(sql);
            return result.ToList();
        }

        public async Task<Student?> GetById(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Students WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
        }

        public async Task<bool> Update(Student student)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"UPDATE Students 
                    SET Name = @Name,
                        Email = @Email,
                        DOB = @DOB,
                        Number = @Number,
                        EmrollementDate = @EmrollementDate,
                        IsActive = @IsActive,
                        Gender = @Gender,
                        Address = @Address
                    WHERE StudentId = @StudentId";
            var rows = await connection.ExecuteAsync(sql, student);
            return rows > 0;
        }



        #endregion

        #region Validation of Student

        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
