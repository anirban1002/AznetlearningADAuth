var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Audience = "api://3a38e155-21df-4b14-b0f9-c0a9c9c48f43";
        options.Authority = "https://login.microsoftonline.com/cfde1bcf-94d1-4a7f-9288-0392d590ee5b";
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
