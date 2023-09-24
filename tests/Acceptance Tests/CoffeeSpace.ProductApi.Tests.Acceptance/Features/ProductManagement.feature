Feature: Manage products in the system

    Scenario: Creating a new product
        When the POST request is sent with following request:
          | Title     | Description       | UnitPrice | Discount | Quantity |
          | Product 1 | This is Product 1 | 10.99     | 0.2      | 50       |
        Then the product are created successfully
        And the response status code should be 201

    Scenario: Retrieving product information
        Given the following products in the system:
          | Title     | Description       | UnitPrice | Discount | Quantity |
          | Product 1 | This is Product 1 | 10.99     | 0.2      | 50       |
          | Product 2 | Another Product   | 15.49     | 0.15     | 30       |
          | Product 3 | Description 3     | 5.99      | 0.1      | 100      |
        When a GET request is sent to retrieve the product by ID
        Then the response status code should be 200
        And the response body should contain the correct product information

    Scenario: Updating product information
        Given the following products in the system:
          | Title     | Description       | UnitPrice | Discount | Quantity |
          | Product 1 | This is Product 1 | 10.99     | 0.2      | 50       |
        When a PUT request is sent to update the product with updated product data:
          | Title         | Description         | UnitPrice | Discount | Quantity |
          | Updated Title | Updated Description | 12.99     | 0.1      | 75       |
        Then the response status code should be 200
        And a GET request to retrieve the updated product should return the correct updated product information with 200 status code

    Scenario: Deleting a product
        Given the following products in the system:
          | Title     | Description       | UnitPrice | Discount | Quantity |
          | Product 1 | This is Product 1 | 10.99     | 0.2      | 50       |
        When a DELETE request is sent to delete the product by ID
        Then the response status code should be 204
        And a GET request to retrieve the deleted product should return a status code of 404 (Not Found)
