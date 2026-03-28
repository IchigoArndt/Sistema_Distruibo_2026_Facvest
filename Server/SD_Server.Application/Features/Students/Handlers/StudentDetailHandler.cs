using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Detail;
using SD_Server.Application.Features.Students.DTO;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentDetailHandler
    {
        public class Handler(ILogger<Handler> logger, IStudentRepository repository) : IRequestHandler<StudentDetailCommand, Result<Exception, StudentDTO>>
        {
            public async Task<Result<Exception, StudentDTO>> Handle(StudentDetailCommand request, CancellationToken cancellationToken)
            {
                var validator = new StudentDetailCommandValidator();

                var resultValidate = validator.Validate(request);

                if (!resultValidate.IsValid)
                {
                    var messageError = resultValidate.Errors.FirstOrDefault().ErrorMessage;

                    logger.LogError("Ocorreu um erro na validação do comando. Erro: {error}", messageError);

                    return new Exception(messageError);
                }

                var student = await repository.GetByIdAsync(request.Id);

                if (student.IsFailure)
                {
                    logger.LogError("Ocorreu uma falha ao buscar o aluno com o id {id}. Erro: {erro}", request.Id, student.Failure.Message);

                    return new Exception(student.Failure.Message);
                }

                if (student.Success == null)
                {
                    logger.LogError("Não foi encontrado nenhum aluno com o id {id}", request.Id);

                    return new BusinessException(ErrorCodes.NotFound, $"Não foi nenhum aluno com o Id {request.Id}");
                }

                logger.LogInformation($"Aluno encontrado com o Id{request.Id}");

                var dto = new StudentDTO
                {
                    Id = request.Id,
                    Name = student.Success.Name,
                    Email = student.Success.Email,
                    Age = student.Success.Age,
                    CellPhone = student.Success.CellPhone,
                    Status = student.Success.Status,
                    LastReview = student.Success.LastReview
                };

                return dto;
            }
        }
    }
}
