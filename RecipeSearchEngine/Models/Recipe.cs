using System.Text.Json.Serialization;

namespace RecipeSearchEngine.Models
{
    public class Recipe
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("cookingTimeMinutes")]
        public int CookingTimeMinutes { get; set; }

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("dietaryRestrictions")]
        public List<string> DietaryRestrictions { get; set; }

        [JsonPropertyName("cuisine")]
        public string Cuisine { get; set; }

        [JsonPropertyName("ingredients")]
        public List<string> Ingredients { get; set; }

        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
