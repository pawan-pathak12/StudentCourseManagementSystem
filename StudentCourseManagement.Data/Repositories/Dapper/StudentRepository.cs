using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentSysDbContext _dbContext;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(StudentSysDbContext dbContext, ILogger<StudentRepository> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        #region CURD Operation
        public async Task<int> AddAsync(Student student)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"INSERT INTO Students(Name, Email, DOB, Number, IsActive, Gender, Address)
                    VALUES (@Name, @Email, @DOB, @Number, @IsActive, @Gender, @Address);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            return await connection.ExecuteAsync(sql, student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "Update Students set IsActive=0 where StudentId=@StudentId";
            var rows = await connection.ExecuteAsync(sql, new { StudentId = id });
            if (rows > 0)
            {
                _logger.LogInformation("Student with ID {StudentId} marked inactive.", id);
                return true;
            }

            _logger.LogWarning("Delete failed. No student found with ID {StudentId}.", id);
            return false;

        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Students";
            _logger.LogInformation("Fetching all record of Student table");
            var result = await connection.QueryAsync<Student>(sql);
            return result.ToList();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Students WHERE StudentId = @Id";

            _logger.LogInformation("Fetching Student record with Id " + id);
            return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"UPDATE Students 
                    SET Name = @Name,
                        Email = @Email,
                        DOB = @DOB,
                        Number = @Number,
                        EnrollmentDate = @EnrollmentDate,
                        IsActive = @IsActive,
                        Gender = @Gender,
                        Address = @Address
                    WHERE StudentId = @StudentId";
            var rows = await connection.ExecuteAsync(sql, student);

            if (rows > 0)
            {
                _logger.LogInformation($"Updated Student with Id{student.StudentId}");
                return true;
            }

            _logger.LogWarning($"Failed to Update student record with id {student.StudentId}");
            return false;

        }



        #endregion

        #region Validation of Student
        public async Task<bool> IsStudentActiveAsync(int studentId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select case " +
                 "when exists (select 1 from Students where StudentId=@StudentId and IsActive=1) then 1" +
                 "else 0 end";
            _logger.LogInformation("Repo : Checking student is active or not");
            var studentExists = await connection.ExecuteScalarAsync<int>(sql, new { StudentId = studentId });
            return studentExists == 1;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select case " +
                 "when exists (select 1 from Students where Email=@Email) then 1" +
                 "else 0 end";
            _logger.LogInformation("Repo : Checking student email exists or not");
            var emailExists = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
            return emailExists == 1;
        }

        #endregion
    }
}
