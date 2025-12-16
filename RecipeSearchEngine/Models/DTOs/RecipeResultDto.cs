namespace RecipeSearchEngine.Models.DTOs
{
    public class RecipeResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CookingTimeMinutes { get; set; }
        public string Difficulty { get; set; }
        public List<string> DietaryRestrictions { get; set; }
        public string Cuisine { get; set; }
        //public List<string> Ingredients { get; set; }
        //public string Instructions { get; set; }
        public string ImageUrl { get; set; }
    }
}
