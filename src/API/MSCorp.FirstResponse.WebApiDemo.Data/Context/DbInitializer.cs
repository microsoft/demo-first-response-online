using System.Linq;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using Microsoft.AspNet.Identity;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Context
{
    public static class DbInitializer
    {
        public static async void Initialize(FirstResponseContext context, UserManager<User> userManager)
        {
            bool isDatabaseAlreadyInitialized = context.Cities.Any();

            if (!isDatabaseAlreadyInitialized)
            {
                var users = Seed.GetUsers();
                foreach (User u in users)
                {
                    await userManager.CreateAsync(u, u.PasswordHash);
                }
                
                var cities = Seed.GetCities();
                context.Cities.AddRange(cities);
                context.SaveChanges();

                foreach (City c in cities)
                {
                    var responders = Seed.GetResponders(c);
                    var heatMap = Seed.GetHeatMap(c);
                    context.Responders.AddRange(responders);
                    context.HeatMapPoints.AddRange(heatMap);
                }

                var seattle = SeattleSeed.GetCity();
                context.Cities.Add(seattle);

                var seattleHeatMap = Seed.GetHeatMap(seattle);
                context.HeatMapPoints.AddRange(seattleHeatMap);

                var seattleRoutes = SeattleSeed.GetResponderRoutes();
                context.ResponderRoutes.AddRange(seattleRoutes);
                context.SaveChanges();

                var seattleResponders = SeattleSeed.GetResponders(seattle, seattleRoutes[0].Id, seattleRoutes[1].Id);
                context.Responders.AddRange(seattleResponders);

                context.SaveChanges();
            }
        }
    }
}
