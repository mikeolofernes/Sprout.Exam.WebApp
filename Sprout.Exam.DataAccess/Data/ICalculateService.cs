using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Data
{
    public interface ICalculateService
    {
        decimal ComputeRegular(decimal baseSalary, decimal absentDays);
        decimal ComputeContactual(decimal baseSalary, decimal workedDays);
    }
}
