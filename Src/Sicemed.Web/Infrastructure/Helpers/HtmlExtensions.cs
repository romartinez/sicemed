using System.Collections.Generic;
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
            return htmlHelper.ValidationSummary("Uppss.. corrija los siguientes puntos antes de continuar:");
        }    
    }
}