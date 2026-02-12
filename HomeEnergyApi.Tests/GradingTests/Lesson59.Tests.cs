using System.Reflection;
using HomeEnergyApi.Models;

public class LessonTests
{
    private readonly string[] _requiredTestMethods =
    {
        "ShouldDeleteHome_WhenGivenValidHomeId",
        "ShouldNotDeleteHome_WhenGivenInvalidHomeId"
    };

    [Fact]
    public void AuthenticationV1ControllerTestExists()
    {
        var testAssembly = Assembly.GetExecutingAssembly();
        var authenticationTestClass = testAssembly.GetTypes()
            .FirstOrDefault(t => t.Name == "HomeAdminControllerTest");

        Assert.NotNull(authenticationTestClass);
    }

    [Fact]
    public void AuthenticationV1ControllerTestHasAllRequiredMethods()
    {
        var testAssembly = Assembly.GetExecutingAssembly();
        var authenticationTestClass = testAssembly.GetTypes()
            .FirstOrDefault(t => t.Name == "HomeAdminControllerTest");

        Assert.NotNull(authenticationTestClass);

        foreach (var requiredMethodName in _requiredTestMethods)
        {
            var testMethod = authenticationTestClass.GetMethod(requiredMethodName);
            Assert.True(testMethod != null, $"Method {requiredMethodName} not found in HomeAdminControllerTest class");
        }
    }

    [Fact]
    public void AuthenticationV1ControllerTestMethodsHaveFactAttribute()
    {
        var testAssembly = Assembly.GetExecutingAssembly();
        var authenticationTestClass = testAssembly.GetTypes()
            .FirstOrDefault(t => t.Name == "HomeAdminControllerTest");

        Assert.NotNull(authenticationTestClass);

        foreach (var requiredMethodName in _requiredTestMethods)
        {
            var testMethod = authenticationTestClass.GetMethod(requiredMethodName);
            Assert.NotNull(testMethod);

            var factAttribute = testMethod.GetCustomAttribute<FactAttribute>();
            Assert.True(factAttribute != null, $"Method {requiredMethodName} should have [Fact] attribute");
        }
    }

    // [Fact]
    // public void AllStudentTestsPass()
    // {
    //     var testAssembly = Assembly.GetExecutingAssembly();
    //     var authenticationTestClass = testAssembly.GetTypes()
    //         .FirstOrDefault(t => t.Name == "AuthenticationV1ControllerTest");

    //     Assert.NotNull(authenticationTestClass);

    //     var instance = Activator.CreateInstance(authenticationTestClass);

    //     // Call InitializeAsync if the class implements IAsyncLifetime
    //     var initializeMethod = authenticationTestClass.GetMethod("InitializeAsync");
    //     if (initializeMethod != null)
    //     {
    //         var initResult = initializeMethod.Invoke(instance, null);
    //         if (initResult is Task initTask)
    //         {
    //             initTask.GetAwaiter().GetResult();
    //         }
    //     }

    //     foreach (var requiredMethodName in _requiredTestMethods)
    //     {
    //         var testMethod = authenticationTestClass.GetMethod(requiredMethodName);
    //         Assert.True(testMethod != null, $"Method {requiredMethodName} not found in AuthenticationV1ControllerTest class");

    //         try
    //         {
    //             var result = testMethod.Invoke(instance, null);
    //             if (result is Task task)
    //             {
    //                 task.GetAwaiter().GetResult();
    //             }
    //         }
    //         catch (TargetInvocationException ex)
    //         {
    //             Assert.Fail($"Test {requiredMethodName} failed: {ex.InnerException?.Message ?? ex.Message}");
    //         }
    //     }
    // }
}
