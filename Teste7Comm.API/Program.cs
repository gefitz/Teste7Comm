using Teste7Comm.API.Data;
using Teste7Comm.API.DTO.Mapper;
using Teste7Comm.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Command>( opc => new Command(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<PessoaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
