var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

// data bilgilerinin ba� harfini b�y�k yapmam�za ra�men k���k g�r�n�yor bunu d�zeltmek i�in;
builder.Services.AddControllers().AddJsonOptions(options  => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

builder.Services.AddSwaggerGen();

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
