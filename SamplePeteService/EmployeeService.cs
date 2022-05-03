using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamplePeteService.Models;

namespace SamplePeteService
{
    public interface IEmployeeService
    {
        Task CreateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo);
        Task DeleteEmployeeAsync(TblEmployeeInfo tblEmployeeInfo);
        Task<List<TblEmployeeInfo>> GetEmployeesAsync();
        Task UpdateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo);
    }

    /// <summary>
    /// CRUD functions for TblEmployeeInfo table
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly Context _context;

        public EmployeeService(Context context)
        {
            _context = context;
        }

        public async Task CreateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            tblEmployeeInfo.EmployeeID = Guid.NewGuid().ToString();

            await _context.AddAsync(tblEmployeeInfo).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<TblEmployeeInfo>> GetEmployeesAsync()
        {
            return await _context.TblEmployeeInfos
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task UpdateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            TblEmployeeInfo entity = await _context.TblEmployeeInfos.FindAsync(tblEmployeeInfo.EmployeeID).ConfigureAwait(false);

            _context.Entry(entity).CurrentValues.SetValues(tblEmployeeInfo);

            _context.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            TblEmployeeInfo entity = await _context.TblEmployeeInfos.FindAsync(tblEmployeeInfo.EmployeeID).ConfigureAwait(false);

            _context.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
