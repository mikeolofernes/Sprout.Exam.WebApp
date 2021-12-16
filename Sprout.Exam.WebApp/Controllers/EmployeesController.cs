using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.DataAccess.Data;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDBContext _context;
        private readonly ICalculateService _calcContext;
        public EmployeesController(EmployeeDBContext context, ICalculateService calcContext)
        {
            _context = context;
            _calcContext = calcContext;
        }
        //public EmployeesController(ICalculateService calcContext)
        //{
        //    _calcContext = calcContext;
        //}
        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Task.FromResult(_context.Employee.ToList());
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {
                var item = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == input.Id));
                if (item == null) return NotFound();
                item.FullName = input.FullName;
                item.Tin = input.Tin;
                item.Birthdate = Convert.ToDateTime(input.Birthdate.ToString("yyyy-MM-dd"));
                item.TypeId = input.TypeId;
                _context.Employee.Update(item);
               
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            try
            {
                var id = await Task.FromResult(_context.Employee.Max(m => m.Id) + 1);

                EmployeeDto employee = new EmployeeDto
                {
                    Birthdate = Convert.ToDateTime(input.Birthdate.ToString("yyyy-MM-dd")),
                    FullName = input.FullName,
                    Tin = input.Tin,
                    TypeId = input.TypeId,
                };
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
                return Created($"/api/employees/{id}", id);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex);
            }
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
                if (result == null) return NotFound();

                _context.Employee.Remove(result);
                _context.SaveChanges();
                return Ok(id);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex);
            }
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate/{absentDays}/{workedDays}")]
        public async Task<IActionResult> Calculate(int id, decimal absentDays, decimal workedDays)
        {
            try
            {
                var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));

                if (result == null) return NotFound();
                var type = (EmployeeType)result.TypeId;

                return type switch
                {
                    EmployeeType.Regular =>
                        //create computation for regular.                    
                        Ok(_calcContext.ComputeRegular(20000, absentDays)),
                    EmployeeType.Contractual =>
                        //create computation for contractual.
                        Ok(_calcContext.ComputeContactual(500, workedDays)),
                    _ => NotFound("Employee Type not found")
                };
            }
            catch (Exception ex)
            {
                return this.NotFound(ex);
            }

        }

        

    }
}
