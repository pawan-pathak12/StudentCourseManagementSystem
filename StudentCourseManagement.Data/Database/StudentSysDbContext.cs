using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace StudentCourseManagement.Data.Database
{
    public class StudentSysDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StudentSysDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;

        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(this._connectionString);
        }
    }
}
