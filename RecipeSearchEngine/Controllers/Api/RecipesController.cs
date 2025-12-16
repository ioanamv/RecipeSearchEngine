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
