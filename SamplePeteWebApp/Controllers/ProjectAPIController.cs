﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamplePeteService;
using SamplePeteService.Models;

namespace SamplePeteWeb.Controllers
{
    [Route("api/CurrentProjects")]
    [ApiController]
    public class ProjectAPIController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectAPIController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Route("Project")]
        [HttpPost]
        public async Task CreateProjectAsync(TblProject tblProject)
        {
            // shave off the time portion and UTC info, the JQuery date picker brings that in
            tblProject.StartDate = tblProject.StartDate.Date;
            tblProject.StartDate = DateTime.SpecifyKind(tblProject.StartDate, DateTimeKind.Unspecified);
            tblProject.EndDate = tblProject.EndDate.Date;
            tblProject.EndDate = DateTime.SpecifyKind(tblProject.EndDate, DateTimeKind.Unspecified);

            await _projectService.CreateProjectAsync(tblProject).ConfigureAwait(false);
        }

        [Route("Projects")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<List<TblProject>> ProjectsAsync()
        {
            return await _projectService.GetProjectsAsync().ConfigureAwait(false);
        }

        [Route("Project")]
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task UpdateProjectAsync(TblProject tblProject)
        {
            // shave off the time portion and UTC info, the JQuery date picker brings that in
            tblProject.StartDate = tblProject.StartDate.Date;
            tblProject.StartDate = DateTime.SpecifyKind(tblProject.StartDate, DateTimeKind.Unspecified);
            tblProject.EndDate = tblProject.EndDate.Date;
            tblProject.EndDate = DateTime.SpecifyKind(tblProject.EndDate, DateTimeKind.Unspecified);

            await _projectService.UpdateProjectAsync(tblProject).ConfigureAwait(false);
        }

        [Route("Project")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task DeleteProjectAsync(TblProject tblProject)
        {
            await _projectService.DeleteProjectAsync(tblProject).ConfigureAwait(false);
        }
    }
}