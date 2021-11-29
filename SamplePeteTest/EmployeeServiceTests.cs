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
        [TestMethod]
        public async Task CreateEmployee_RecordAdded()
        {
            // arrange
            TblEmployeeInfo tblEmployeeInfo = new()
            {
                FirstName = "Some",
                LastName = "Person",
                PositionTitle = "Temp",
                DateHired = DateTime.Now
            };

            // act
            await EmployeeService.CreateEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);

            // assert
            List<TblEmployeeInfo> lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsNotNull(lstTblEmployeeInfo.Find(x => x.PositionTitle == "Temp"));
        }

        [TestMethod]
        public async Task GetEmployees_ReturnsList()
        {
            // arrange

            // act
            List<TblEmployeeInfo> lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);

            // assert
            Assert.IsNotNull(lstTblEmployeeInfo);
        }

        [TestMethod]
        public async Task UpdateEmployee_RecordUpdated()
        {
            // arrange
            List<TblEmployeeInfo> lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);
            TblEmployeeInfo tblEmployeeInfo = lstTblEmployeeInfo.Find(x => x.PositionTitle == "Temp");
            tblEmployeeInfo.FirstName = "Special";

            // act
            await EmployeeService.UpdateEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);

            // assert
            lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsTrue(string.Compare(lstTblEmployeeInfo.Find(x => x.PositionTitle == "Temp").FirstName, "Special") == 0);
        }

        [TestMethod]
        public async Task DeleteEmployee_RecordRemoved()
        {
            // arrange
            List<TblEmployeeInfo> lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);
            TblEmployeeInfo tblEmployeeInfo = lstTblEmployeeInfo.Find(x => x.PositionTitle == "Temp");

            // act
            await EmployeeService.DeleteEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);

            // assert
            lstTblEmployeeInfo = await EmployeeService.GetEmployeesAsync().ConfigureAwait(false);
            Assert.IsNull(lstTblEmployeeInfo.Find(x => x.PositionTitle == "Temp"));
        }
    }
}
