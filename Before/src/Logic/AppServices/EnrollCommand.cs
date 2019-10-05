using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.AppServices
{
    public sealed class EnrollCommand : ICommand
    {
        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }

        public EnrollCommand(long id, string course, string grade)
        {
            Id = id;
            Course = course;
            Grade = grade;
        }

        internal sealed class EnrollCommandHandler : ICommandHandler<EnrollCommand>
        {
            private readonly SessionFactory _sessionFactory;

            public EnrollCommandHandler(SessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Result Handle(EnrollCommand command)
            {
                UnitOfWork unitOfWork = new UnitOfWork(_sessionFactory);
                StudentRepository studentRepo = new StudentRepository(unitOfWork);
                CourseRepository courseRepo = new CourseRepository(unitOfWork);

                Student student = studentRepo.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for id {command.Id}");

                Course course = courseRepo.GetByName(command.Course);
                if (course == null)
                    return Result.Fail($"Course is incorrect: '{command.Course}'");

                bool success = Enum.TryParse<Grade>(command.Grade, out Grade grade);
                if (!success)
                    return Result.Fail($"Grade is incorrect: '{command.Grade}'");

                student.Enroll(course, grade);

                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}
