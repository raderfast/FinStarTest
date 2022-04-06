using Microsoft.EntityFrameworkCore;

namespace DBWorker
{
    public class Context : DbContext
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private readonly string _dbConnectionString;

        #region DbSets

        /// <summary>
        /// Набор данных
        /// </summary>
        public DbSet<Models.Body> Bodies { get; set; }

        #endregion

        /// <summary>
        /// Конструктор контекста
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <exception cref="Exception">Сообщение об ошибке</exception>
        public Context(string connectionString)
        {
            try
            {
                _dbConnectionString = connectionString;

                // Проверка наличия БД. Если БД отсутствует - создается новая.
                Database.EnsureCreated();

                if (!Database.CanConnect())
                    throw new Exception("Возникла ошибка при подключении к БД PostgreSQL. Проверьте данные подключения и повторите попытку.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла ошибка при подключении к БД PostgreSQL. Проверьте данные подключения и повторите попытку.", ex);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseNpgsql(_dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Config.ValueConfiguration());
        }
    }
}