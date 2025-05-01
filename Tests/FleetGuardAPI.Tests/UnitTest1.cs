namespace FleetGuardAPI.Tests;

public class UnitTest1
{

    [Fact]
    public async Task SendVehicleRequest20Times()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:44390/");

        for (int i = 1; i <= 20; i++)
        {
            var response = await httpClient.GetAsync("api/v1/vehicle/5YJSA1DP7DFP14705");
            Assert.True(response.IsSuccessStatusCode, $"Request {i} failed");
        }
    }
}
