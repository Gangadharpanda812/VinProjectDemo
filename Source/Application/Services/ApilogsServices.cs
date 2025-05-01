using Application.Interface;
using Domain;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    internal class ApilogsServices(IGenericRepository<ApiLogs> ApilogsRepository) : IApilogsServices
    {

        public async Task<ApiLogDto> AddApilog(InsertApiLogDto insertApiLogDto)
        {
            ArgumentNullException.ThrowIfNull(insertApiLogDto);

            ApiLogs InsertapiLog = new()
            {
                Duration = insertApiLogDto.Duration,
                EndTime = insertApiLogDto.EndTime,
                LogType = insertApiLogDto.LogType,
                RequestMethod = insertApiLogDto.RequestMethod,
                RequestPath = insertApiLogDto.RequestPath,
                ResponseStatusCode = insertApiLogDto.ResponseStatusCode,
                StartTime = insertApiLogDto.StartTime,
            };
            var output = await ApilogsRepository.AddAsync(InsertapiLog);
            return new ApiLogDto
            {
                TransactionId = output.TransactionId,
                Duration = output.Duration,
                EndTime = output.EndTime,
                LogType = output.LogType,
                RequestMethod = output.RequestMethod,
                RequestPath = output.RequestPath,
                ResponseStatusCode = output.ResponseStatusCode,
                StartTime = output.StartTime
            };
        }
    }
}
