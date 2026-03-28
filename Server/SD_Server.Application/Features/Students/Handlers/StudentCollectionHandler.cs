using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.DTO;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentCollectionHandler
    {
        public class Query : IRequest<Result<Exception, IQueryable<StudentDTO>>> { }
        public class HandlerQuery(ILogger<HandlerQuery> logger, IStudentRepository repository) : IRequestHandler<Query, Result<Exception, IQueryable<StudentDTO>>>
        {
            public async Task<Result<Exception, IQueryable<StudentDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando o processo de busca de todos os alunos do sistema");

                var resultStudents = await repository.GetAllAsync();

                if (resultStudents.IsFailure)
                {
                    logger.LogError("Ocorreu um falha ao buscar todos os alunos. Erro: {erro}", resultStudents.Failure.Message);
                    return new Exception(resultStudents.Failure.Message);
                }

                if (!resultStudents.Success.Any())
                {
                    logger.LogInformation("Nenhum aluno encontrado no sistema");

                    var emptyCollection = new List<StudentDTO>();

                    return Result<Exception, IQueryable<StudentDTO>>.Of(emptyCollection.AsQueryable());
                }

                logger.LogInformation("Finalizando o processo de busca de todas os alunos do sistema. Quantidade encontrada: {qtd}", resultStudents.Success.Count());

                var collectionStudentDTO = resultStudents.Success.Select(x => new StudentDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Age = x.Age,
                    CellPhone = x.CellPhone,
                    Status = x.Status,
                    LastReview = x.LastReview
                });

                return Result<Exception, IQueryable<StudentDTO>>.Of(collectionStudentDTO);

                throw new NotImplementedException();
            }
        }
    }
}
