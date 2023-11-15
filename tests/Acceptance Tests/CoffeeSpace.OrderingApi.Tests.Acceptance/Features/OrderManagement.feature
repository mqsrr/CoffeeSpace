Feature: OrderManagement

    Scenario: Creating a new order
        Given existing buyer with the following details:
          | Email              | Name |
          | ordering@gmail.com | Jack |
        When a POST request is sent to create a new order
        Then the order is created successfully
        And the response status code should be 201

    Scenario: Retrieving an existing order
        Given existing buyer with the following details:
          | Email              | Name |
          | ordering@gmail.com | Jack |
        And existing orders in the system
        When a GET request is sent to retrieve order
        Then the response status code should be 200
        And the order response should be with correct properties   
        
    Scenario: Retrieving an all orders from buyer
        Given existing buyer with the following details:
          | Email              | Name |
          | ordering@gmail.com | Jack |
        And existing orders in the system
        When a GET request is sent to retrieve orders by buyer ID
        Then the response status code should be 200
        And the orders response should be with correct properties    
        
    Scenario: Deleting the buyer's order
        Given existing buyer with the following details:
          | Email                | Name |
          | ordering@gmail.com | Jack |
        And existing orders in the system
        When a DELETE request is sent to DELETE order by buyer ID and order ID
        Then the response status code should be 204
        And the order will be deleted