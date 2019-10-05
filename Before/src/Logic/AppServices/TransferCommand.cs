using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.AppServices
{
    public sealed class TransferCommand : ICommand
    {
        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }
        public int EnrollmentNumber { get; }

        public TransferCommand(long id, string course, string grade, int enrollmentNumber)
        {
            Id = id;
            Course = course;
            Grade = grade;
            EnrollmentNumber = enrollmentNumber;
        }

        [AuditLog]
        internal sealed class TransferCommandHandler : ICommandHandler<TransferCommand>
        {
            private readonly SessionFactory _sessionFactory;

            public TransferCommandHandler(SessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Result Handle(TransferCommand command)
            {
                UnitOfWork unitOfWork = new UnitOfWork(_sessionFactory);
                StudentRepository studentRepository = new StudentRepository(unitOfWork);
                CourseRepository courseRepository = new CourseRepository(unitOfWork);

                Student student = studentRepository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                Course course = courseRepository.GetByName(command.Course);
                if (course == null)
                    return Result.Fail($"Course is incorrect: '{command.Course}'");

                bool success = Enum.TryParse(command.Grade, out Grade grade);
                if (!success)
                    return Result.Fail($"Grade is incorrect: '{command.Grade}'");

                Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollment == null)
                    return Result.Fail($"No enrollment found with number: '{command.EnrollmentNumber}'");

                enrollment.Update(course, grade);

                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}
