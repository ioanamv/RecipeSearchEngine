using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using RecipeSearchEngine.Models;

namespace RecipeSearchEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public HomeController(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.HasSearched = false;
            return View(new List<Recipe>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string query)
        {
            ViewBag.HasSearched = true;
            ViewBag.Query= query;

            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Recipe>());
            }

            var response = await _elasticsearchClient.SearchAsync<Recipe>(s => s
                .Indices("recipes")
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(query)
                        .Fields(
                            f => f.Title,
                            f => f.CookingTimeMinutes,
                            f => f.Difficulty,
                            f => f.DietaryRestrictions,
                            f => f.Cuisine,
                            f => f.Ingredients,
                            f => f.Instructions
                        )                       
                    )
                )
            );

            var results=response.IsValidResponse? response.Documents.ToList() : new List<Recipe>();
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _elasticsearchClient.SearchAsync<Recipe>(s => s
                .Indices("recipes")
                .Query(q => q
                    .Term(t => t.Field(f => f.Id).Value(id))
                )
            );

            var recipe = response.Documents.FirstOrDefault();

            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return View(recipe);
        }
    }
}
