using System;
using System.Web.Http.Routing;

namespace ExploreCalifornia.MyAttribute
{
    public class MyUriAttribute : Attribute, IHttpRouteInfoProvider
    {
        public MyUriAttribute(string name, string template, int order)
        {
            Name = name;
            Template = template;
            Order = order;
        }
        //
        // Returns:
        //     Returns System.String.
        public string Name { get; set; }
        //
        // Returns:
        //     Returns System.Int32.
        public int Order { get; set; }
        //
        // Returns:
        //     Returns System.String.
        public string Template { get; }
    }
}
