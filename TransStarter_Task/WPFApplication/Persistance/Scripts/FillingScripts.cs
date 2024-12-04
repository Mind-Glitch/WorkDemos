using System.Diagnostics;
using System.Windows.Controls;
using TransStarter_Task.WPFApplication.API;
using TransStarter_Task.WPFApplication.Common.Entity;

namespace TransStarter_Task.WPFApplication.Persistance.Scripts;
internal class FillingScripts
{

    /// <summary>
    /// <para>.</para>
    /// <para>
    /// <strong>### УДАЛИТ И ПЕРЕСОЗДАСТ ВСЮ БАЗУ ДАННЫХ ###</strong>
    /// </para>
    /// </summary>
    /// <param name="customerCount"></param>
    /// <param name="ordersCount"></param>
    /// <param name="timeFragment"></param>
    /// <param name="loadingController"></param>
    /// <returns></returns>
    internal static async Task Regenerate (int customerCount, int ordersCount, TimeSpan timeFragment, LoadingController? loadingController)
    {
        try
        {
            var dbContext = new DatabaseContext();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            #region MockData

            List<CarModel> carsMock =
        [
            new CarModel
            {
                BrandName = "BMW",
                ModelNames =
                [
                    "320i",
                    "X5",
                    "Z4",
                    "M3",
                    "X7",
                    "530d",
                    "i8",
                    "740Li",
                    "X3 M40i",
                    "228i Gran Coupe"
                ]
            },
            new()
            {
                BrandName = "Audi",
                ModelNames =
                [
                    "A3",
                    "Q7",
                    "A6",
                    "Q5",
                    "R8",
                    "TT",
                    "e-tron",
                    "S5 Sportback",
                    "A8",
                    "Q3"
                ]
            },
            new()
            {
                BrandName = "Mercedes-Benz",
                ModelNames =
                [
                    "C-Class",
                    "E-Class",
                    "GLE",
                    "A-Class",
                    "S-Class",
                    "G-Class",
                    "CLA",
                    "GLS",
                    "AMG GT",
                    "EQS"
                ]
            }
        ];

            List<string> names =
        [
            "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth",
            "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen",
            "Christopher", "Nancy", "Daniel", "Margaret", "Matthew", "Lisa", "Anthony", "Betty", "Mark", "Sandra",
            "Donald", "Ashley", "Steven", "Kimberly", "Paul", "Emily", "Andrew", "Donna", "Joshua", "Michelle",
            "Kenneth", "Dorothy", "Kevin", "Carol", "Brian", "Amanda", "George", "Melissa", "Edward", "Deborah",
            "Ronald", "Stephanie", "Timothy", "Rebecca", "Jason", "Sharon", "Jeffrey", "Laura", "Ryan", "Cynthia",
            "Jacob", "Kathleen", "Gary", "Amy", "Nicholas", "Angela", "Eric", "Shirley", "Jonathan", "Anna",
            "Stephen", "Brenda", "Larry", "Pamela", "Justin", "Emma", "Scott", "Nicole", "Brandon", "Helen",
            "Frank", "Samantha", "Benjamin", "Katherine", "Gregory", "Christine", "Raymond", "Debra", "Samuel",
            "Rachel", "Patrick", "Carolyn", "Alexander", "Janet", "Jack", "Catherine", "Dennis", "Maria", "Jerry",
            "Heather"
        ];
            List<string> surnames =
        [
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
            "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
            "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
            "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
            "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards", "Collins", "Reyes",
            "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
            "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox", "Ward", "Richardson",
            "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza", "Ruiz", "Hughes",
            "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross", "Foster", "Jimenez"
        ];

            // Я понятия не имею какие автомобили какие комплектации содержат,
            // а мне, собственно, всё равно. В т.з. не указано kavo & kuda.
            List<string> equipment =
            [
                "Базовая",
                "Стандартная",
                "Спорт",
                "Люкс"
            ];

            List<string> colors =
            [
                "красный",
                "зелёный",
                "жёлтый",
                "чёрный",
                "белый"
            ];

            #endregion

            #region VehicleInfo

            loadingController?.SetOperationName("Заполнение марок...");
            loadingController?.SetMax(carsMock.Count);
            foreach ( var mock in carsMock )
            {
                loadingController?.SetCurrent(carsMock.IndexOf(mock));
                foreach ( var modelName in mock.ModelNames )
                {
                    foreach ( var color in colors )
                    {
                        foreach ( var eq in equipment )
                        {
                            dbContext.VehicleInfos.Add(new VehicleInfo
                            {
                                BrandName = mock.BrandName,
                                ModelName = modelName,
                                Color = color,
                                Equipment = eq
                            });
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            #endregion

            #region Customers

            loadingController?.SetOperationName("Заполнение клиентов...");
            loadingController?.SetMax(customerCount);
            for ( var customerIndex = 0; customerIndex < customerCount; customerIndex++ )
            {
                loadingController?.SetCurrent(customerIndex + 1);
                dbContext.Customers.Add(new Customer
                {
                    FullName =
                        $"{names[Random.Shared.Next(names.Count)]} {surnames[Random.Shared.Next(surnames.Count)]}"
                });
                await dbContext.SaveChangesAsync();
            }

            #endregion

            #region Orders

            var dateTo = DateTime.Now;
            var dateFrom = dateTo - timeFragment;
            var currentTime = dateFrom;

            loadingController?.SetOperationName("Заполнение заказов...");
            loadingController?.SetMax(ordersCount);
            for ( var orderIndex = 0; orderIndex < ordersCount; orderIndex++ )
            {
                loadingController?.SetCurrent(orderIndex);

                while ( Random.Shared.Next(11) < 8 )
                {
                    currentTime += TimeSpan.FromDays(Random.Shared.Next(60));
                    currentTime += TimeSpan.FromHours(Random.Shared.Next(25));
                    currentTime += TimeSpan.FromMinutes(Random.Shared.Next(61));
                    currentTime += TimeSpan.FromSeconds(Random.Shared.Next(61));
                }

                if ( currentTime > dateTo )
                    currentTime = dateFrom;

                if ( currentTime > dateFrom + TimeSpan.FromDays(365 * 2) )
                    Debug.WriteLine("F");

                var carCount = Random.Shared.Next(1, 3);
                var vehicleInfosCount = dbContext.VehicleInfos.Count();

                List<Car> cars = [];
                for ( var carIndex = 0; carIndex < carCount; carIndex++ )
                {
                    cars.Add(new Car
                    {
                        VehicleInfo = dbContext.VehicleInfos.ElementAt(Random.Shared.Next(vehicleInfosCount)),
                        Price = Random.Shared.Next(240, 13000) * 1000 // 240 000 - 12 999 999
                    });
                }

                var order = new Order
                {
                    Customer = dbContext.Customers.ElementAt(Random.Shared.Next(dbContext.Customers.Count())),
                    Cars = cars,
                    Completed = true, // Всегда true потому что это мок
                    TotalPrice = cars.Select(x=>x.Price)
                    .Aggregate((accumulated, current) => accumulated + current),
                    OrderDate = currentTime
                };

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();
            }

            #endregion

        }
        catch ( Exception ex )
        {
            Debug.WriteLine(ex);
        }
    }

    private class CarModel
    {
        public string BrandName { get; set; } = string.Empty;
        // ReSharper disable once TypeWithSuspiciousEqualityIsUsedInRecord.Local
        public List<string> ModelNames { get; set; } = [];
    }
}