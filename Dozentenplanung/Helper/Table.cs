using System;
using System.Collections.Generic;

namespace Dozentenplanung.Helper
{
    public class Table
    {
        public int NumberOfColumns { get; set; }
        public List<List<string>> Rows { get; set;}
        public List<string> headings { get; set; }

        public Table(int numberOfColumns)
        {
            this.NumberOfColumns = numberOfColumns;
            this.Rows = new List<List<string>>();
            this.headings = new List<string>();
        }

        public void AddHeading(string aString) {
            this.headings.Add(aString);
        }

        public void AddRow(List<string> row) {
            this.Rows.Add(row);
        }

        public string CreateHtml() {
            string resultString = "<table class='reportTable'>";

            foreach (string eachHeading in headings) {
                resultString += "<th>" + eachHeading + "</th>";
            }

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