using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamplePeteService.Models;

namespace SamplePeteService
{
    public interface IProjectService
    {
        Task CreateProjectAsync(TblProject tblProject);
        Task DeleteProjectAsync(TblProject tblProject);
        Task<List<TblProject>> GetProjectsAsync();
        Task UpdateProjectAsync(TblProject tblProject);
    }

    /// <summary>
    /// CRUD functions for TblProject table
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly Context _context;

        public ProjectService(Context context)
        {
            _context = context;
        }

        public async Task CreateProjectAsync(TblProject tblProject)
        {
            tblProject.ProjectID = Guid.NewGuid().ToString();

            await _context.AddAsync(tblProject).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<TblProject>> GetProjectsAsync()
        {
            return await _context.TblProjects
                    .AsNoTracking()
                    .OrderBy(tblProject => tblProject.ProjectName)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task UpdateProjectAsync(TblProject tblProject)
        {
            TblProject entity = await _context.TblProjects.FindAsync(tblProject.ProjectID).ConfigureAwait(false);

            _context.Entry(entity).CurrentValues.SetValues(tblProject);

            _context.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteProjectAsync(TblProject tblProject)
        {
            TblProject entity = await _context.TblProjects.FindAsync(tblProject.ProjectID).ConfigureAwait(false);

            _context.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
