using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace MockResponse.Web.Infrastructure
{
    public static class UrlHelperExtensions
    {
        public static TagBuilder Action<TController>(this IUrlHelper helper, Expression<Action<TController>> action, string text, object routeValues = null)
        {
            var controllerName = typeof(TController)
                .Name
                .Remove(
                    typeof(TController)
                        .Name
                        .LastIndexOf("Controller", StringComparison.Ordinal));

            var actionInfo = GetMemberInfo(action) as MethodInfo;

            if (actionInfo == null)
            {
                throw new ArgumentException("Action not a method");
            }

            var parameters = actionInfo
                .GetParameters();

            var actionName = actionInfo.Name;

            var url = helper.Action(actionName, controllerName, routeValues, "http");
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("href", url);
            tagBuilder.InnerHtml.Append(text);
            return tagBuilder;
        }

        static MemberInfo GetMemberInfo<TController>(Expression<Action<TController>> expression)
        {
            var member = expression.Body as MethodCallExpression;
            if (member != null)
            {
                return member.Method;
            }

            throw new ArgumentException("Expression is not a member access expression", "expression");
        }
    }
}