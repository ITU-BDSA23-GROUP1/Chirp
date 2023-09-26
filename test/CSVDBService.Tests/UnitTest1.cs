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


        //Assert.IsType<List<Cheep>>(response.Content.);
        //Assert.IsType<JsonContent>(response.Content);
        //Assert.IsType<List<Cheep>>(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));


        Assert.Equal(HttpStatusCode.OK,  response.StatusCode);



    }
}