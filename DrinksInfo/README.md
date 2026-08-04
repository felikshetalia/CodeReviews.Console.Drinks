# Drinks Info

Console-based application for browsing drink categories, viewing drinks within a selected category, and displaying detailed drink recipes from TheCocktailDB API.

Developed using C#/.NET 9, HttpClient, System.Text.Json, and Spectre.Console.

## Given Requirements

- [x] The application retrieves drink information from a public external API.
- [x] API requests are made asynchronously using .NET's HttpClient.
- [x] Users can view the available drink categories.
- [x] Users can enter a category and view all drinks assigned to it.
- [x] Users can select a drink by its ID and view its full details.
- [x] Drink details include the category, alcoholic status, glass, ingredients, measurements, and instructions.
- [x] Empty or unavailable drink properties are not displayed.
- [x] Invalid category names and drink IDs are validated before further API requests are made.
- [x] HTTP errors, connection failures, timeouts, and unexpected API responses are handled without crashing the application.
- [x] The application is separated into models, DTOs, views, controllers, services, and repositories.
- [x] SQL is not required.

## Features

- Drink category browsing
  - Retrieve and display all available drink categories from TheCocktailDB.

- Drinks by category
  - Enter a category name and view the IDs and names of the drinks assigned to it.

- Detailed drink information
  - Select a drink by ID to view its category, alcoholic status, recommended glass, ingredients, measurements, and preparation instructions.

- Favourite drinks
  - Add or remove a drink from favourites directly from its details screen.
  - View all saved favourites from the main menu.
  - Duplicate favourites are prevented.

- JSON persistence
  - Favourite drinks are stored locally in `Data/favourites.json` so they remain available after restarting the application.
  - Data is written to a temporary file before replacing the existing favourites file.

- Input validation
  - Category input is checked against the categories returned by the API.
  - Drink IDs are checked against the currently displayed drinks.

- Asynchronous loading
  - API requests run asynchronously and display Spectre.Console loading spinners.

- Error handling
  - The application displays user-friendly messages for HTTP errors, connection failures, request timeouts, missing results, and unexpected JSON responses.

- Clean console UI
  - Tables, prompts, menus, status indicators, and messages are displayed using Spectre.Console.

- Configurable API connection
  - The API base URL and key are loaded from `Configuration/appsettings.json`.

## What I've Learned

- Sending asynchronous GET requests with HttpClient.
- Loading application settings from an `appsettings.json` file.
- Deserializing external JSON responses with System.Text.Json.
- Using DTOs and `JsonPropertyName` attributes to map API field names to C# properties.
- Mapping API DTOs into cleaner application models.
- Converting numbered ingredient and measurement properties into a collection of ingredient objects.
- Formatting and encoding query parameters for API requests.
- Validating user input against data returned by the API.
- Handling HTTP, timeout, and JSON deserialization errors.
- Separating responsibilities between controllers, views, services, repositories, models, and DTOs.
- Persisting local application data in a JSON file and preventing duplicate records.
- Building a polished terminal interface with Spectre.Console.

## How to Run

1. Install the .NET 9 SDK.
2. Clone the repository.
3. Open a terminal in the repository and enter the project directory:
   - `cd DrinksInfo`
4. Restore the project dependencies:
   - `dotnet restore`
5. Run the application:
   - `dotnet run`

An internet connection is required to retrieve drink data from TheCocktailDB. The `Data` directory and `favourites.json` file are created automatically when the first favourite is saved.
