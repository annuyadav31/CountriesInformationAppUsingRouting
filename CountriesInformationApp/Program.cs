var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); //will show static files in wwwroot folder

app.UseRouting();

//Input Data
Dictionary<int,string> countries = new Dictionary<int,string>()
{
    {1,"United Kingdom"},
    {2,"Canada"},
    {3,"United States"},
    {4,"Japan"},
    {5,"India"}
};

//Define Endpoint
app.UseEndpoints(endpoints =>
{
    //when endpoint is /countries
    endpoints.MapGet("/countries", async context => {
        //write country details to response
        foreach (KeyValuePair<int, string> country in countries)
        {
            await context.Response.WriteAsync($"{country.Key},{country.Value}\n");
        }
    });

    //when endpoint is /countries/{countryId}
    endpoints.MapGet("/countries/{countryId:int:range(1,100)}", async context => {

        //check if countryId is submitted or not
        if (!context.Request.RouteValues.ContainsKey("countryId"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("The CountryID should be between 1 and 100");
        }
        else
        {
            int countryId = Convert.ToInt32(context.Request.RouteValues["countryId"]);

            //if countryId exists in dictionary
            if (countries.ContainsKey(countryId))
            {
                var country = countries[countryId];
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync($"{country}");
            }
            else
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"[No country]");
            }
        }
    });

     // When request path is "countries/{countryID}"
     endpoints.MapGet("/countries/{countryID:min(101)}", async context =>
     {
     context.Response.StatusCode = 400;
     await context.Response.WriteAsync("The CountryID should be between 1 and 100 - min");
     });

});

//Default middleware
app.Run(async context =>
{
    await context.Response.WriteAsync("No response");
});

app.Run();
