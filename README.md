# 059 Test Driven Development
## Lecture

[![# Test Driven Development](https://img.youtube.com/vi/But_M5k9GvY/0.jpg)](https://www.youtube.com/watch?v=But_M5k9GvY)

## Instructions

In this lesson you will be testing the method contained in `HomeEnergyApi/Services/RateLimitingService.cs` where a new method `IsWeekend()` has been added. You will need to create test stubs to test this method, in addition to adding tests to `HomeEnergyApi.Tests/Lesson54Tests/RateLimitingService.Tests.cs`. You should NOT change any test files inside of the `HomeEnergyApi.Tests/GradingTests`, these are used to grade your assignment.

- In `HomeEnergyApi.Tests/Lesson59Tests/Controllers/HomeAdminControllerTest.cs`
    - Create a new public async method `ShouldDeleteHome_WhenGivenValidHomeId()` returning a `Task`
        - Create a new variable for the initial response, holding the result of awaiting `_client.PostAsJsonAsync` with `"/admin/Homes"` and `_homeDto` given as arguments
        - Assert that the `StatusCode` property on the newly created variable holding the initial response is equal to `HttpStatusCode.Created`
        - Create a new variable for the created home, holding the result of awaiting the result of calling `ReadFromJsonAsync<Home>()` on the `Content` property on your newly created initial response variable
        - Assert that the newly created home variable is not null
        - Create a second response variable, holding the result of calling `_client.DeleteAsync()` with `"/admin/Homes/$HOME_ID"` given as an argument
            - `$HOME_ID` in that argument should be equal to the `Id` property on your newly created home variable
        - Assert that the `StatusCode` property on the second response variable is equal to `HttpStatusCode.Created`
        - Create a deleted home variable, holding the result of calling `ReadFromJsonAsync<Home>()` on the `Content` property on your second response variable
        - Assert that the deleted home variable is not null
        - Assert that the `Id` property on the deleted home variable is equal to the `Id` property on your created home variable

- In `HomeEnergyApi/Controllers/HomeAdminController.cs`
    - Create a new public method `Delete()` returning an `IActionResult` and taking one argument of type `int`
        - The method should have an `HttpDelete` attribute with `"{id}"` as the given value
        - Create a variable for the deleted home, holding the result of calling `RemoveById()` on `repository` and passing the `int` argument
        - Return `Ok()` passing your newly created deleted home variable as an argument

- In `HomeEnergyApi.Tests/Lesson59Tests/Controllers/HomeAdminControllerTest.cs`
    - Create a new public async method `ShouldNotDeleteHome_WhenGivenInvalidHomeId()` returning a `Task`
        - Create a new variable for the initial response, holding the result of awaiting `_client.PostAsJsonAsync` with `"/admin/Homes"` and `_homeDto` given as arguments
        - Assert that the `StatusCode` property on the newly created variable holding the initial response is equal to `HttpStatusCode.Created`
        - Create a new variable for the created home, holding the result of awaiting the result of calling `ReadFromJsonAsync<Home>()` on the `Content` property on your newly created initial response variable
        - Assert that the newly created home variable is not null
        - Create a variable representing an invalid `Id` property for a home, by adding a large number like 100 to your newly created home variable's `Id` property
        - Create a second response variable, holding the result of calling `_client.DeleteAsync()` with `"/admin/Homes/$INVALID_HOME_ID"` given as an argument
            - `$INVALID_HOME_ID` in that argument should be equal to the `Id` property on your newly created home variable with a large number like 100 added to it
        - Assert that the `StatusCode` property on the second response variable is equal to `HttpStatusCode.NotFound`

- In `HomeEnergyApi/Controllers/HomeAdminController.cs`
    - Wrap the existing code you created in a try block
    - If an `Exception` occurs, catch it by returning `NotFound()` instead

## Additional Information

- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:
- Develop and use a series of test cases to verify that a program performs according to its design specifications (3B-AP-21) https://www.csteachers.org/page/standards

## Resources
- https://en.wikipedia.org/wiki/Unit_testing
- https://martinfowler.com/bliki/TestPyramid.html
- https://xunit.net/
- https://en.wikipedia.org/wiki/Test-driven_development

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
