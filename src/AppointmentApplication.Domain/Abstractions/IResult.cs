using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Abstractions;

public interface IResult
{
    List<Error>? Errors { get; }

    bool IsSuccess { get; }
}

public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}