﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models.Enumerations;
using Textile;

namespace System.Web.Mvc.Html
// ReSharper restore CheckNamespace
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString SelectJsonValues<T>(this HtmlHelper<IEnumerable<T>> htmlHelper,
                                                        Expression<Func<T, object>> id, Expression<Func<T, object>> text,
                                                        string emptySelectionText = null)
        {
            return SelectJsonValues(htmlHelper, htmlHelper.ViewData.Model, id, text, emptySelectionText);
        }

        public static MvcHtmlString SelectJsonValues<T>(this HtmlHelper<IEnumerable<T>> htmlHelper,
                                                        IEnumerable<T> models, Expression<Func<T, object>> id,
                                                        Expression<Func<T, object>> text,
                                                        string emptySelectionText = null)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (text == null) throw new ArgumentNullException("text");
            if (models == null) throw new ArgumentNullException("models");

            var propertyId = ReflectionHelper.GetProperty(id);
            var propertyText = ReflectionHelper.GetProperty(text);

            var list = new ListDictionary();

            if (emptySelectionText != null)
            {
                list.Add(string.Empty, emptySelectionText);
            }

            foreach (var model in models)
            {
                var valueId = propertyId.GetValue(model, null);
                var valueText = propertyText.GetValue(model, null);

                list.Add(valueId, valueText);
            }
            return new MvcHtmlString(JsonConvert.SerializeObject(list));
        }

        public static MvcHtmlString SelectJsonValues(this HtmlHelper htmlHelper, IEnumerable<Enumeration> models)
        {
            if (models == null) throw new ArgumentNullException("models");

            var list = new ListDictionary();
            foreach (var model in models)
            {
                list.Add(model.Value, model.DisplayName);
            }
            return new MvcHtmlString(JsonConvert.SerializeObject(list));
        }

        public static MvcHtmlString Textile(this HtmlHelper htmlHelper, string textileContent)
        {
            var output = new StringBuilderTextileFormatter();
            var formatter = new TextileFormatter(output);

            formatter.Format(textileContent);

            return new MvcHtmlString(output.GetFormattedText());
        }

        public static MvcHtmlString ValidationSummaryWithMessage(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewData.ModelState.IsValid) return null;
            return htmlHelper.ValidationSummary("Uppss.. corrija los siguientes puntos antes de continuar:");
        }

        public static MvcHtmlString Submit(this HtmlHelper htmlHelper, string value = null)
        {
            return htmlHelper.Partial("_Submit", value);
        }


        //http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
        #region Collection Items Extensions
        private const string IDS_TO_REUSE_KEY = "__htmlPrefixScopeExtensions_IdsToReuse_";

        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

            return BeginHtmlFieldPrefixScope(html, string.Format("{0}[{1}]", collectionName, itemIndex));
        }

        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        private static Queue<string> GetIdsToReuse(HttpContextBase httpContext, string collectionName)
        {
            // We need to use the same sequence of IDs following a server-side validation failure,  
            // otherwise the framework won't render the validation error messages next to each item.
            string key = IDS_TO_REUSE_KEY + collectionName;
            var queue = (Queue<string>)httpContext.Items[key];
            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();
                var previouslyUsedIds = httpContext.Request[collectionName + ".index"];
                if (!string.IsNullOrEmpty(previouslyUsedIds))
                    foreach (string previouslyUsedId in previouslyUsedIds.Split(','))
                        queue.Enqueue(previouslyUsedId);
            }
            return queue;
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this._templateInfo = templateInfo;

                _previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousHtmlFieldPrefix;
            }
        }
        #endregion
    }
}