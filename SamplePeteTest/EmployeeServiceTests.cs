using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamplePeteService;
using SamplePeteService.Models;

namespace SamplePeteTest
{
    [TestClass]
    public class EmployeeServiceTests
    {
        private static TblEmployeeInfo _tblEmployeeInfo;
        private static Context _dbContext;
        private static EmployeeService _employeeService;

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            _tblEmployeeInfo = new()
            {
                FirstName = "Some",
                LastName = "Person",
                PositionTitle = "Temp",
                DateHired = DateTime.Now
            };
        }

        [TestInitialize]
        public void Setup()
        {
            _dbContext = new();
            _employeeService = new(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted(); // Remove from memory
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task CreateEmployee_RecordAdded()
        {
            // arrange

            // act
            await _employeeService.CreateEmployeeAsync(_tblEmployeeInfo).ConfigureAwait(false);

            // assert
            List<TblEmployeeInfo> lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsNotNull(lstTblEmployeeInfo[0]);
        }

        [TestMethod]
        public async Task GetEmployees_ReturnsList()
        {
            // arrange

            // act
            List<TblEmployeeInfo> lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);

            // assert
            Assert.IsNotNull(lstTblEmployeeInfo);
        }

        [TestMethod]
        public async Task UpdateEmployee_RecordUpdated()
        {
            // arrange
            await _employeeService.CreateEmployeeAsync(_tblEmployeeInfo).ConfigureAwait(false);
            List<TblEmployeeInfo> lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
            TblEmployeeInfo tblEmployeeInfo = lstTblEmployeeInfo[0];
            tblEmployeeInfo.FirstName = "Special";

            // act
            await _employeeService.UpdateEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);

            // assert
            lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsTrue(string.Compare(lstTblEmployeeInfo[0].FirstName, "Special") == 0);
        }

        [TestMethod]
        public async Task DeleteEmployee_RecordRemoved()
        {
            // arrange
            await _employeeService.CreateEmployeeAsync(_tblEmployeeInfo).ConfigureAwait(false);
            List<TblEmployeeInfo> lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
            TblEmployeeInfo tblEmployeeInfo = lstTblEmployeeInfo[0];

            // act
            await _employeeService.DeleteEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);

            // assert
            lstTblEmployeeInfo = await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsTrue(lstTblEmployeeInfo.Count == 0);
        }
    }
}
