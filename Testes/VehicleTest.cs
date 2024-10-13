using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using API.Shared;

namespace Testes;
public class VehicleTest: IDisposable
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    //_
    public VehicleTest(ITestOutputHelper output)
    {
        _client = new HttpClient() {BaseAddress = new Uri("http://localhost:5229")};
        _output = output;
    }

    [Fact]
    public async Task Test_Vehicle_Delete_Must_Work()
    {
        object payload = new  {
            make = "toyota",
            name = "x",
            year = 2022
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );
  
        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);

        response.EnsureSuccessStatusCode();
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        string result = await response.Content.ReadAsStringAsync();
        int vehicleId;

        Assert.True(int.TryParse(result, out vehicleId));
        Assert.True(vehicleId > 0);

        using HttpResponseMessage responseDelete = await _client.DeleteAsync(
        $"vehicle/{vehicleId}");

        string resultDelete = await responseDelete.Content.ReadAsStringAsync();
        _output.WriteLine(resultDelete);

        responseDelete.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, responseDelete.StatusCode);
    }

    [Fact]
    public async Task Test_Vehicle_Delete_Shouldnt_Work()
    {
        using HttpResponseMessage response = await _client.DeleteAsync("vehicle/-1");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Vehicle_Create_Must_Work()
    {
        object payload = new  {
            make = "toyota",
            name = "x",
            year = 2022
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);

        response.EnsureSuccessStatusCode();
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test_Vehicle_Create_Shouldnt_Work()
    {
        object payload = new  {
            make = "",
            name = "",
            year = -1
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);
        
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task Test_Vehicle_Update_Must_Work()
    {
        object payload = new  {
            make = "tesla",
            name = "y",
            year = 2018
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        string result = await response.Content.ReadAsStringAsync();
        int id;

        Assert.True(int.TryParse(result, out id));
        
        payload = new  {
            make = "volkswagen",
            name = "z",
            year = 2015
        };

        using StringContent jsonContentUpd = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage responseUpd = await _client.PutAsync(
        $"vehicle/{id}",
        jsonContentUpd);

        responseUpd.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, responseUpd.StatusCode);

        result = await responseUpd.Content.ReadAsStringAsync();

        VehicleDto? v = JsonSerializer.Deserialize<VehicleDto>(result);

        Assert.NotNull(v);

        Assert.True(v.Make == "volkswagen" && v.Name == "z" && v.Year == 2015);
    }

    [Fact]
    public async Task Test_Vehicle_Update_Shouldnt_Work()
    {
        object payload = new  {
            make = "tesla",
            name = "y",
            year = 2018
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        string result = await response.Content.ReadAsStringAsync();
        int id;

        Assert.True(int.TryParse(result, out id));
        
        object updatedPayload = new  {
            make = "",
            name = "",
            year = -1
        };

        using StringContent jsonContentUpd = new (
            JsonSerializer.Serialize(updatedPayload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage responseUpd = await _client.PutAsync(
        $"vehicle/{id}",
        jsonContentUpd);

        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, responseUpd.StatusCode);
    }

    [Fact]
    public async Task Test_Vehicle_Get_Must_Work()
    {
        object payload = new  {
            make = "tesla",
            name = "y",
            year = 2018
        };

        using StringContent jsonContent = new (
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await _client.PostAsync(
        "vehicle",
        jsonContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        string result = await response.Content.ReadAsStringAsync();
        int id;

        Assert.True(int.TryParse(result, out id));

        using HttpResponseMessage responseGet = await _client.GetAsync($"vehicle/{id}");

        responseGet.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, responseGet.StatusCode);

        result = await responseGet.Content.ReadAsStringAsync();

        VehicleDto? v = JsonSerializer.Deserialize<VehicleDto>(result);

        Assert.NotNull(v);
        Assert.True(v.Make == "tesla" && v.Name == "y" && v.Year == 2018);
    }

    [Fact]
    public async Task Test_Vehicle_Get_All_Must_Work()
    {
         using HttpResponseMessage response = await _client.GetAsync($"vehicle/all");

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        string result = await response.Content.ReadAsStringAsync();

        List<VehicleDto>? vs = JsonSerializer.Deserialize<List<VehicleDto>>(result);

        Assert.NotNull(vs);
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}