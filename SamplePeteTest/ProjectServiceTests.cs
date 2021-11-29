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
        [TestMethod]
        public async Task CreateProject_RecordAdded()
        {
            // arrange
            TblProject tblProject = new()
            {
                ProjectName = "New Project",
                StartDate = new DateTime(2021, 11, 27),
                EndDate = new DateTime(2021, 11, 29)
            };

            // act
            await ProjectService.CreateProjectAsync(tblProject).ConfigureAwait(false);

            // assert
            List<TblProject> lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsNotNull(lstTblProject.Find(x => x.ProjectName == "New Project"));
        }

        [TestMethod]
        public async Task GetProjects_ReturnsList()
        {
            // arrange

            // act
            List<TblProject> lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);

            // assert
            Assert.IsNotNull(lstTblProject);
        }

        [TestMethod]
        public async Task UpdateProject_RecordUpdated()
        {
            // arrange
            List<TblProject> lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);
            TblProject tblProject = lstTblProject.Find(x => x.ProjectName == "New Project");
            tblProject.ProjectName = "A Project";

            // act
            await ProjectService.UpdateProjectAsync(tblProject).ConfigureAwait(false);

            // assert
            lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsTrue(string.Compare(lstTblProject.Find(x => x.ProjectName == "A Project").ProjectName, "A Project") == 0);
        }

        [TestMethod]
        public async Task DeleteProject_RecordRemoved()
        {
            // arrange
            List<TblProject> lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);
            TblProject tblProject = lstTblProject.Find(x => x.ProjectName == "A Project");

            // act
            await ProjectService.DeleteProjectAsync(tblProject).ConfigureAwait(false);

            // assert
            lstTblProject = await ProjectService.GetProjectsAsync().ConfigureAwait(false);
            Assert.IsNull(lstTblProject.Find(x => x.ProjectName == "A Project"));
        }
    }
}
