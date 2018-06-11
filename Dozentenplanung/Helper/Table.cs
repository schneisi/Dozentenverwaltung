using System;
using System.Collections.Generic;

namespace Dozentenplanung.Helper
{
    public class Table
    {
        public int NumberOfColumns { get; set; }
        public List<List<string>> Rows { get; set;}

        public Table(int numberOfColumns)
        {
            this.NumberOfColumns = numberOfColumns;
            this.Rows = new List<List<string>>();
        }

        public void AddRow(List<string> row) {
            this.Rows.Add(row);
        }

        public string CreateHtml() {
            string resultString = "<table>";
            foreach (List<string> eachRow in this.Rows) {
                resultString += "<tr>";
                foreach (string eachColumn in eachRow) {
                    resultString += "<td>" + eachColumn + "</td>";
                }
                resultString += "</tr>";
            }
            resultString += "</table>";
            return resultString;
        }
    }
}