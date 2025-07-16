using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Response.TokenResponse;
using Domain.Utilities.Results.Interfaces;

namespace Application.Interfaces
{
    public interface IAuthRequestCodeService
    {
        Task<IDataResult<TokenResponse>> RequestCodeAsync(string tckn);
    }
}