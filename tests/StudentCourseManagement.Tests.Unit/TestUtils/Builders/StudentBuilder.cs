using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders
{
    internal class StudentBuilder
    {
        // Private fields with default values
        private int _studentId = 1;
        private string _name = "Test Student";
        private string _email = "test@student.com";
        private DateTimeOffset _dob = DateTimeOffset.UtcNow.AddYears(-20); // default: 20 years old
        private long _number = 9800000000; // example default phone number
        private DateTimeOffset _enrollmentDate = DateTimeOffset.UtcNow;
        private bool _isActive = true;
        private string _gender = "Male";
        private string _address = "Default Address";

        //Fluent "With...." methods to customize values 
        public StudentBuilder WithId(int studentId)
        {
            _studentId = studentId;
            return this;
        }
        public StudentBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public StudentBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public StudentBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public StudentBuilder AsInactive()
        {
            _isActive = false;
            return this;
        }

        public StudentBuilder AsActive()
        {
            _isActive = true;
            return this;
        }

        //Build Method that Creates the actual Student 
        public Student Build()
        {
            var student = new Student
            {
                StudentId = _studentId,
                Name = _name,
                Email = _email,
                Address = _address,
                Gender = _gender,
                Number = _number,
                DOB = _dob,
                IsActive = true
            };
            return student;
        }
    }
}
