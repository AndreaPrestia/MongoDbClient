using Microsoft.Extensions.Configuration;
using MongoDbClient.Settings;
using System;
using System.IO;

namespace MongoDbClient.Configuration
{
    /// <summary>
    /// Static class used to retrieve configuration
    /// </summary>
    public static class MongoClientConfiguration
    {
        /// <summary>
        /// Get the configuration from local.settings.json (ex. Azure functions environment)
        /// </summary>
        /// <returns></returns>
        public static T GetAzureConfiguration<T>() where T : class, IMongoSettings, new()
        {
            string key = typeof(T).Name;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"{nameof(key)} is null or empty");

            T instance = new T();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();

            var configuration = builder.Build();

            configuration.Bind(key, instance);

            return instance;
        }

        /// <summary>
        /// Get the configuration from appsettings.json 
        /// </summary>
        /// <returns></returns>
        public static T GetConfiguration<T>() where T : class, IMongoSettings, new()
        {
            string key = typeof(T).Name;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"{nameof(key)} is null or empty");

            T instance = new T();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();

            var configuration = builder.Build();

            configuration.Bind(key, instance);

            return instance;
        }

        /// <summary>
        /// Get configuration from specified json settings file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T GetConfiguration<T>(string filename) where T : class, IMongoSettings, new()
        {
            string key = typeof(T).Name;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"{nameof(key)} is null or empty");

            T instance = new T();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(filename, optional: true, reloadOnChange: true).AddEnvironmentVariables();

            var configuration = builder.Build();

            configuration.Bind(key, instance);

            return instance;
        }
    }
}
