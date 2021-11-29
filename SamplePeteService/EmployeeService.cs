using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamplePeteService.Models;

namespace SamplePeteService
{
    /// <summary>
    /// CRUD functions for TblEmployeeInfo table
    /// </summary>
    public static class EmployeeService
    {
        public static async Task CreateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            using var context = new Context();

            tblEmployeeInfo.EmployeeID = Guid.NewGuid().ToString();

            await context.AddAsync(tblEmployeeInfo).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public static async Task<List<TblEmployeeInfo>> GetEmployeesAsync()
        {
            using var context = new Context();

            return await context.TblEmployeeInfos
                    .ToListAsync().ConfigureAwait(false);
        }

        public static async Task UpdateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            using var context = new Context();

            context.Update(tblEmployeeInfo);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public static async Task DeleteEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            using var context = new Context();

            context.Remove(tblEmployeeInfo);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
