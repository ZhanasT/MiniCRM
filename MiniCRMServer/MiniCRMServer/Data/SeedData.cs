using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MiniCRMServer.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                    return;
                
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                roleManager.CreateAsync(new IdentityRole("admin")).Wait();
                var adminPassword = "admin";
                var employeePassword = "password";
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Patronymic = "Admin",
                    Position = "Администратор",
                };
                string adminHashedPassword = userManager.PasswordHasher.HashPassword(admin, adminPassword);
                admin.PasswordHash = adminHashedPassword;
                userManager.CreateAsync(admin).Wait();
                userManager.AddToRoleAsync(admin, "admin").Wait();
                var employees = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        UserName = "Ivan",
                        FirstName = "Иван",
                        LastName = "Иванов",
                        Patronymic = "Иванович",
                        Position = "Менеджер",
                    },
                    new ApplicationUser
                    {
                        UserName = "Petr",
                        FirstName = "Петр",
                        LastName = "Петров",
                        Patronymic = "Петрович",
                        Position = "Специалист по продажам",
                    },
                    new ApplicationUser
                    {
                        UserName = "Maria",
                        FirstName = "Мария",
                        LastName = "Сидорова",
                        Patronymic = "Ивановна",
                        Position = "Разработчик",
                    },
                    new ApplicationUser
                    {
                        UserName = "Elena",
                        FirstName = "Елена",
                        LastName = "Смирнова",
                        Patronymic = "Петровна",
                        Position = "Проектный менеджер",
                    },
                    new ApplicationUser
                    {
                        UserName = "Anna",
                        FirstName = "Анна",
                        LastName = "Козлова",
                        Patronymic = "Сергеевна",
                        Position = "Специалист по маркетингу",
                    }
                };
                foreach (var employee in employees)
                {
                    string hashedPassword = userManager.PasswordHasher.HashPassword(employee, employeePassword);
                    employee.PasswordHash = hashedPassword;
                    userManager.CreateAsync(employee).Wait();
                }
                var tasks = new List<TaskModel> {
                    new TaskModel
                    {
                        Title = "Планирование маркетинговой кампании",
                        Description = "Разработать стратегию маркетинга на следующий квартал.",
                        StartDate = new DateTime(2023, 10, 15),
                        DeadLine = new DateTime(2023, 11, 15),
                        CompetionRate = 100,
                        UserId = employees[0].Id,
                    },
                    new TaskModel
                    {
                        Title = "Встреча с клиентом",
                        Description = "Провести встречу с ключевым клиентом для обсуждения нового проекта.",
                        StartDate = new DateTime(2023, 10, 20),
                        DeadLine = new DateTime(2023, 11, 25),
                        CompetionRate = 80,
                        UserId = employees[0].Id,
                    },
                    new TaskModel
                    {
                        Title = "Обучение сотрудников",
                        Description = "Организовать тренинг для сотрудников по новым продуктам.",
                        StartDate = new DateTime(2023, 10, 10),
                        DeadLine = new DateTime(2023, 10, 31),
                        CompetionRate = 60,
                        UserId = employees[0].Id,
                    },
                    new TaskModel
                    {
                        Title = "Поиск новых клиентов",
                        Description = "Провести маркетинговые исследования и найти новых потенциальных клиентов.",
                        StartDate = new DateTime(2023, 10, 14),
                        DeadLine = new DateTime(2023, 11, 14),
                        CompetionRate = 30,
                        UserId = employees[1].Id,
                    },
                    new TaskModel
                    {
                        Title = "Подготовка презентации",
                        Description = "Подготовить презентацию для встречи с клиентами.",
                        StartDate = new DateTime(2023, 10, 19),
                        DeadLine = new DateTime(2023, 10, 23),
                        CompetionRate = 70,
                        UserId = employees[1].Id,
                    },
                    new TaskModel
                    {
                        Title = "Обработка запросов клиентов",
                        Description = "Обработать запросы клиентов и предоставить необходимую информацию.",
                        StartDate = new DateTime(2023, 10, 8),
                        DeadLine = new DateTime(2023, 10, 14),
                        CompetionRate = 100,
                        UserId = employees[1].Id,
                    },
                    new TaskModel
                    {
                        Title = "Разработка нового модуля",
                        Description = "Написать и протестировать новый модуль для программного продукта.",
                        StartDate = new DateTime(2023, 10, 10),
                        DeadLine = new DateTime(2023, 10, 31),
                        CompetionRate = 80,
                        UserId = employees[2].Id,
                    },
                    new TaskModel
                    {
                        Title = "Оптимизация базы данных",
                        Description = "Провести оптимизацию базы данных для увеличения производительности.",
                        StartDate = new DateTime(2023, 10, 15),
                        DeadLine = new DateTime(2023, 10, 25),
                        CompetionRate = 80,
                        UserId = employees[2].Id,
                    },
                    new TaskModel
                    {
                        Title = "Интеграция сторонних API",
                        Description = "Реализовать интеграцию с API сторонних сервисов.",
                        StartDate = new DateTime(2023, 10, 12),
                        DeadLine = new DateTime(2023, 10, 18),
                        CompetionRate = 60,
                        UserId = employees[2].Id,
                    },
                    new TaskModel
                    {
                        Title = "Планирование проекта",
                        Description = "Разработать план проекта и распределить задачи между участниками команды.",
                        StartDate = new DateTime(2023, 10, 05),
                        DeadLine = new DateTime(2023, 10, 20),
                        CompetionRate = 60,
                        UserId = employees[3].Id,
                    },
                    new TaskModel
                    {
                        Title = "Оценка рисков",
                        Description = "Провести анализ рисков и разработать стратегии их снижения.",
                        StartDate = new DateTime(2023, 10, 8),
                        DeadLine = new DateTime(2023, 10, 15),
                        CompetionRate = 100,
                        UserId = employees[3].Id,
                        
                    },
                    new TaskModel
                    {
                        Title = "Взаимодействие с заказчиком",
                        Description = "Общение с заказчиком для уточнения требований и предоставление обратной связи.",
                        StartDate = new DateTime(2023, 10, 5),
                        DeadLine = new DateTime(2023, 10, 14),
                        CompetionRate = 100,
                        UserId = employees[3].Id,
                    },
                    new TaskModel
                    {
                        Title = "Социальные медиа кампания",
                        Description = "Запустить рекламную кампанию в социальных сетях.",
                        StartDate = new DateTime(2023, 10, 08),
                        DeadLine = new DateTime(2023, 10, 28),
                        CompetionRate = 70,
                        UserId = employees[4].Id,
                    },
                    new TaskModel
                    {
                        Title = "Анализ рынка",
                        Description = "Провести анализ рынка и конкурентов для разработки маркетинговой стратегии.",
                        StartDate = new DateTime(2023, 10, 10),
                        DeadLine = new DateTime(2023, 10, 31),
                        CompetionRate = 50,
                        UserId = employees[4].Id,
                    },
                    new TaskModel
                    {
                        Title = "Email-рассылка",
                        Description = "Подготовить и отправить email-рассылку для клиентов.",
                        StartDate = new DateTime(2023, 10, 12),
                        DeadLine = new DateTime(2023, 10, 15),
                        CompetionRate = 100,
                        UserId = employees[4].Id,
                    }
                };
                context.Tasks.AddRange(tasks);
                context.SaveChanges(true);
            }
        }
    }
}
