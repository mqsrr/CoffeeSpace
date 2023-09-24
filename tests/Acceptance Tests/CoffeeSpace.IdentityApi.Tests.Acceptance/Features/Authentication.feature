Feature: Authentication
    
    Scenario: User login
        Given user with following credentials:
        | Name | LastName | Username | Email             | Password  |
        | Jack | Jackson  | Testing  | testing@gmail.com | Pass1234! |
        When the POST request is sent with the following credentials:
        | Username | Password  |
        | Testing  | Pass1234! |
        Then the response status code should be 200
        And the response should contain JWT token    
        
    Scenario: User register
        When POST request is sent with following credentials:
        | Name | LastName | Username | Email             | Password  |
        | Jack | Jackson  | register  | registerme@gmail.com | Pass1234! |
        Then the response status code should be 200
        And the response should contain JWT token