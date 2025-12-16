using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.AspNetCore.Mvc;
using RecipeSearchEngine.Models.DTOs;
using RecipeSearchEngine.Models;
using System.Threading.Tasks;

namespace RecipeSearchEngine.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public RecipesController(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        /// <summary>
        /// Search for recipes based on a query string.
        /// </summary>
        /// <param name="query">Keywords for search</param>
        /// <returns>The list of the recipes</returns>
        [HttpGet]
        public async Task<IActionResult> SearchRecipes([FromQuery] string query)
        {
            //var mockResults = new List<RecipeResultDto>
            //{
            //    new RecipeResultDto { Id = 101, Title = $"Result 1 for '{query}'", Difficulty = "Easy", CookingTimeMinutes = 30, Cuisine = "Italian", DietaryRestrictions = new List<string>{"Vegetarian"}, ImageUrl = "mock-url/101.jpg" },
            //    new RecipeResultDto { Id = 102, Title = $"Result 2 for '{query}'", Difficulty = "Medium", CookingTimeMinutes = 45, Cuisine = "Asian", DietaryRestrictions = new List<string>{"Gluten Free"}, ImageUrl = "mock-url/102.jpg" },
            //};
            //return Ok(mockResults);
            ////return Ok($"You searched for: {query}");
            
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter cannot be empty.");
            }

            var response = await _elasticsearchClient.SearchAsync<Recipe>(s => s
                .Indices("recipes")
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(query)
                        .Fields(new[]
                        {
                            "title",
                            "cookingTimeMinutes",
                            "difficulty", 
                            "dietaryRestrictions", 
                            "cuisine", 
                            "ingredients",
                            "instructions"
                        })
                    )
                )
            );


            if (!response.IsValidResponse)
            {
                return StatusCode(500, "Error occurred while searching for recipes.");
            }

            var results = response.Documents;

            return Ok(results);
        }


        /// <summary>
        /// Get the details of a specific recipe by its ID.
        /// </summary>
        /// <param name="id">Recipe's ID</param>
        /// <returns>Details for recipe</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            //if (id == 0) 
            //{
            //    return NotFound($"Recipe with ID {id} not found.");
            //}

            //var mockDetail = new RecipeDetailsDTO
            //{
            //    Id = id,
            //    Title = $"Full Details for Recipe #{id}",
            //    Difficulty = "Medium",
            //    CookingTimeMinutes = 55,
            //    Cuisine = "French",
            //    DietaryRestrictions = new List<string> { "Vegan", "Nut Free" },
            //    Ingredients = new List<string> { "Ingredient A", "Ingredient B" },
            //    Instructions = "Step 1: Prep. Step 2: Cook.",
            //    ImageUrl = $"mock-url/{id}-full.jpg"
            //};

            //return Ok(mockDetail); 
            ////return Ok($"Recipe details for ID: {id}");
            

            var response = await _elasticsearchClient.SearchAsync<Recipe>(s => s
                .Indices("recipes")
                .Query(q=>q
                    .Term(t=>t.Field(f=>f.Id).Value(id))
                )
            );

            var recipe=response.Documents.FirstOrDefault();

            if (recipe==null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return Ok(recipe);
        }
    }
}
