using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDo_Mvc_App.Helper;
using ToDo_Mvc_App.Models;

namespace ToDo_Mvc_App.Controllers
{

    public class ToDoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ToDoController> _logger;
        ToDoAPI _toDoApi;

        public ToDoController(IConfiguration configuration, ILogger<ToDoController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _toDoApi = new ToDoAPI(_configuration);
        }

        public async Task<IActionResult> Index()
        {
            List<ToDoViewModel> toDos = new List<ToDoViewModel>();

            try
            {
                HttpClient client = _toDoApi.Init();
                HttpResponseMessage responseMessage = await client.GetAsync("api/todo");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var results = responseMessage.Content.ReadAsStringAsync().Result;

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                    toDos = JsonConvert.DeserializeObject<List<ToDoViewModel>>(results);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(toDos);
        }

        // GET: ToDoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                StringValues title;
                collection.TryGetValue("Title", out title);

                StringValues description;
                collection.TryGetValue("Description", out description);

                ToDoViewModel toDoViewModel = new ToDoViewModel()
                {
                    Title = title.ToString(),
                    Description = description.ToString()
                };

                HttpClient client = _toDoApi.Init();
                HttpResponseMessage createToDoTask = await client.PostAsJsonAsync<ToDoViewModel>("api/todo", toDoViewModel);

                if (createToDoTask.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                throw;
            }

            return View();
        }

        // GET: ToDoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ToDoViewModel toDo = null;

            try
            {
                HttpClient client = _toDoApi.Init();
                HttpResponseMessage responseMessage = await client.GetAsync(string.Format("/api/todo/{0}", id));

                if (responseMessage.IsSuccessStatusCode)
                {
                    var results = responseMessage.Content.ReadAsStringAsync().Result;

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                    toDo = JsonConvert.DeserializeObject<ToDoViewModel>(results);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(toDo);
        }

        // POST: ToDoController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                StringValues title;
                collection.TryGetValue("Title", out title);

                StringValues description;
                collection.TryGetValue("Description", out description);

                ToDoViewModel toDoViewModel = new ToDoViewModel()
                {
                    Title = title.ToString(),
                    Description = description.ToString()
                };

                HttpClient client = _toDoApi.Init();
                HttpResponseMessage createToDoTask = await client.PostAsJsonAsync<ToDoViewModel>(string.Format("api/todo/{0}", id), toDoViewModel);

                if (createToDoTask.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                throw;
            }

            return View();
        }

        // GET: ToDoController/Delete
        public async Task<ActionResult> Delete(int id)
        {
            ToDoViewModel toDo = null;

            try
            {
                HttpClient client = _toDoApi.Init();
                HttpResponseMessage responseMessage = await client.GetAsync(string.Format("/api/todo/{0}", id));

                if (responseMessage.IsSuccessStatusCode)
                {
                    var results = responseMessage.Content.ReadAsStringAsync().Result;

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                    toDo = JsonConvert.DeserializeObject<ToDoViewModel>(results);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(toDo);
        }

        // POST: ToDoController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                HttpClient client = _toDoApi.Init();
                HttpResponseMessage createToDoTask = await client.DeleteAsync(string.Format("api/todo/{0}", id));

                if (createToDoTask.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}