﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IModuleRepository
    {
        Task<List<ModuleEntity>> GetAllAsync();
    }
}
