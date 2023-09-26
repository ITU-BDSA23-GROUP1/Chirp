using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace CSVDBService.Tests;

public class UnitTest1
{
    [Fact]
    public async Task GetCheepsTest()
    {
        //Arrange
        var baseURL = "http://localhost:5000";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        //Act
        var response = await client.GetAsync("/cheeps");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Test if the status code is 200
        Assert.IsType<List<Cheep>>(await response.Content.ReadFromJsonAsync<IEnumerable<Cheep>>()); // Test if the response body is of type List<Cheep> (when deserialized from JSON)
    }


    [Fact]
    public async Task PostCheepTest()
    {
        //Arrange
        var baseURL = "http://localhost:5000";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        //Act
        Cheep testCheep = new Cheep {Author = "Username", Message = "\"TestMsg\"", Timestamp = 1694349000 };
        JsonContent content = JsonContent.Create(testCheep);
        var response = await client.PostAsync("/cheep", content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Test if the status code is 200
    }

}