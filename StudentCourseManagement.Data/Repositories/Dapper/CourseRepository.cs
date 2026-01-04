using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentSysDbContext _dbContext;
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(StudentSysDbContext dbContext, ILogger<CourseRepository> logger)
        {
            _dbContext = dbContext;
            this._logger = logger;
        }

        #region CURD Operations 

        public async Task<int> AddAsync(Course course)
        {
            string sql = @"
                INSERT INTO Courses ( Code, Title, Credits, Description, Instructor,StartDate, EndDate, Capacity,
                    EnrollmentStartDate, EnrollmentEndDate) VALUES (
                    @Code, @Title, @Credits, @Description, @Instructor,
                    @StartDate, @EndDate, @Capacity,
                    @EnrollmentStartDate, @EnrollmentEndDate
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            _logger.LogInformation($"Repo : adding new record of course");
            var newId = await connection.QuerySingleAsync<int>(sql, course);

            if (newId > 0)
            {
                course.CourseId = newId;
                _logger.LogInformation("Successfully created course with ID {CourseId}", course.CourseId);
                return newId;
            }

            _logger.LogWarning("Course creation returned invalid ID (<= 0) for {Code}", course.Code);
            return 0;

        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            string sql = @" SELECT * FROM Courses WHERE IsActive = 1";

            using var connection = _dbContext.CreateConnection();
            _logger.LogInformation("Repo : Fetching all record of course");
            var courses = await connection.QueryAsync<Course>(sql);
            _logger.LogInformation("Repo : Fetch all record of course");

            return courses;

        }

        public async Task<Course> GetByIdAsync(int id)
        {
            string sql = @"
                SELECT * FROM Courses  WHERE CourseId = @Id AND IsActive = 1;";

            using var connection = _dbContext.CreateConnection();

            var course = await connection.QueryFirstOrDefaultAsync<Course>(sql, new { Id = id });

            _logger.LogInformation($"Repo : Fetched record of course of course ID {id}");

            return course;
        }

        public async Task<bool> UpdateAsync(int id, Course course)
        {
            string sql = @"
                UPDATE Courses
                SET 
                    Code = @Code,
                    Title = @Title,
                    Credits = @Credits,
                    Description = @Description,
                    Instructor = @Instructor,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Capacity = @Capacity,
                    EnrollmentStartDate = @EnrollmentStartDate,
                    EnrollmentEndDate = @EnrollmentEndDate
                WHERE CourseId = @CourseId AND IsActive = 1;";

            course.CourseId = id;

            using var connection = _dbContext.CreateConnection();
            _logger.LogInformation($"Repo : Updating record of course with Id : {id}");
            var rowsAffected = await connection.ExecuteAsync(sql, course);
            if (rowsAffected > 0)
            {
                return true;
            }
            _logger.LogInformation($"Repo :Failed to  Update record of course with Id : {id}");
            return false;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Courses SET IsActive = 0  WHERE CourseId = @Id AND IsActive = 1;";

            using var connection = _dbContext.CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            if (rowsAffected > 0)
            {
                _logger.LogInformation($"Repo :softly Deleted course record with Id {id}");
                return true;
            }

            _logger.LogInformation($"Repo :Failed to Delete course record with Id {id}");
            return false;

        }

        #endregion

        #region Validation of Course 
        public async Task<bool> CodeExistsAsync(string code)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select case " +
                 "when exists (select 1 from Courses where Code=@Code) then 1" +
                 " else 0 end";
            var codeExists = await connection.ExecuteScalarAsync<int>(sql, new { Code = code });
            return codeExists == 1;
        }

        public async Task<bool> TitleExistsAsync(string title)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select case " +
                 "when exists (select 1 from Courses where Title=@Title) then 1" +
                 " else 0 end";
            var titleExists = await connection.ExecuteScalarAsync<int>(sql, new { Title = title });
            return titleExists == 1;
        }
        #endregion

        #region Enrollment Service Required Methods 
        public async Task<bool> CheckEnrollmentDateAsync(int courseId, DateTimeOffset enrollmentDate)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select  CASE WHEN EXISTS (
                              SELECT 1
                              FROM Courses
                              WHERE CourseId = @CourseId AND IsActive = 1
                              AND @EnrollmentDate BETWEEN EnrollmentStartDate AND EnrollmentEndDate )
                              THEN 1 END";
            var isValid = await connection.ExecuteScalarAsync<bool>(sql, new { CourseId = courseId, EnrollmentDate = enrollmentDate });
            return isValid;

        }

        #endregion

    }
}