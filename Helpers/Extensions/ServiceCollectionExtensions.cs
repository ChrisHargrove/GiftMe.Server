using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

namespace Helpers.Extensions;

/// <summary>
/// Extension class for all the helper methods related to the ServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers all classes in the AppDomain by their given implemented interface.
    /// </summary>
    /// <param name="serviceCollection">The service collection that these services are being added to.</param>
    /// <typeparam name="T">The base interface all these services must have implemented.</typeparam>
    /// <returns>
    /// Returns the service collection to allow for chaining of other methods after this, in fluent style. 
    /// </returns>
    public static IServiceCollection AddAllScoped<T>(this IServiceCollection serviceCollection) {
        Func<Type, bool> checkFunc = typeof(T).IsInterface
            ? t => t is { IsAbstract: false, IsClass: true } && t.GetInterfaces().Contains(typeof(T))
            : t => t is { IsAbstract: false, IsClass: true } && t.BaseType == typeof(T);
        
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()).Where(checkFunc);
        foreach (Type type in types) {
            Console.WriteLine($"Registering: {type.Name}");
            serviceCollection.AddScoped(type);
        }
        return serviceCollection;
    }

    /// <summary>
    /// Allows for the system to register the firebase admin sdk so that it can be used to authoritatively handle authentication.
    /// It registers the main FirebaseApp object as well as the FirebaseAuth object so they can be retrieved in the service layer.
    /// </summary>
    /// <param name="serviceCollection">The service collection that the firebase app is being registered to.</param>
    /// <param name="configManager">The configuration for the app to retrieve the firebase configuration from.</param>
    /// <returns>Registers the service collection to allow for chaining of other methods after this, in fluent style.</returns>
    public static IServiceCollection AddFirebaseApp(this IServiceCollection serviceCollection, ConfigurationManager configManager) {
        //Get the Firebase Credentials that will be used to register the app with google.
        FirebaseCredentials? creds = configManager.GetSection("Firebase").GetSection("Credentials").Get<FirebaseCredentials>();
        FirebaseApp.Create(new AppOptions {
            //Use The retrieved object from the AppSetting to construct a JSON representation to send to google. 
            Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(creds))
        });

        //Register both firebase app objects as singletons to make sure they last for the entire length of the program execution.
        serviceCollection.AddSingleton(FirebaseApp.DefaultInstance);
        serviceCollection.AddSingleton(FirebaseAuth.DefaultInstance);

        Console.WriteLine("Registering: FirebaseApp, FirebaseAuth");
        return serviceCollection;
    }

    /// <summary>
    /// Allows for the adding of swagger documentation generation that also adds the ability to use authorisation on the swagger website.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthSwaggerGen(this IServiceCollection serviceCollection) {
        serviceCollection.AddSwaggerGen(opt => {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Authorization Required!",
                BearerFormat = "JWT",
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http,
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme() {
                        Reference = new OpenApiReference() {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new string[]{}
                }
            });
        });
        return serviceCollection;
    }
}