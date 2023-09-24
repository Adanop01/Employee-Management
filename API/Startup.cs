//using API.Helper;
//using Data.Entity;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using static System.Net.Mime.MediaTypeNames;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;
//using Bussiness.Service.Interface;
//using Bussiness.Service.Employees;

//namespace API
//{
//	public class Startup
//	{
//		public IConfiguration Configuration { get; }
//		public Startup(IConfiguration configuration)
//		{
//			Configuration = configuration;
//		}
//		public void ConfigureServices(IServiceCollection services)
//		{
//			services.AddDbContext<Context>(i => i.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
//			services.AddDbContext<IdentityDbContext>(options =>
//			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
//			services.AddDbContext<AppDBContext>(options =>
//			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
//			sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(90)));

//			services.AddDbContext<IdentityDbContext>(options =>
//			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
//			services.AddDbContext<AppDBViewContext>(options =>
//			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
//			sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(90)));

//			services.AddMvc(x => x.MaxIAsyncEnumerableBufferLimit = 50000);
//			services.AddIdentity<IdentityUser, IdentityRole>()
//				.AddEntityFrameworkStores<IdentityDbContext>()
//				.AddDefaultTokenProviders();

//			services.Configure<ApiBehaviorOptions>(options =>
//			{
//				options.SuppressModelStateInvalidFilter = true;
//			});

//			var appSettingSection = Configuration.GetSection("APISettings");
//			services.Configure<APISettings>(appSettingSection);

//			var apiSettings = appSettingSection.Get<APISettings>();
//			var key = Encoding.ASCII.GetBytes(apiSettings.SecretKey);

//			services.AddAuthentication(opt =>
//			{
//				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//				opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//			}).AddJwtBearer(x =>
//			{
//				x.RequireHttpsMetadata = false;
//				x.SaveToken = true;
//				x.TokenValidationParameters = new TokenValidationParameters
//				{
//					ValidateIssuerSigningKey = true,
//					IssuerSigningKey = new SymmetricSecurityKey(key),
//					ValidateIssuer = true,
//					ValidAudience = apiSettings.ValidAudience,
//					ValidIssuer = apiSettings.ValidIssuer,
//					ClockSkew = TimeSpan.Zero
//				};
//			});

//			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			

//			services.AddCors(o => o.AddPolicy("API", builder =>
//			{
//				builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//			}));
			
			
//			services.AddControllersWithViews();
//			services.AddRazorPages();
//			services.AddRouting(option => option.LowercaseUrls = true);
//			services.AddSwaggerGen(c =>
//			{
//				c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
//				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//				{
//					In = ParameterLocation.Header,
//					Description = "Please add Bearer and then token in the field",
//					Name = "Authorization",
//					Type = SecuritySchemeType.ApiKey
//				});
//				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//				   {
//					 new OpenApiSecurityScheme
//					 {
//					   Reference = new OpenApiReference
//					   {
//						 Type = ReferenceType.SecurityScheme,
//						 Id = "Bearer"
//					   }
//					  },
//					  new string[] { }
//					}
//				});
//			});

//			services.AddScoped<IEmployeeInterface, EmployeeService>();



//			services.AddRazorPages();
//		}

//	}
//}
