using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task = PanOpticon.Models.Task;
namespace PanOpticon.TagHelpers
{

    [HtmlTargetElement("pancal", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CalendarTagHelper : TagHelper
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public List<Task> Tasks { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "section";
            output.Attributes.Add("class", "pancal");
            output.Attributes.Add("month", Month);
            output.Attributes.Add("year", Year);
            output.Attributes.Add("id", "pancalid");
            output.Content.SetHtmlContent(GetHTML());
            output.TagMode = TagMode.StartTagAndEndTag;
        }


        private string GetHTML()
        {
            DateTime MonthStart = new(Year, Month, 1);
            var tasks = Tasks?.GroupBy(t => t.DueDate.ToString("M/d/yyyy"));

            //XDocument xDocument = new(
            //    new XElement("div", new XAttribute("class", "container-fluid calen"),
            //            new XElement("header", new XElement("h4", new XAttribute("class", "display-4 mb-2 text-center"),
            //            MonthStart.ToString("MMMM yyyy") ,new XElement("div", new XAttribute("class", "row d-none d-lg-flex p1 text-white calBack"),
            //            Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(d=> new XElement("h5", new XAttribute("class", "col-lg p1 text-center"), d.ToString()
            //    )))
            //            ), new XElement("div", new XAttribute("class", "row border border-right-0 border-bottom-0"),GetDates())
            //            ))
            //    );
            //return xDocument.ToString();

            XDocument xDocument = new
                (

                new XElement("div", new XAttribute("class", "container-fluid bg-primary border"),
                        new XElement("header", new XAttribute("class", "row"),
                       new XElement("button", new XAttribute("id", "calBtnPrev"), new XAttribute("class", "col-2 calen btn"), new XElement("span", new XAttribute("class", "fal fa-angle-double-left fa-3x text-muted"))),
                            new XElement("h4", new XAttribute("class", "display-4 col-8 mb-0"), MonthStart.ToString("MMMM yyyy")),
                            new XElement("button", new XAttribute("id", "calBtnNext"), new XAttribute("class", "col-2 calen btn"), new XElement("span", new XAttribute("class", "fal fa-angle-double-right fa-3x text-muted"))),
                            new XElement("div", new XAttribute("class", "row d-none d-lg-flex p1 m-0 text-white bg-primary p-0"),
                                Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(d => new XElement("h5", new XAttribute("class", "col-lg p1 text-center"), d.ToString())),
                               new XElement("div", new XAttribute("class", "row border m-0 p-0 border-right-0 border-bottom-0"), GetDates())
                                         )

                            )
                        )
                );


            return xDocument.ToString();

            void GetTaskOnThisDate(DateTime d)
            {
                // Debug.WriteLine(d.ToString("M/d/yyyy"));
                //var x = tasks?.SingleOrDefault(t => t.Key == d.ToString("M/d/yyyy")).First();
                // var x = 
                var x = tasks.SingleOrDefault(t => t.Key.Equals(d.ToString("M/d/yyyy"))) ;
            }

            IEnumerable<XElement> GetTasks(DateTime date)
            {
                return tasks?.SingleOrDefault(t => t.Key.Equals( date.ToString("M/d/yyy") ))?.Select(ta => 

                new XElement("a", new XAttribute("class", $"event d-block p1 pl-2 pr-2 mb-1 rounded text-truncate small bg-secondary text-white {ta.TaskCss}"),ta.TaskName ,new XAttribute("title", ta.TaskName))) ?? new[]
                {
                    new XElement("p", new XAttribute("class", "d-lg-none"), "no event")
            };
            }
            // get dates
            IEnumerable<XElement> GetDates()
            {
                DateTime StartDate = MonthStart.AddDays(-(int)MonthStart.DayOfWeek);
                IEnumerable<DateTime> Dates = Enumerable.Range(0, 42).Select(d => StartDate.AddDays(d));

                foreach (DateTime date in Dates)
                {
                    if (date.DayOfWeek == DayOfWeek.Sunday && date != StartDate)
                    {
                        yield return new XElement("div", new XAttribute("class", "w-100"), "");
                    }

                    //GetTaskOnThisDate(date);

                    string differentMonth = "d-none d-lg-inline-block bg-secondary";
                    yield return new XElement("div", 
                        new XAttribute("class", $"day col-lg p-2 border border-left-0 border-top-0 text-truncate {(date.Month != MonthStart.Month ? differentMonth : null)}"), 
                        new XElement("h5",  new XAttribute("class", "row align-items-center"), 
                        new XElement("span", new XAttribute("class", "date col-1"), date.Day), 
                        new XElement("small", new XAttribute("class", "col d-lg-none text-center text-muted"), date.DayOfWeek.ToString() ),
                        new XElement("span", new XAttribute("class", "col-1"), String.Empty
                                    )
                        ),
                    GetTasks(date)
                   );

                }

            }




    }

    // get events


    }
}
