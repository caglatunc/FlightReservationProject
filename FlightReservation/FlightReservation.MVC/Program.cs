using FlightReservation.MVC.Context;
using FlightReservation.MVC.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddAuthentication().AddCookie(configuration =>
{
    configuration.AccessDeniedPath = "/Account/Login";
    configuration.LoginPath = "/Account/Login";
    configuration.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //�lk Defa Kullan�c� Olu�turuyorsak, Kullan�c� olu�turduktan sonra Role'e bak�yoruz. E�er Role yoksa olu�turuyoruz.Ard�ndan Kullan�c�ya Admin Rol� veriyoruz.
    if (!context.Set<User>().Any(p => p.Email == "c11112222@sakarya.edu.tr")) // Any metodu ile veritaban�nda bu maille kay�tl� bir kullan�c� var m� diye kontrol ediyoruz.
    {
        User user = new User()
        {
            FirstName = "Cagla",
            LastName = "Tunc Savas",
            Email = "c11112222@sakarya.edu.tr",
            Password = "Password12*",
        };

        context.Set<User>().Add(user);

        Role? role = context.Set<Role>().Where(p=> p.Name == "Admin").FirstOrDefault();

        if(role is null)
        {

           role = new Role()
            {
                Name = "Admin"
            };

         context.Set<Role>().Add(role);
        }

        context.Set<UserRole>().Add(new()
        {
            RoleId = role.Id,
            UserId = user.Id
        });
    }
    

}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
