using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Shared.Services;

public sealed record SortMapping(string SortField, string PropertyName, bool Reverse = false);