namespace StudentCourseManagement.Tests.Integration
{
    public abstract class BaseIntegrationTest
    {
        protected readonly DatabaseFixture _fixture;

        protected BaseIntegrationTest(DatabaseFixture fixture)
        {
            _fixture = fixture;

        }
    }
}
