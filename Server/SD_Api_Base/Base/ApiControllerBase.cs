using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Exceptions;
using SD_Server.Domain.Exceptions;
using SD_SharedKernel.Helpers;
using System.Net;

namespace SD_Api_Base.Base
{
    public class ApiControllerBase(IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Manuseia o result. Valida se é necessário retornar erro ou o próprio TSuccess
        /// </summary>
        /// <typeparam name="TFailure"></typeparam>
        /// <typeparam name="TSuccess"></typeparam>
        /// <param name="result">Objeto Result utilizado nas chamadas.</param>
        /// <returns></returns>
        protected IActionResult HandleCommand<TFailure, TSuccess>
            (Result<TFailure, TSuccess> result) where TFailure : Exception
        {
            return result.IsFailure ? HandleFailure(result.Failure) : Ok(result.Success);
        }

        /// <summary>
        /// Manuseia a query para retornar os dados.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected IActionResult HandleQueryable<TSource, TResult>(Result<Exception, IQueryable<TSource>> result)
        {
            if (result.IsFailure)
                return this.HandleFailure(result.Failure);

            var mappedResult = result.Success.ProjectTo<TResult>(mapper.ConfigurationProvider);

            return Ok(mappedResult);
        }

        /// <summary>
        /// Manuseia o result. Verifica se a resposta é uma falha ou sucesso, retornando os dados apropriados.
        /// É importante destacar que este método realiza o mapeamento da classe TSource em TResult
        /// </summary>
        /// <typeparam name="TSource">Classe de origem (ex.: domínio)</typeparam>
        /// <typeparam name="TResult">ViewModel</typeparam>
        /// <param name="result">Objeto Result retornado pelas chamadas.</param>
        /// <returns>Resposta apropriada baseado no result enviado como parâmetro</returns>
        protected IActionResult HandleQuery<TSource, TResult>(Result<Exception, TSource> result)
        {
            return result.IsSuccess ? Ok(mapper.Map<TSource, TResult>(result.Success)) : HandleFailure(result.Failure);
        }

        /// <summary>
        /// Verifica a exceção passada por parametro para passar o StatusCode correto para o frontend.
        /// </summary>
        /// <typeparam name="T">Qualquer classe que herde de Exeption</typeparam>
        /// <param name="exceptionToHandle">obj de exceção</param>
        /// <returns></returns>
        protected IActionResult HandleFailure<T>(T exceptionToHandle) where T : Exception
        {
            HttpStatusCode code;
            ExceptionPayload payload;

            if (exceptionToHandle is BusinessException)
            {
                code = HttpStatusCode.BadRequest;
                payload = ExceptionPayload.New(exceptionToHandle, (exceptionToHandle as BusinessException).ErrorCode.GetHashCode());
            }
            else if (exceptionToHandle is FluentValidation.ValidationException)
            {
                code = HttpStatusCode.BadRequest;
                payload = ExceptionPayload.New(exceptionToHandle, ErrorCodes.BadRequest.GetHashCode(), (exceptionToHandle as FluentValidation.ValidationException).Errors.ToList());
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
                payload = ExceptionPayload.New(exceptionToHandle, ErrorCodes.Unhandled.GetHashCode());
            }

            return this.StatusCode(code.GetHashCode(), payload);
        }
    }
}
