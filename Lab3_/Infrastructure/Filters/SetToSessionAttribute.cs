using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lab3_.Infrastructure.Filters
{
    //Фильтр действий для запись в сессию данных из ModelState
    public class SetToSessionAttribute : Attribute, IActionFilter
    {
        private readonly string _name; //имя ключа

        public SetToSessionAttribute(string name)
        {
            _name = name;
        }

        // Выполняется до выполнения метода контроллера, но после привязки данных передаваемых в контроллер
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        // Выполняется после выполнения метода контроллера
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var dict = new Dictionary<string, string>();
            // считывание данных из ModelState и запись в сессию
            if (context.ModelState != null)
            {
                foreach (var item in context.ModelState) dict.Add(item.Key, item.Value.AttemptedValue);
                context.HttpContext.Session.Set(_name, dict);
            }
        }
    }
}