using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamplePeteService;
using SamplePeteService.Models;

namespace SamplePeteWeb.Controllers
{
    [Route("api/EmployeeAPI")]
    [ApiController]
    public class EmployeeAPIController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeAPIController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Route("Employee")]
        [HttpPost]
        public async Task CreateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            await _employeeService.CreateEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);
        }

        [Route("Employees")]
        [HttpGet]
        public async Task<List<TblEmployeeInfo>> EmployeesAsync()
        {
            return await _employeeService.GetEmployeesAsync().ConfigureAwait(false);
        }

        [Route("Employee")]
        [HttpPatch]
        public async Task UpdateEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            await _employeeService.CreateEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);
        }

        [Route("Employee")]
        [HttpDelete]
        public async Task DeleteEmployeeAsync(TblEmployeeInfo tblEmployeeInfo)
        {
            await _employeeService.DeleteEmployeeAsync(tblEmployeeInfo).ConfigureAwait(false);
        }
    }
}
