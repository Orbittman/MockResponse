using System;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MockResponse.Web.Extensions
{
    public static class RouteBuilderExtensions
    {
        public static void AddRoute<TController>(this IRouteBuilder builder, string routeName, string url, Expression<Action<TController>> action)
            where TController : Controller
        {
            var routeInfo = GetRouteInfo(action);
            builder.MapRoute(routeName, url, new { controller = routeInfo.ControllerName, action = routeInfo.ActionName });
        }

        public static RouteInfo GetRouteInfo<TController>(
            Expression<Action<TController>> expression)
            where TController : Controller
        {
            var controllerName = typeof(TController)
                .Name
                .Remove(
                    typeof(TController)
                        .Name
                        .LastIndexOf("Controller", StringComparison.Ordinal));

            var actionInfo = GetMemberInfo(expression) as MethodInfo;

            if (actionInfo == null)
            {
                throw new ArgumentException("Action not a method");
            }

            var actionName = actionInfo.Name;

            return new RouteInfo
            {
                ControllerName = controllerName,
                ActionName = actionName.Replace("Async", string.Empty)
            };
        }

        private static MemberInfo GetMemberInfo<TController>(Expression<Action<TController>> expression)
        {
            var member = expression.Body as MethodCallExpression;
            if (member != null)
            {
                return member.Method;
            }

            throw new ArgumentException("Expression is not a member access", nameof(expression));
        }

        public class RouteInfo
        {
            public string ActionName { get; set; }

            public string ControllerName { get; set; }
        }
    }
}
