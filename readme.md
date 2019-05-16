# Test

For the first part of the test (C#) yo will need to:
  - Set the database connection string in the file DAL/DbContext.cs
  - Create the data base and run the scripts that you can find in the scripts folder (CreateTable.sql, PopulateTable.sql)
  - Run the solution and you will be able to use the REST Api

# Api endpoints!

  - Currency -> https://localhost:5001/api/cotizacion/{Currency}
  - List users -> (GET) https://localhost:5001/api/user.
  - Get specific user -> (GET) https://localhost:5001/api/user/{userId}.
  - Delete specific user -> (DELETE) https://localhost:5001/api/user/{userId}.
  - Update specific user-> (PUT) https://localhost:5001/api/user/{userId}, body must be as follow:
    ```sh
    {
        "nombre": "Pedro",
        "apellido": "Perez",
        "email": "a123@aaa.com",
        "password": "123456"
    }
    ```
  - Create user -> (POST) https://localhost:5001/api/user  body must be as follow:
    ```sh
    {
        "nombre": "Pedro",
        "apellido": "Perez",
        "email": "a123@aaa.com",
        "password": "123456"
    }
    ```

For the second part of the test (Javascript) yo will need to:
  - Open the Index.html file in some browser, all the javascript code, styles and html are included there in order to make it easier the test.
