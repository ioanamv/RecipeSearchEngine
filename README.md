# Recipe Search Engine

## Milestone 1: Project Description
This is a recipe search web application built with **Elasticsearch** and **ASP.NET MVC** designed to provide fast and intuitive access to a collection of cooking recipes using a single search bar.

Users can search recipes by title, ingredients, instructions, cuisine, dietary restrictions, difficulty and cooking time. All these fields are indexed in Elasticsearch, allowing users to find relevant recipes without relying on separate filters.

The application targets users looking for cooking inspiration, recipes based on available ingredients or meals that meet specific dietary preferences. The system is designed to be simple, accessible and easy to use.

## Milestone 2: List of Use Cases
**Main Use Cases:**
- **Search recipes by keyword:** Users enter one or more keywords and the system searches across multiple recipe fields, returning relevant results.
- **View recipe details:** Users select a recipe from the results list to view full information, including ingredients and instructions.
- **Handle multi-keyword searches:** The system supports searches containing multiple terms that may match different recipe attributes.
- **API access for external clients:** Developers can use REST API endpoints to search for recipes and retrieve recipe details in JSON format.

## Milestone 3: Rest API – SWAGGER
The application’s core functionality is exposed through a REST API and documented using **Swagger (OpenAPI)**. Swagger automatically generates an interactive interface that allows all available endpoints to be explored, tested, and validated directly from the browser.

**Implemented endpoints:**

- **`GET /api/recipes`**
Allows searching for recipes using a keyword provided as a query parameter. The search is performed across multiple recipe fields, and Swagger displays the list of matching recipes returned in JSON format.

- **`GET /api/recipes/{id}`**
Retrieves full details for a specific recipe based on its unique identifier. Swagger documents the required path parameter and the structure of the detailed response.

By integrating Swagger, the REST API becomes self-documented, easy to test without external tools, and straightforward to extend or maintain as the project evolves.

## Milestone 4: Elastic Mapping
To configure Elasticsearch for the application, Postman was used to interact directly with the Elasticsearch REST API.
First, the recipes index was created using a PUT request, with authentication enabled. A custom JSON mapping was defined to specify how each recipe field is stored and indexed. All searchable fields (such as title, ingredients, cuisine, difficulty, and dietary restrictions) were configured as text fields to support flexible, full-text search, while the recipe ID was stored as an integer. 

After creating the index, the mapping was verified using the _mapping endpoint to ensure that all fields were correctly defined and ready for indexing.

Next, multiple recipe documents were inserted into the index using Elasticsearch’s Bulk API, allowing efficient addition of a larger dataset in a single request.

Finally, the data was validated using the _search endpoint to confirm that the recipes were successfully indexed and could be retrieved through search queries.

## Milestone 5: Implementation
The application is implemented using **ASP.NET Core MVC** as the main backend framework and **Elasticsearch** as the search engine. The solution follows a clean separation of concerns between data access, business logic and presentation.

Elasticsearch is integrated into the application using the official **Elastic .NET client**. This allows the backend to search recipes and retrieve data from the index.

The user interface is built using **Razor Views** and styled with **Bootstrap**, providing a simple and responsive layout. The main page contains a single search bar where users can enter free-text queries. These queries are sent to Elasticsearch, which searches across multiple recipe fields such as title, ingredients, instructions, cuisine, difficulty and dietary restrictions.

Search results are displayed as cards containing key recipe information and images. When a user selects a recipe, they are redirected to a dedicated details page that shows the complete recipe data, including ingredients, cooking instructions and cooking time.

## Milestone 6: Postman Testing
Postman was used to manually test and validate the REST API endpoints exposed by the application. This ensured that the API behaves correctly and returns the expected data from Elasticsearch.

The following types of requests were tested:

**1. Search Recipes**

`GET https://localhost:7055/api/Recipes?query=garlic`

  This request verifies that the search endpoint correctly returns a list of recipes matching the provided keyword across multiple fields.

**2. View Recipe Details**

`GET https://localhost:7055/api/Recipes/3`

This request retrieves the full details of a specific recipe based on its ID.

Successful responses confirmed that Elasticsearch is properly connected, the data is indexed correctly, and the API endpoints function as expected.

