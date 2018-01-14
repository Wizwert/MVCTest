using System.Web;
using System.Web.Mvc;
using MVCTest.Models;
using MVCTest.ViewModel;
using Newtonsoft.Json;

namespace MVCTest.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString HtmlConvertToJson(this HtmlHelper htmlHelper,
            object model)
        {
            var settings = new JsonSerializerSettings
                           {
                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                               Formatting = Formatting.Indented
                           };

            return new HtmlString(JsonConvert.SerializeObject(model, settings));
        }

        public static MvcHtmlString BuildSortableLink(this HtmlHelper htmlHelper,
            string fieldName, string actionName, string sortField, QueryOptions queryOptions)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var isCurrentSortField = queryOptions.SortField == sortField;

            return new MvcHtmlString($"<a href=\"{urlHelper.Action(actionName, new {SortField = sortField, SortOrder = (isCurrentSortField && queryOptions.SortOrder == SortOrder.ASC) ? SortOrder.DESC : SortOrder.ASC})}\">{fieldName} {BuildSortIcon(isCurrentSortField, queryOptions)}</a>");
        }

        private static string BuildSortIcon(bool isCurrentSortField
            , QueryOptions queryOptions)
        {
            string sortIcon = "sort";

            if (isCurrentSortField)
            {
                sortIcon += "-by-alphabet";
                if (queryOptions.SortOrder == SortOrder.DESC)
                    sortIcon += "-alt";
            }

            return $"<span class=\"glyphicon glyphicon-{sortIcon}\"></span>";
        }

        public static MvcHtmlString BuildNextPreviousLinks(
            this HtmlHelper htmlHelper, QueryOptions queryOptions, string actionName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            return new MvcHtmlString("<nav>" + "    <ul class=\"pager\">" + $"      <li class=\"previous {IsPreviousDisabled(queryOptions)}\">{BuildPreviousLink(urlHelper, queryOptions, actionName)}</li>"
                                     + $"      <li class=\"next {IsNextDisabled(queryOptions)}\">{BuildNextLink(urlHelper, queryOptions, actionName)}</li>" + "    </ul>" + "</nav>");
        }

        private static string IsPreviousDisabled(QueryOptions queryOptions)
        {
            return (queryOptions.CurrentPage == 1)
                ? "disabled" : string.Empty;
        }

        private static string IsNextDisabled(QueryOptions queryOptions)
        {
            return (queryOptions.CurrentPage == queryOptions.TotalPages)
                ? "disabled" : string.Empty;
        }

        private static string BuildPreviousLink(
            UrlHelper urlHelper, QueryOptions queryOptions, string actionName)
        {
            return $"<a href=\"{urlHelper.Action(actionName, new { queryOptions.SortOrder, queryOptions.SortField, CurrentPage = queryOptions.CurrentPage - 1, queryOptions.PageSize})}\"><span aria-hidden=\"true\">&larr;</span> Previous</a>";
        }

        private static string BuildNextLink(
            UrlHelper urlHelper, QueryOptions queryOptions, string actionName)
        {
            return $"<a href=\"{urlHelper.Action(actionName, new { queryOptions.SortOrder, queryOptions.SortField, CurrentPage = queryOptions.CurrentPage + 1, queryOptions.PageSize})}\">Next <span aria-hidden=\"true\">&rarr;</span></a>";
        }
    }
}