using System;
using System.Collections.Generic;
using System.Text;
using Customs.DAL;
using Customs.DAL.Models;
using Lab3_.Middleware;
using Lab3_.Services;
using Lab3_.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lab3_
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // внедрение зависимости для доступа к БД с использованием EF
            var connection = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<CustomsContext>(options => options.UseSqlServer(connection));
            // внедрение зависимости OperationService
            services.AddTransient<IProductsService, ProductsService>();
            // добавление кэширования
            services.AddMemoryCache();
            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            // добавляем поддержку статических файлов
            app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();

            app.UseMiddleware<InfoMiddleware>();

            // добавляем компонент middleware по инициализации базы данных и производим инициализацию базы
            app.UseDbInitializer();

            // добавляем компонент middleware для реализации кэширования и записывем данные в кэш
            app.UseOperatinCache("Storage 20");

            app.MapWhen(ctx => ctx.Request.Path == "/", Index);
            app.MapWhen(ctx => ctx.Request.Path == "/searchform1", CookieSearch);
            app.MapWhen(ctx => ctx.Request.Path == "/search1", CookieSearchHandler);
            app.MapWhen(ctx => ctx.Request.Path == "/searchform2", SessionSearch);
            app.MapWhen(ctx => ctx.Request.Path == "/search2", SessionSearchHandler);

            app.UseRouting();
        }

        private static string AppendProductTable(string htmlString, IEnumerable<ProductViewModel> products)
        {
            var stringBuilder = new StringBuilder(htmlString);

            stringBuilder.Append("<div style=\"display: inline-block;\"><H1>Товары</H1>");
            stringBuilder.Append("<TABLE BORDER=1>");
            stringBuilder.Append("<TH>");
            stringBuilder.Append("<TD>#</TD>");
            stringBuilder.Append("<TD>Наименование</TD>");
            stringBuilder.Append("<TD>Единица изм.</TD>");
            stringBuilder.Append("<TD>Склад</TD>");
            stringBuilder.Append("</TH>");

            foreach (var product in products)
            {
                stringBuilder.Append("<TR>");
                stringBuilder.Append("<TD>-</TD>");
                stringBuilder.Append($"<TD>{product.Id}</TD>");
                stringBuilder.Append($"<TD>{product.Name}</TD>");
                stringBuilder.Append($"<TD>{product.UnitMeasurement}</TD>");
                stringBuilder.Append($"<TD>{product.Storage}</TD>");
                stringBuilder.Append("</TR>");
            }

            stringBuilder.Append("</table>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        private static string AppendStorageTable(string htmlString, IEnumerable<Storage> storages)
        {
            var stringBuilder = new StringBuilder(htmlString);

            stringBuilder.Append("<div style=\"display: inline-block; margin-left: 35px;\"><H1>Склады</H1>");
            stringBuilder.Append("<TABLE BORDER=1>");
            stringBuilder.Append("<TH>");
            stringBuilder.Append("<TD>#</TD>");
            stringBuilder.Append("<TD>Наименование</TD>");
            stringBuilder.Append("</TH>");

            foreach (var storage in storages)
            {
                stringBuilder.Append("<TR>");
                stringBuilder.Append("<TD>-</TD>");
                stringBuilder.Append($"<TD>{storage.Id}</TD>");
                stringBuilder.Append($"<TD>{storage.Name}</TD>");
                stringBuilder.Append("</TR>");
            }

            stringBuilder.Append("</table>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        private static string AppendEmployeeTable(string htmlString, IEnumerable<Employee> employees)
        {
            var stringBuilder = new StringBuilder(htmlString);

            stringBuilder.Append("<div style=\"display: inline-block; margin-left: 35px;\"><H1>Сотрудники</H1>");
            stringBuilder.Append("<TABLE BORDER=1>");
            stringBuilder.Append("<TH>");
            stringBuilder.Append("<TD>#</TD>");
            stringBuilder.Append("<TD>Фамилия</TD>");
            stringBuilder.Append("<TD>Имя</TD>");
            stringBuilder.Append("<TD>Отчество</TD>");
            stringBuilder.Append("<TD>Номер удостоверения</TD>");
            stringBuilder.Append("<TD>Роль</TD>");
            stringBuilder.Append("</TH>");

            foreach (var employee in employees)
            {
                stringBuilder.Append("<TR>");
                stringBuilder.Append("<TD>-</TD>");
                stringBuilder.Append($"<TD>{employee.Id}</TD>");
                stringBuilder.Append($"<TD>{employee.LastName}</TD>");
                stringBuilder.Append($"<TD>{employee.FirstName}</TD>");
                stringBuilder.Append($"<TD>{employee.MiddleName}</TD>");
                stringBuilder.Append($"<TD>{employee.IdNumber}</TD>");
                stringBuilder.Append($"<TD>{employee.Role}</TD>");
                stringBuilder.Append("</TR>");
            }

            stringBuilder.Append("</table>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        private static string AppendMenu(string htmlString)
        {
            return htmlString +
                   "<div style=\"margin-top: 15px; margin-bottom: 15px;\">" +
                        "<div style=\"display: inline-block; margin-right: 10px;\"><a href=\"/info\">Информация</a></div>" +
                        "<div style=\"display: inline-block; margin-right: 10px;\"><a href=\"/\">Таблица с кешированием</a></div>" +
                        "<div style=\"display: inline-block; margin-right: 10px;\"><a href=\"/searchform1\">Поиск (куки)</a></div>" +
                        "<div style=\"display: inline-block; margin-right: 10px;\"><a href=\"/searchform2\">Поиск (сессия)</a></div>" +
                   "</div>";
        }

        private static void Index(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                var service = context.RequestServices.GetService<IProductsService>();

                if (service == null)
                {
                    throw new InvalidOperationException($"Unable to retrieve {nameof(IProductsService)} service");
                }

                var models = service.GetHomeViewModel("Storage 20");
                var htmlString = "<HTML><HEAD>" +
                                 "<TITLE>С кешированием</TITLE></HEAD>" +
                                 "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                                 "<BODY>";
                htmlString = AppendMenu(htmlString);
                htmlString = AppendProductTable(htmlString, models.Products);
                htmlString = AppendStorageTable(htmlString, models.Storages);
                htmlString = AppendEmployeeTable(htmlString, models.Employees);

                htmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(htmlString);
            });
        }

        private static void SessionSearchHandler(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                var storage = context.Request.Query["storage"].ToString();
                var name = context.Request.Query["name"].ToString();

                context.Session.Set("storage", Encoding.Default.GetBytes(storage));
                context.Session.Set("name", Encoding.Default.GetBytes(name));

                var service = context.RequestServices.GetService<IProductsService>();

                if (service == null)
                {
                    throw new InvalidOperationException($"Unable to retrieve {nameof(IProductsService)} service");
                }

                var htmlString = "<HTML><HEAD>" +
                                 "<TITLE>Поиск (куки)</TITLE></HEAD>" +
                                 "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                                 "<BODY>";

                var products = service.SearchProducts(storage, name);
                htmlString = AppendMenu(htmlString);
                htmlString = AppendProductTable(htmlString, products);

                return context.Response.WriteAsync(htmlString);
            });
        }

        private static void SessionSearch(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                var htmlString = "<HTML><HEAD>" +
                                 "<TITLE>Поиск (сессия)</TITLE></HEAD>" +
                                 "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                                 "<BODY>";
                htmlString = AppendMenu(htmlString);

                if (!context.Session.TryGetValue("storage", out var storageArray))
                {
                    storageArray = null;
                }

                var storageValue = storageArray == null
                    ? string.Empty
                    : Encoding.Default.GetString(storageArray);

                if (!context.Session.TryGetValue("name", out var nameArray))
                {
                    nameArray = null;
                }

                var name = nameArray == null
                    ? string.Empty
                    : Encoding.Default.GetString(nameArray);

                var service = context.RequestServices.GetService<IProductsService>();

                if (service == null)
                {
                    throw new InvalidOperationException($"Unable to retrieve {nameof(IProductsService)} service");
                }

                var storages = service.GetStorages();

                var selectHtml = "<select name='storage'>";
                foreach (var storage in storages)
                {
                    if (storage != string.Empty && storage == storageValue)
                    {
                        selectHtml += $"<option selected=\"selected\" value='{storage}'>" + storage + "</option>";
                    }

                    selectHtml += $"<option value='{storage}'>" + storage + "</option>";
                }

                selectHtml += "</select>";

                htmlString += "<form action = /search2 >" +
                "<br>Склад: " + selectHtml +
                "<br>Наименование: " + $"<input type = 'text' name = 'name' value='{name}'>" +
                    "<br><input type = 'submit' value = 'Найти' ></form>";


                htmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(htmlString);
            });
        }

        private static void CookieSearchHandler(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                var storageParam = context.Request.Query["storage"].ToString();
                var nameParam = context.Request.Query["name"].ToString();

                context.Response.Cookies.Append("storage", storageParam);
                context.Response.Cookies.Append("name", nameParam);

                var service = context.RequestServices.GetService<IProductsService>();

                if (service == null)
                {
                    throw new InvalidOperationException($"Unable to retrieve {nameof(IProductsService)} service");
                }

                var htmlString = "<HTML><HEAD>" +
                                 "<TITLE>Поиск (куки)</TITLE></HEAD>" +
                                 "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                                 "<BODY>";

                var products = service.SearchProducts(storageParam, nameParam);
                htmlString = AppendMenu(htmlString);
                htmlString = AppendProductTable(htmlString, products);

                return context.Response.WriteAsync(htmlString);
            });
        }

        private static void CookieSearch(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                var htmlString = "<HTML><HEAD>" +
                                 "<TITLE>Поиск (куки)</TITLE></HEAD>" +
                                 "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                                 "<BODY>";
                htmlString = AppendMenu(htmlString);

                if (!context.Request.Cookies.TryGetValue("storage", out var storage))
                {
                    storage = string.Empty;
                }

                if (!context.Request.Cookies.TryGetValue("name", out var name))
                {
                    name = string.Empty;
                }

                var service = context.RequestServices.GetService<IProductsService>();

                if (service == null)
                {
                    throw new InvalidOperationException($"Unable to retrieve {nameof(IProductsService)} service");
                }

                var storages = service.GetStorages();

                var selectHtml = "<select name='storage'>";
                foreach (var storageItem in storages)
                {
                    if (storage != string.Empty && storage == storageItem)
                    {
                        selectHtml += $"<option selected=\"selected\" value='{storageItem}'>" + storageItem + "</option>";
                    }

                    selectHtml += $"<option value='{storageItem}'>" + storageItem + "</option>";
                }

                selectHtml += "</select>";

                htmlString += "<form action = /search1 >" +
                "<br>Склад: " + selectHtml +
                "<br>Наименование: " + $"<input type = 'text' name = 'name' value='{name}'>" +
                    "<br><input type = 'submit' value = 'Найти' ></form>";


                htmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(htmlString);
            });
        }
    }
}