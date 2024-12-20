namespace UserPanel.Providers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using Microsoft.CodeAnalysis.Elfie.Diagnostics;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using UserPanel.Models.Logs;

    public class EfLogger : ILogger
    {
        private readonly AppDbContext _dbContext;
        private readonly string _categoryName;

        public EfLogger(AppDbContext dbContext, string categoryName)
        {
            _dbContext = dbContext;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (state is IReadOnlyList<KeyValuePair<string, object>> structuredState)
            {
                if (eventId.Name != "LogUserEntry") return;

                var pair = structuredState.FirstOrDefault();
                var value = pair.Value;

                if (value == null)
                {
                    return;
                }

                if(!(value is string))
                {
                    return;
                }

                string json = value as string;

                LogUserEntry? logUserEntry = null;
                try
                {
                    logUserEntry = JsonSerializer.Deserialize<LogUserEntry>(json);
                }
                catch (JsonException)
                {
                    return;
                }

                if (logUserEntry != null) { 
                
                    _dbContext.LogUsers.Add(logUserEntry);
                    _dbContext.SaveChanges();
                
                }
            }
            
          
        }
    }

    public class EfLoggerProvider : ILoggerProvider
    {
        private readonly AppDbContext _dbContext;

        public EfLoggerProvider(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EfLogger(_dbContext, categoryName);
        }

        public void Dispose() { }
    }

}
