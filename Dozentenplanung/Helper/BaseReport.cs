using System;
namespace Dozentenplanung.Helper
{
    public abstract class BaseReport
    {
        
        public BaseReport()
        {
        }

        protected abstract string Title();
        protected string TitleString() {
            return "<h2 class='title'>" + this.Title() + "</h2>";
        }

        protected string HeadString() {
            string htmlString = "<html><head>";
            htmlString += "<meta charset='utf-8' />";
            htmlString += "<title>" + this.Title() + "</title>";
            htmlString += "<link href='/css/report.css' rel='stylesheet' />";
            htmlString += "<link href='https://cdnjs.cloudflare.com/ajax/libs/bulma/0.6.2/css/bulma.min.css' rel='stylesheet'/>";
            htmlString += "</head><body>";
            return htmlString;
        }

        protected string FootString() {
            return "<body></html>";
        }
    }
}
