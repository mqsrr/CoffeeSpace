Feature: BuyersManagement

    Scenario: Creating a new buyer
        When a POST request is sent to create a new buyer
          | Name | Email             |
          | test | testing@gmail.com |
        Then the buyer is created successfully
        And the response status code should be 201

    Scenario: Retrieving buyer By Id
        Given existing buyer with the following details:
          | Email                | Name |
          | helloThere@gmail.com | Jack |
        When a GET request is sent to retrieve buyer by ID
        Then the response status code should be 200
        And the buyer response should be with correct properties

    Scenario: Retrieving buyer By Email
        Given existing buyer with the following details:
          | Email                | Name |
          | helloThere@gmail.com | Jack |
        When a GET request is sent to retrieve buyer by Email
        Then the response status code should be 200
        And the buyer response should be with correct properties

    Scenario: Updating existing buyer
        Given existing buyer with the following details:
          | Email                | Name |
          | helloThere@gmail.com | Jack |
        When PUT request is sent to update the buyer with updated buyer's data:
          | Email              | Name |
          | newEmail@gmail.com | Nick |
        Then the response status code should be 200
        And a GET request to retrieve the updated buyer should return the correct updated buyer's information
        
    Scenario: Deleting buyer
        Given existing buyer with the following details:
          | Email                | Name |
          | helloThere@gmail.com | Jack |
        When DELETE request is sent to delete buyer By ID:
        Then the response status code should be 204
        And the buyer should be deleted