using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesMVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipesMVC.Controllers
{
    public class RecipesController : Controller
    {
        // GET all items in recipes
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var url = "https://localhost:5001/api/recipe/";
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

            var recipes = JsonSerializer.Deserialize<IList<RecipeModel>>(content, options);

            return View(recipes);
        }


        // GET recipes by Id. RecipesController/Recipes/
        [HttpGet] //("{id}")
        public async Task<IActionResult> Recipe(int id)
        {
            var url = $"https://localhost:5001/api/recipe/{id}/";
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

            var recipe = JsonSerializer.Deserialize<IList<RecipeModel>>(content, options);

            return View(recipe);
        }

        // GET: RecipesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipesController/Create

        [HttpPost]
        public async Task<IActionResult> Create(RecipeModel recipe)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var url = $"https://localhost:5001/api/recipe/";
            using var client = new HttpClient();

            var content = new StringContent(JsonSerializer.Serialize(recipe), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{response.StatusCode} ${message}");
            }
            return RedirectToAction("Index");
        }

        // GET: RecipesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(RecipeModel recipe)
        {
            try
            {
                var id = recipe.Id;
                var url = $"https://localhost:5001/api/recipe/{id}";
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using var client = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(recipe), Encoding.UTF8);
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

        // POST: RecipesController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var url = "https://localhost:5001/api/recipe/";
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

            var recipe = JsonSerializer.Deserialize<RecipeModel>(content, options);

            return View(recipe);
        }

        // GET: RecipesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var url = "https://localhost:5001/api/recipe/";
            using var client = new HttpClient();
            var response = await client.DeleteAsync(url + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: RecipesController/Delete/5
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
