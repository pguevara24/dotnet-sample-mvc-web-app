using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamplePeteService.Models;

namespace SamplePeteService
{
    /// <summary>
    /// CRUD functions for TblProject table
    /// </summary>
    public static class ProjectService
    {
        public static async Task CreateProjectAsync(TblProject tblProject)
        {
            using var context = new Context();

            tblProject.ProjectID = Guid.NewGuid().ToString();

            await context.AddAsync(tblProject).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public static async Task<List<TblProject>> GetProjectsAsync()
        {
            using var context = new Context();

            return await context.TblProjects
                    .AsNoTracking()
                    .OrderBy(tblProject => tblProject.ProjectName)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public static async Task UpdateProjectAsync(TblProject tblProject)
        {
            using var context = new Context();

            context.Update(tblProject);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public static async Task DeleteProjectAsync(TblProject tblProject)
        {
            using var context = new Context();

            context.Remove(tblProject);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
