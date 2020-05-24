﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MLDShopping_Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Components
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
    }
    public class BreadcrumbActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Path.HasValue && context.HttpContext.Request.Path.Value.Contains("Api"))
            {
                base.OnActionExecuted(context);
                return;
            }

            var breadcrumbs = this.ConfigureBreadcrumb(context);

            var controller = context.Controller as Controller;
            controller.ViewBag.Breadcrumbs = breadcrumbs;

            base.OnActionExecuted(context);
        }

        private List<Breadcrumb> ConfigureBreadcrumb(ActionExecutedContext context)
        {
            var breadcrumbList = new List<Breadcrumb>();

            breadcrumbList.Add(new Breadcrumb
            {
                Text = "Dashboard",
                Action = "Index",
                Controller = "Home", // Change this controller name to match your Home Controller.
                Area = "Dashboard",
                Active = true
            });

            if (context.HttpContext.Request.Path.HasValue)
            {
                var pathSplit = context.HttpContext.Request.Path.Value.Split("/");
                var area = context.RouteData.Values["area"].ToString();
                var controllerText = context.RouteData.Values["controller"];
                var action = context.RouteData.Values["action"];
                for (var i = 0; i < pathSplit.Length; i++)
                {
                    if (string.IsNullOrEmpty(pathSplit[i]))
                    {
                        continue;
                    }
                    //convert first letter to uppercase
                    pathSplit[i] = pathSplit[i].UppercaseFirst();

                    // Check if first element is equal to our Index (portal) page.
                    if (string.Compare(pathSplit[i], "Dashboard", true) == 0)
                    {
                        break;
                    }
                    //Check if this is an area.


                    var controller = Extensions.GetControllerType(pathSplit[i] + "Controller");
                    // First check if path is a Controller class.



                    // If this is a controller, does it have a default Index method? If so, that needs adding as a breadcrumb. Is the next path element called Index?
                    if (controller != null)
                    {
                        var indexMethod = controller.GetMethod("Index");

                        if (indexMethod != null)
                        {
                            breadcrumbList.Add(new Breadcrumb
                            {
                                Text = Extensions.CamelCaseSpacing(pathSplit[i]),
                                Action = "Index",
                                Controller = pathSplit[i],
                                Area = area,
                                Active = true
                            });

                            if (i + 1 < pathSplit.Length && string.Compare(pathSplit[i + 1], "Index", true) == 0)
                            {
                                // This is the last element in the breadcrumb list. This should be disabled.
                                breadcrumbList.LastOrDefault().Active = false;

                                // Next path item is the Index method. We can escape from this method now.
                                return breadcrumbList;
                            }
                        }
                    }

                    // If not a Controller, check if this is a method on the previous path element.
                    if (i - 1 > 0)
                    {
                        var controllerName = pathSplit[i - 1] + "Controller";
                        var prevController = Extensions.GetControllerType(controllerName);

                        if (prevController != null)
                        {
                            var method = prevController.GetMethods().FirstOrDefault(f => f.Name == pathSplit[i]);

                            if (method != null)
                            {
                                // We've found an endpoint on the previous controller.
                                breadcrumbList.Add(new Breadcrumb
                                {
                                    Text = Extensions.CamelCaseSpacing(pathSplit[i]),
                                    Action = pathSplit[i],
                                    Controller = pathSplit[i - 1],
                                    Area = area,
                                });
                            }
                        }
                    }
                }
            }

            // There will always be at least 1 entry in the breadcrumb list. The last element should always be disabled.
            breadcrumbList.LastOrDefault().Active = false;

            return breadcrumbList;
        }
    }
}
