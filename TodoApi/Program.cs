using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת Swagger כדי לייצר תיעוד אוטומטי של ה-API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // מוסיף את הגנרטור של Swagger ליצירת תיעוד אוטומטי של ה-API.

//הוספת שירותי cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // מאפשר גישה מכל מקור (domain).
              .AllowAnyMethod()  // מאפשר כל שיטה HTTP (GET, POST, PUT, DELETE וכו').
              .AllowAnyHeader();  // מאפשר כל כותרת HTTP.
    });
});
//חיבור ל-DB
builder.Services.AddDbContext<ToDoDBContext>(options=>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))
    )
);

var app = builder.Build();

// Swagger הפעלת 
//if (app.Environment.IsDevelopment())  
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// cors הפעלת ה 
app.UseCors();

app.MapGet("/",()=>"הפרויקט רץ");

app.MapGet("/items",async (ToDoDBContext context) => await context.Items.ToArrayAsync());

app.MapPost("/items",async(ToDoDBContext context,Item item)=>{
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (ToDoDBContext context, int id, Item updatedItem) =>
{
    var item = await context.Items.FindAsync(id);
    if (item is null)
        return Results.NotFound();
    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;
    await context.SaveChangesAsync(); 
    return Results.Created($"/items/{item.Id}", item);
});

app.MapDelete("/items/{id}", async (ToDoDBContext context, int id) =>
{
    var item = await context.Items.FindAsync(id);
    if (item is null)
        return Results.NotFound();
    context.Items.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/",()=>"TodoApi is running");

app.Run();