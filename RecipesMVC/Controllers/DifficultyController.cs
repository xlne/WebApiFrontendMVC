using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipesMVC.Controllers
{
    public class DifficultyController : Controller
    {
        // GET: DifficultyController
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var url = "https://localhost:5001/api/difficulty/";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var difficultys = JsonSerializer.Deserialize<IList<ComplexityModel>>(content, options);

            return View(difficultys);
        }

        // GET: DifficultyController/Details/5
        // GET: RecipesController/Difficulty/
        [HttpGet]
        public async Task<IActionResult> Difficulty(int id)
        {
            var url = "https://localhost:5001/api/difficulty/";
            using var client = new HttpClient();
            var response = await client.GetAsync(url + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var difficulties = JsonSerializer.Deserialize<IList<ComplexityModel>>(content, options);

            return View(difficulties);
        }

        // GET: DifficultyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Difficulty/Create. Creates a new difficulty level.
        [HttpPost]
        public async Task<IActionResult> Create(ComplexityModel complexity)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var url = $"https://localhost:5001/api/difficulty/";
            using var client = new HttpClient();

            var content = new StringContent(JsonSerializer.Serialize(complexity), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{response.StatusCode} ${message}");
            }
            return RedirectToAction("Index");
        }
                
        // POST: Difficulty/Edit - edit a difficulty
        [HttpPost]
        public async Task<IActionResult> Edit(ComplexityModel complexity)
        {
            try
            {
                var id = complexity.Id;
                var url = $"https://localhost:5001/api/difficulty/{id}";
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using var client = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(complexity), Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await client.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{response.StatusCode} ${response.ToString()}");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: DifficultyController/Edit/ - get the difficulty that is going to be modified
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var url = "https://localhost:5001/api/difficulty/";
            using var client = new HttpClient();
            var response = await client.GetAsync(url + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var complexity = JsonSerializer.Deserialize<ComplexityModel>(content, options);

            return View(complexity);
        }

        // GET: DifficultyController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var url = "https://localhost:5001/api/difficulty/";
            using var client = new HttpClient();
            var response = await client.DeleteAsync(url + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: DifficultyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
