using API.Helper;
using Bussiness.Service.Employees;
using Bussiness.Service.Interface;
using Data;
using Data.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<Context>(i => i.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
builder.Services.AddDbContext<IdentityContext>(options =>
		  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(90)));


//builder.Services.AddDbContext<IdentityDbContext>(options =>
//		   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<AppDBViewContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(90)));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var appSettingSection = builder.Configuration.GetSection("APISettings");
builder.Services.Configure<APISettings>(appSettingSection);

var apiSettings = appSettingSection.Get<APISettings>();
var key = Encoding.ASCII.GetBytes(apiSettings.SecretKey);


builder.Services.AddAuthentication(opt =>
{
opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidAudience = apiSettings.ValidAudience,
				ValidIssuer = apiSettings.ValidIssuer,
				ClockSkew = TimeSpan.Zero
			};
		});
builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(o => o.AddPolicy("API", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddScoped<IEmployeeInterface, EmployeeService>();
builder.Services.AddRazorPages();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<IdentityContext>()
				.AddDefaultTokenProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddSwaggerGen(c =>
{
	c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please add Bearer and then token in the field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
				   {
					 new OpenApiSecurityScheme
					 {
					   Reference = new OpenApiReference
					   {
						 Type = ReferenceType.SecurityScheme,
						 Id = "Bearer"
					   }
					  },
					  new string[] { }
					}
				});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



//app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("API");
app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();




app.MapRazorPages();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapDefaultControllerRoute();
	endpoints.MapFallbackToFile("index.html");
	endpoints.MapControllers();
	
});

app.Run();
