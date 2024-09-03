# UserManager

## Architecture and design decesions.

- A `3 layer` architecture is used, of which first is a presentation layer using Web API, second is an application layer and third is a data access layer.
- Design patterns like `repository pattern` and `dependancy injection` are used to reduce the coupling of each layers. Interfaces are used to ensure loose coupling between the diffrent layers.
- `Entity Framework Core` is the ORM framework used to integrate with a SQL Server database.
- Two tables `Users` and `UserDetails` are used. The `Users` table remains lean, focusing on essential login/authentication details, while the `UserProfile` table can handle expandable fields(scalable).
- Implemented `JWT Bearer Authentication` in the application using the Bearer authentication scheme. This setup ensures secure, stateless authentication by validating JSON Web Tokens (JWTs) for API requests, enhancing security.
- To enhance security, a password is hashed along with a unique salt value.

## Additional features

- A `GlobalErrorHandler` controller is created, which handles all unhandled exceptions logs the error(console), and returns the response.
- A string extension `SanitizeAndTrim` has been added to sanitize and trim the data before adding to databse. This sanitization removes the html tags, which can cause an `XSS` attack.

## Endpoints

### Register User

- **Route:** `api/auth/register`
- **Method:** `POST`
- **Description:** Registers a new user by creating a user in the system.
- **Request Body:**
  ```json
  {
    "username": "string",
    "password": "string",
    "email": "string"
  }
  ```
- **Response:** 201 Created

### Authenticate User

- **Route:** `api/auth/login`
- **Method:** `POST`
- **Description:** Authenticates a user and returns a JWT token if successful.
- **Request Body:**
  ```json
  {
    "usernameOrEmail": "string",
    "password": "string"
  }
  ```
- **Response:** 200 Ok
  ```json
  {
    "token": "token"
  }
  ```

### Retrieve User Profile

- **Route:** `api/userprofile`
- **Method:** `GET`
- **Headers:**
  - **Authorization:** Bearer `<JWT token>`
- **Description:** Retrieves the profile details of the currently authenticated user.
- **Response:** 200 Ok
  ```json
  {
    "fullName": "string",
    "firstName": "string",
    "lastName": "string",
    "phone": "string",
    "address": "string",
    "dateOfBirth": "2018-02-17"
  }
  ```

### Update User Profile

- **Route:** `api/userprofile`
- **Method:** `PUT`
- **Headers:**
  - **Authorization:** Bearer `<JWT token>`
- **Description:** Updates the profile details of the currently authenticated user.
- **Request Body:**
  ```json
  {
    "firstName": "string",
    "lastName": "string",
    "phone": "string",
    "address": "string",
    "dateOfBirth": "2018-02-17"
  }
  ```
- **Response:** 200 Ok

## Enhancements

- In .NET 8, you can use IdentityDbContext and Identity endpoints, which provide built-in APIs for authentication and authorization.
- To enhance logging, consider integrating external logging services.

## Running the Application using command line

- To run the project, ensure you .NET Core SDK installed on your machine.
- Clone the repository.
- Navigate to the project root directory.
- Configure the database connection string under the section `ConnectionStrings` in `appsettings.json`
- The following items are also made configurable, the values can be chaged in `appsettings.json`

```js
  "JwtConfigurations": {
    "Secret": "secret",
    "TokenExpiryInMins": 5,
    "Issuer": "issuer"
  }
```

- Restore the dependencies using `dotnet restore`.
- Run the ef migrations by running the command `dotnet ef database update --startup-project src/UserManager.API --project src/UserManager.Repo`.
- Run the application using the command `dotnet run --project src/UserManager.API --urls=http://localhost:5001/`.
- The app will be available at `http://localhost:5001`.
- Open `http://localhost:5001/swagger/index.html` to view the swagger API documentation.
