using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.AppServices
{
    public sealed class UnregisterCommand : ICommand
    {
        public long Id { get; }

        public UnregisterCommand(long id)
        {
            Id = id;
        }

        internal sealed class UnegisterCommandHandler : ICommandHandler<UnregisterCommand>
        {
            private readonly SessionFactory _sessionFactory;

            public UnegisterCommandHandler(SessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Result Handle(UnregisterCommand command)
            {
                UnitOfWork unitOfWork = new UnitOfWork(_sessionFactory);
                StudentRepository repository = new StudentRepository(unitOfWork);
                Student student = repository.GetById(command.Id);

                if (student == null)
                    return Result.Fail($"No student found for id {command.Id}");

                repository.Delete(student);
                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}
