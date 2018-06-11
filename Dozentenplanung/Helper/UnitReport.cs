using System;
using System.Collections.Generic;
using Dozentenplanung.Models;

namespace Dozentenplanung.Helper
{
    public class UnitReport
    {
        public UnitSearch Search { get; set; }
        public UnitReport()
        {
        }

        public string CreateReport() {
            Table table = new Table(5);
            foreach (Unit eachUnit in Search.Search()) {
                List<string> tuple = new List<string>();
                tuple.Add(eachUnit.Designation);
                tuple.Add(eachUnit.Title);
                tuple.Add(eachUnit.ExamType);
                tuple.Add(eachUnit.DurationOfExam.ToString());
                tuple.Add(eachUnit.LecturerName);
                table.AddRow(tuple);
            }
            return table.CreateHtml();
        }
    }
}
