using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Data
{
    public class CalculateService : ICalculateService
    {
        public decimal ComputeRegular(decimal baseSalary, decimal absentDays)
        {
            decimal computedSalary = 0;

            int taxPercentage = 12;

            var tmp = baseSalary - (baseSalary / 22 * absentDays);
            computedSalary = System.Math.Round(tmp - ((tmp / 100) * taxPercentage), 2);

            return computedSalary;
        }

        public decimal ComputeContactual(decimal baseSalary, decimal workedDays)
        {
            decimal computedSalary = 0;

            computedSalary = baseSalary * workedDays;

            return computedSalary;
        }
    }
}
