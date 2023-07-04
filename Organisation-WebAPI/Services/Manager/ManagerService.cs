using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.ManagerDto;


namespace Organisation_WebAPI.Services.Managers
{
    public class ManagerService : IManagerService
    {   

        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context ; // Represents the database context

        public ManagerService(OrganizationContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetManagerDto>>> AddManager(AddManagerDto newManager)
        {
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            var manager = _mapper.Map<Manager>(newManager);

             _context.Managers.Add(manager);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Managers.Select(c => _mapper.Map<GetManagerDto>(c)).ToListAsync();
            return serviceResponse;   
        }    
        public async Task<ServiceResponse<List<GetManagerDto>>> DeleteManager(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            try {

            var manager = await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);
            if (manager is null)
                throw new Exception($"Manager with id '{id}' not found");
            
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Managers.Select(c => _mapper.Map<GetManagerDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetManagerDto>>> GetAllManagers()
        {
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            var dbManagerTasks = await _context.Managers.ToListAsync();
            serviceResponse.Data = dbManagerTasks.Select(c => _mapper.Map<GetManagerDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetManagerDto>> GetManagerById(int id)
        {
            var serviceResponse = new ServiceResponse<GetManagerDto>();
            try
            {
            var dbManager =  await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);
            if (dbManager is null)
                    throw new Exception($"Manager with id '{id}' not found");

            serviceResponse.Data = _mapper.Map<GetManagerDto>(dbManager);
            return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public Task<ServiceResponse<int>> GetManagerCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count =  _context.Managers.Count();
            serviceResponse.Data = count;
            return Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetManagerDto>> UpdateManager(UpdateManagerDto updatedManager, int id)
        {
            var serviceResponse = new ServiceResponse<GetManagerDto>();
            try {
                var manager = await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);

                if (manager is null)
                    throw new Exception($"Manager with id '{id}' not found");
                
                manager.ManagerName = updatedManager.ManagerName;
                manager.ManagerSalary = updatedManager.ManagerSalary;
                manager.ManagerAge = updatedManager.ManagerAge;
                manager.ProductID = updatedManager.ProductID;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetManagerDto>(manager);

                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }
    }
}