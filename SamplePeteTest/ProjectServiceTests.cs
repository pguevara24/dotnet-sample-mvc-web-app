using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamplePeteService;
using SamplePeteService.Models;

namespace SamplePeteTest
{
    [TestClass]
    public class ProjectServiceTests
    {
        private static TblProject _tblProject;
        private static Context _dbContext;
        private static ProjectService _projectService;

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            _tblProject = new()
            {
                ProjectName = "New Project",
                StartDate = new DateTime(2021, 11, 27),
                EndDate = new DateTime(2021, 11, 29)
            };
        }

        [TestInitialize]
        public void Setup()
        {
            _dbContext = new();
            _projectService = new(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted(); // Remove from memory
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task CreateProject_RecordAdded()
        {
            // arrange

            // act
            await _projectService.CreateProjectAsync(_tblProject).ConfigureAwait(false);

            // assert
            List<TblProject> lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsNotNull(lstTblProject[0]);
        }

        [TestMethod]
        public async Task GetProjects_ReturnsList()
        {
            // arrange

            // act
            List<TblProject> lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);

            // assert
            Assert.IsNotNull(lstTblProject);
        }

        [TestMethod]
        public async Task UpdateProject_RecordUpdated()
        {
            // arrange
            await _projectService.CreateProjectAsync(_tblProject).ConfigureAwait(false);
            List<TblProject> lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);
            TblProject tblProject = lstTblProject[0];
            tblProject.ProjectName = "A Project";

            // act
            await _projectService.UpdateProjectAsync(tblProject).ConfigureAwait(false);

            // assert
            lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsTrue(string.Compare(lstTblProject[0].ProjectName, "A Project") == 0);
        }

        [TestMethod]
        public async Task DeleteProject_RecordRemoved()
        {
            // arrange
            await _projectService.CreateProjectAsync(_tblProject).ConfigureAwait(false);
            List<TblProject> lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);
            TblProject tblProject = lstTblProject[0];

            // act
            await _projectService.DeleteProjectAsync(tblProject).ConfigureAwait(false);

            // assert
            lstTblProject = await _projectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsTrue(lstTblProject.Count == 0);
        }
    }
}
