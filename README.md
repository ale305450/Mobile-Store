# Mobile Store Website

This is a mobile store website that sells phones and smartwatches. It is built using the Three-Tier architecture pattern with ASP.NET Core MVC and utilizes Microsoft Identity for user authentication. The website allows users to view and purchase mobile devices and watches, while administrators have additional privileges to add new products and manage companies.

## Features

- Users can view and purchase mobile phones and smartwatches.
- Admins can add, edit, and delete products.
- Admins can manage the companies that produce the products.
- User authentication and authorization using Microsoft Identity.
- Three-Tier architecture for better separation of concerns and maintainability.

## Technologies Used

- ASP.NET Core MVC
- Microsoft Identity
- Entity Framework Core (for data access)
- HTML/CSS/JavaScript (for frontend)
- SQL Server (for database)


## Architecture Overview
The Three-Tier architecture separates the application into three distinct layers: Presentation Layer, Business Logic Layer, and Data Access Layer. Microsoft Identity is used for user authentication and authorization.

1. **Presentation Layer:** This layer is responsible for handling the user interface and user interactions. In this project, the presentation layer is implemented using ASP.NET Core MVC. It consists of controllers, views, and client-side scripts. Users can view and purchase mobile devices and watches through the website's user interface.

2. **Business Logic Layer:** This layer contains the business logic of the application. It handles the processing and manipulation of data and enforces the application's rules and logic. It interacts with the data access layer to retrieve and store data.

3. **Data Access Layer:** This layer is responsible for accessing the  database

### Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/your/repository.git
   ```

2. Open the solution in Visual Studio.

3. Open the Package Manager Console:
   - Go to `Tools` > `NuGet Package Manager` > `Package Manager Console`.

4. Update the database:
   - Run Entity Framework Core migrations to create the database schema:
     ```
      Update-Database
     ```

5. Run the application:
   - Press `F5` or click on the green "Play" button in Visual Studio.

6. Use the website:
   - Browse products and make purchases as a user.
   - Log in as an admin to manage products and companies.

## Contributing
If you would like to contribute to this project, please follow these steps:

1. Fork the repository and clone it to your local machine.

2. Create a new branch for your feature or bug fix.

3. Make the necessary changes and commit them.

4. Push your changes to your forked repository.

5. Submit a pull request detailing your changes and explaining the purpose of the pull request.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

Special thanks to the ASP.NET Core team and the open-source community for providing excellent tools and resources for web development.
