using System;

namespace SD_Server.Domain.Exceptions
{
    /// <summary>
    /// Representa uma exceção de negócio.
    /// É a classe base para a implementação de exceções de negócio.
    ///
    /// </summary>
    public class BusinessException(ErrorCodes errorCode, string message) : Exception(message)
    {
        public ErrorCodes ErrorCode { get; } = errorCode;
    }
}