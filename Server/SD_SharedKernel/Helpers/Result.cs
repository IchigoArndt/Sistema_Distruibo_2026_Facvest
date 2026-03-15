using System;
using System.Collections.Generic;
using System.Text;

namespace SD_SharedKernel.Helpers
{
    public struct Result<TFailure, TSuccess>
    {
        public TFailure Failure { get; internal set; }
        public TSuccess Success { get; internal set; }

        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        internal Result(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default;
        }

        internal Result(TSuccess success)
        {
            IsFailure = false;
            Failure = default;
            Success = success;
        }

        public TResult Match<TResult>(
                Func<TFailure, TResult> failure,
                Func<TSuccess, TResult> success
            )
            => IsFailure ? failure(Failure) : success(Success);

        public static implicit operator Result<TFailure, TSuccess>(TFailure failure)
            => new Result<TFailure, TSuccess>(failure);

        public static implicit operator Result<TFailure, TSuccess>(TSuccess success)
            => new Result<TFailure, TSuccess>(success);

        public static Result<TFailure, TSuccess> Of(TSuccess obj) => obj;

        public static Result<TFailure, TSuccess> Of(TFailure obj) => obj;
    }
}
