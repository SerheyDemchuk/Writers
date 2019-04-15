using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Writers.Extensions
{
    public static class HtmlExtensions
    {
        public static string BuildBreadcrumbs(this HtmlHelper helper)
        {
            if(helper.ViewContext.RouteData.Values["Controller"].ToString() == "Home")
            {
                return string.Empty;
            }

            StringBuilder breadcrumb = new StringBuilder("<ul class='bread'><li>").Append(helper.ActionLink("Home", "Index", "Home").ToHtmlString()).Append("</li>");

            breadcrumb.Append("<li>");

            if (helper.ViewContext.RouteData.Values["Controller"].ToString() == "Person")
            {
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["Controller"].ToString().Titleize() + "alities",
                "Index",
                helper.ViewContext.RouteData.Values["Controller"].ToString()));
                breadcrumb.Append("</li>");
            }
            else
            {
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["Controller"].ToString().Titleize(),
                "Index",
                helper.ViewContext.RouteData.Values["Controller"].ToString()));
                breadcrumb.Append("</li>");
            }

            if(helper.ViewContext.RouteData.Values["Action"].ToString() != "Index")
            {
                if (helper.ViewContext.RouteData.Values["Action"].ToString() == "Details")
                {
                    breadcrumb.Append("<li>");
                    breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["id"].ToString(),
                        helper.ViewContext.RouteData.Values["Action"].ToString(),
                        helper.ViewContext.RouteData.Values["Controller"].ToString()));
                    breadcrumb.Append("</li>");
                }
                else
                {
                    breadcrumb.Append("<li>");
                    breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["Action"].ToString().Titleize(),
                        helper.ViewContext.RouteData.Values["Action"].ToString(),
                        helper.ViewContext.RouteData.Values["Controller"].ToString()));
                    breadcrumb.Append("</li>");
                }
            }
            return breadcrumb.Append("</ul>").ToString();
        }

        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, object htmlAttributes)
        {
            //var img = "Image";
            //if(image != null)
            //{
            //    img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            //}
            //return new MvcHtmlString("<img src='" + img + "' />");

            TagBuilder tb = new TagBuilder("img");

            if(image != null)
            {
                var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
                tb.Attributes.Add("src", String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image)));
            }

            RouteValueDictionary htmlAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            foreach (var thisAttribute in htmlAttrs)
            {
                tb.Attributes.Add(thisAttribute.Key, thisAttribute.Value.ToString());
            }

            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));

        }
    }
}