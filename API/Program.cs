using API.Extensions;
using API.Services.Contracts;
using API.Shared;
using Microsoft.AspNetCore.Mvc;
using API.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

#region vehicle
app.MapPost("/vehicle", async ([FromBody] VehicleForCreateDto vehicle, IVehicleService service) => {
    var validation = new ValidationModel();

    if(string.IsNullOrEmpty(vehicle.Make)) 
        validation.Messages.Add("vehicle make is null or empty");

    if(vehicle.Make.Length > 100) 
        validation.Messages.Add("vehicle make must have up to 100 characters");

    if(string.IsNullOrEmpty(vehicle.Name))
        validation.Messages.Add("vehicle name is null or empty");

    if(vehicle.Name.Length > 150) 
        validation.Messages.Add("vehicle name must have up to 100 characters");

    if(vehicle.Year <= 0)
        validation.Messages.Add("vehicle year is not positive");

    if(validation.Messages.Any())
        return Results.UnprocessableEntity(validation);

    int id = await service.CreateVehicle(vehicle);
    return Results.Ok(id);
});

app.MapGet("/vehicle/{id}", async ([FromRoute] int id, IVehicleService service) => {
    var vehicle = await service.GetVehicle(id);
    
    if(vehicle is null)
        return Results.NotFound();

    return Results.Ok(vehicle);
});

app.MapPut("/vehicle/{id}", async ([FromRoute] int id, [FromBody] VehicleForUpdateDto vehicle, IVehicleService service) => {
    var validation = new ValidationModel();

    if(string.IsNullOrEmpty(vehicle.Make)) 
        validation.Messages.Add("vehicle make is null or empty");

    if(vehicle.Make.Length > 100) 
        validation.Messages.Add("vehicle make must have up to 100 characters");

    if(string.IsNullOrEmpty(vehicle.Name))
        validation.Messages.Add("vehicle name is null or empty");

    if(vehicle.Name.Length > 150) 
        validation.Messages.Add("vehicle name must have up to 100 characters");

    if(vehicle.Year <= 0)
        validation.Messages.Add("vehicle year is not positive");

    if(validation.Messages.Any())
        return Results.UnprocessableEntity(validation);

    var updatedVehicle = await service.UpdateVehicle(id, vehicle);
    return Results.Ok(updatedVehicle);
});

app.MapDelete("/vehicle/{id}", async ([FromRoute] int id, IVehicleService service) => {
    await service.DeleteVehicle(id);
    return Results.NoContent();
});

app.MapGet("/vehicle/all", async (IVehicleService service) => {
    var vehicles = await service.GetAllVehicles();
    return Results.Ok(vehicles);
});
#endregion

#region admin
string GerarTokenJwt(AdminDto administrador){
    string key = builder.Configuration.GetSection("JWT").GetValue<string>("Key") ?? "xxx123";

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.UserName),
        new Claim(ClaimTypes.Role, "Adm"),
    };
    
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
app.MapPost("/admin", async ([FromBody] AdminForCreateDto admin, IAdminService service) => {
    var validation = new ValidationModel();
    
    if(string.IsNullOrEmpty(admin.Email))
        validation.Messages.Add("admin email is null or empty");

    if(admin.Email.Length > 255)
        validation.Messages.Add("admin email must have up to 255 characters");

    if(string.IsNullOrEmpty(admin.Password))
        validation.Messages.Add("admin password is null or empty");

    if(admin.Password.Length > 50)
        validation.Messages.Add("admin password must have up to 50 characters");

    if(string.IsNullOrEmpty(admin.UserName))
        validation.Messages.Add("admin password is null or empty");

    if(admin.UserName.Length > 10)
        validation.Messages.Add("admin password must have up to 10 characters");

    if(validation.Messages.Any())
        return Results.UnprocessableEntity(validation);

    int id = await service.CreateAdmin(admin);
    return Results.Ok(id);
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("adms");

app.MapPut("/admin/{id}", async ([FromRoute] int id, [FromBody] AdminForUpdateDto admin, IAdminService service) => {
    var validation = new ValidationModel();
    
    if(string.IsNullOrEmpty(admin.Email))
        validation.Messages.Add("admin email is null or empty");

    if(admin.Email.Length > 255)
        validation.Messages.Add("admin email must hava up to 255 characters");

    if(string.IsNullOrEmpty(admin.Password))
        validation.Messages.Add("admin password is null or empty");

    if(admin.Password.Length > 50)
        validation.Messages.Add("admin password must have up to 50 characters");

    if(string.IsNullOrEmpty(admin.UserName))
        validation.Messages.Add("admin password is null or empty");

    if(admin.UserName.Length > 10)
        validation.Messages.Add("admin password must have up to 10 characters");

    if(validation.Messages.Any())
        return Results.UnprocessableEntity(validation);

    var result = await service.UpdateAdmin(id, admin);
    return Results.Ok(result);
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("adms");

app.MapDelete("/admin/{id}", async ([FromRoute] int id, IAdminService service) => {
    await service.DeleteAdmin(id);
    return Results.NoContent();
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("adms");

app.MapGet("/admin/{id}", async ([FromRoute] int id, IAdminService service) => {
    var admin = await service.GetAdmin(id);

    if(admin is null)
        return Results.NotFound();

    return Results.Ok(admin);
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("adms");

app.MapGet("/admin/all", async (IAdminService service) => {
    var admins = await service.GetAllAdmins();
    return Results.Ok(admins);
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("adms");

app.MapPost("/admin/login", async ([FromBody] LoginDto login, IAdminService service) => {
    var admin = await service.LoginAdmin(login);

    if(admin is null)
        return Results.Unauthorized();

    string jwt = GerarTokenJwt(admin);
    return Results.Ok(jwt);
}).AllowAnonymous().WithTags("admins");
#endregion

app.Run();