using AutoMapper;
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
        protected IMapper mapper;
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

        public async Task<Student> CreateAndReturnStudent()
        {
            var rand = new Random();
            var student = new Student
            {
                Name = $"Test Student {rand.Next(100, 999)}",
                IsActive = true,
                Email = $"user{rand.Next(0000, 9999)}" + "@gmail.com",
                Gender = "Male",
            };

            var id = await _studentRepository.AddAsync(student);
            return await _studentRepository.GetByIdAsync(id);
        }


        public async Task<Course> CreateAndReturnCourse()
        {
            var rand = new Random();
            var course = new Course
            {
                Code = $"CS{rand.Next(1000, 9999)}",
                Title = "Introduction to Computer Science",
                Credits = 3,
                Description = "Foundational course covering programming basics, algorithms, and problem-solving.",
                Instructor = "Dr. Jane Doe",


                StartDate = DateTimeOffset.UtcNow.AddDays(7),
                EndDate = DateTimeOffset.UtcNow.AddDays(120),

                IsActive = true,
                Capacity = 50,

                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(-1),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(30)
            };



            var id = await _courseRepository.AddAsync(course);
            return await _courseRepository.GetByIdAsync(id);
        }


        public async Task<Enrollment?> CreateAndReturnEnrollment(int studentId, int courseId)
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

        public async Task UpdateEnrollment(int id, Enrollment enrollment)
        {
            await _enrollmentRepository.UpdateAsync(id, enrollment);
        }

        public async Task<FeeTemplate?> CreateAndReturnFeeTemplate(int courseId)
        {
            var template = new FeeTemplate
            {
                Name = "Test Template",
                Amount = 1000,
                IsActive = true,
                CourseId = courseId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var id = await _feeTemplateRepository.AddAsync(template);
            return await _feeTemplateRepository.GetByIdAsync(id);
        }



        public async Task<FeeAssessment?> CreateAndReturnFeeAssessment(int enrollmentId, int templateId)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);

            var fee = new FeeAssessment
            {
                EnrollmentId = enrollmentId,
                FeeTemplateId = templateId,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                CourseId = enrollment!.CourseId,
                Amount = 1000,
                DueDate = DateTime.UtcNow.AddDays(30),
                IsActive = true,
                AssessmentDate = DateTimeOffset.UtcNow
            };

            var id = await _feeAssessmentRepository.AddAsync(fee);
            return await _feeAssessmentRepository.GetByIdAsync(id);
        }


        public async Task<Invoice?> CreateAndReturnInvoice(int studentId, int feeAssessmentId, int courseId)
        {
            var invoice = new Invoice
            {
                StudentId = studentId,
                AmountPaid = 0,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 1000,
                IsActive = true,
                FeeAssessmentId = feeAssessmentId,
                CourseId = courseId
            };

            var id = await _invoiceRepository.AddAsync(invoice);
            return await _invoiceRepository.GetByIdAsync(id);
        }

        public async Task<InvoiceLineItem?> CreateInvoiceLineItem(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                return null;
            }
            var feeTemplate = await _feeTemplateRepository.GetActiveByCourseId(invoice!.CourseId);
            if (feeTemplate == null)
            {
                return null;
            }
            var item = new InvoiceLineItem
            {
                InvoiceId = invoiceId,
                Description = "Test Item",
                Amount = 1000,
                IsActive = true,
                CourseId = invoice!.CourseId,
                CreatedAt = DateTimeOffset.UtcNow,
                FeeTemplateId = feeTemplate!.FeeTemplateId
            };

            var id = await _invoiceLineItemRepository.AddAsync(item);
            return await _invoiceLineItemRepository.GetByIdAsync(id);
        }


        public async Task<PaymentMethod?> CreateAndReturnPaymentMethod()
        {
            var method = new PaymentMethod
            {
                Name = "Cash",
                IsActive = true,
                PaymentMethodType = PaymentMethodType.Cash
            };

            var id = await _paymentMethodRepository.AddAsync(method);
            return await _paymentMethodRepository.GetByIdAsync(id);
        }


        public async Task<Payment?> CreateAndReturnPayment(int invoiceId, int methodId, decimal amount)
        {
            var studentId = await _studentRepository.AddAsync(new Student { Name = "Ram Nath", IsActive = true, Address = "Ktm", Gender = "male" });

            var payment = new Payment
            {
                StudentId = studentId,
                PaymentStatus = PaymentStatus.Completed,
                InvoiceId = invoiceId,
                PaymentMethodId = methodId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow,
                IsActive = true
            };

            var id = await _paymentRepository.AddAsync(payment);
            return await _paymentRepository.GetByIdAsync(id);
        }
    }
}
