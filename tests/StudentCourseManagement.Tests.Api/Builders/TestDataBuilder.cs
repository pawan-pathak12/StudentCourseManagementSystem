using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Repositories.Identities;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Entities.Identites;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Api.Builders
{
    public class TestDataBuilder
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly IFeeTemplateRepository _feeTemplateRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceLineItemRepository _invoiceLineItemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public TestDataBuilder(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _studentRepository = serviceProvider.GetRequiredService<IStudentRepository>();
            _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
            _enrollmentRepository = serviceProvider.GetRequiredService<IEnrollmentRepository>();
            _feeAssessmentRepository = serviceProvider.GetRequiredService<IFeeAssessmentRepository>();
            _feeTemplateRepository = serviceProvider.GetRequiredService<IFeeTemplateRepository>();
            _invoiceRepository = serviceProvider.GetRequiredService<IInvoiceRepository>();
            _invoiceLineItemRepository = serviceProvider.GetRequiredService<IInvoiceLineItemRepository>();
            _paymentRepository = serviceProvider.GetRequiredService<IPaymentRepository>();
            _paymentMethodRepository = serviceProvider.GetRequiredService<IPaymentMethodRepository>();
        }


        public async Task<User> CreateUser()
        {
            var user = new User();

            var rand = new Random();
            user.PasswordHash = "AQAAAAIAAYagAAAAEIu6fYWy1QHI+acYlUsU83kFNlNoFfKymXcjBio7LI8+aNomKFEBlvdgGiqFs9onlA==";
            user.Email = $"user+{rand.Next(10000, 99999)}@gmail.com";
            user.Role = "Admin";

            var userId = await _userRepository.AddAsync(user);
            var userData = await _userRepository.GetByIdAsync(userId);
            return new User
            {
                Email = userData.Email,
                Role = userData.Role,
                UserId = userId
            };
        }

        public async Task<Student> CreateStudent()
        {
            var rand = new Random();
            var student = new Student
            {
                Name = "Test Student",
                IsActive = true,
                Email = $"user{rand.Next(0000, 9999)}" + "@gmail.com"
            };

            var id = await _studentRepository.AddAsync(student);
            return await _studentRepository.GetByIdAsync(id);
        }


        public async Task<Course> CreateCourse()
        {
            var course = new Course
            {
                Title = "Test Course",
                Credits = 3,
                IsActive = true,
            };

            var id = await _courseRepository.AddAsync(course);
            return await _courseRepository.GetByIdAsync(id);
        }


        public async Task<Enrollment?> CreateEnrollment(int studentId, int courseId)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true,
                EnrollmentStatus = EnrollmentStatus.Comfirmed
            };

            var id = await _enrollmentRepository.AddAsync(enrollment);
            return await _enrollmentRepository.GetByIdAsync(id);
        }


        public async Task<FeeTemplate?> CreateFeeTemplate(int courseId)
        {
            var template = new FeeTemplate
            {
                Name = "Test Template",
                Amount = 1000,
                IsActive = true,
                CourseId = courseId
            };

            var id = await _feeTemplateRepository.AddAsync(template);
            return await _feeTemplateRepository.GetByIdAsync(id);
        }


        public async Task<FeeAssessment?> CreateFeeAssessment(int enrollmentId, int templateId)
        {
            var fee = new FeeAssessment
            {
                EnrollmentId = enrollmentId,
                FeeTemplateId = templateId,
                Amount = 1000,
                DueDate = DateTime.UtcNow.AddDays(30),
                IsActive = true
            };

            var id = await _feeAssessmentRepository.AddAsync(fee);
            return await _feeAssessmentRepository.GetByIdAsync(id);
        }


        public async Task<Invoice?> CreateInvoice(int studentId)
        {
            var invoice = new Invoice
            {
                StudentId = studentId,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 1000,
                IsActive = true
            };

            var id = await _invoiceRepository.AddAsync(invoice);
            return await _invoiceRepository.GetByIdAsync(id);
        }

        public async Task<InvoiceLineItem?> CreateInvoiceLineItem(int invoiceId)
        {
            var item = new InvoiceLineItem
            {
                InvoiceId = invoiceId,
                Description = "Test Item",
                Amount = 1000,
                IsActive = true
            };

            var id = await _invoiceLineItemRepository.AddAsync(item);
            return await _invoiceLineItemRepository.GetByIdAsync(id);
        }


        public async Task<PaymentMethod?> CreatePaymentMethod()
        {
            var method = new PaymentMethod
            {
                Name = "Cash",
                IsActive = true
            };

            var id = await _paymentMethodRepository.AddAsync(method);
            return await _paymentMethodRepository.GetByIdAsync(id);
        }


        public async Task<Payment?> CreatePayment(int invoiceId, int methodId)
        {
            var payment = new Payment
            {
                InvoiceId = invoiceId,
                PaymentMethodId = methodId,
                Amount = 1000,
                PaymentDate = DateTime.UtcNow,
                IsActive = true
            };

            var id = await _paymentRepository.AddAsync(payment);
            return await _paymentRepository.GetByIdAsync(id);
        }
    }
}
