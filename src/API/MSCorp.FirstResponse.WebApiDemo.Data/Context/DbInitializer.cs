using System.Linq;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Context
{
    public static class DbInitializer
    {
        public static void Initialize(FirstResponseContext context, UserManager<User> userManager)
        {
            bool isDatabaseAlreadyInitialized = context.Cities.Any();

            if (!isDatabaseAlreadyInitialized)
            {
                var users = Seed.GetUsers();
                foreach (User u in users)
                {
                    userManager.Create(u, u.PasswordHash);
                }

                var cities = Seed.GetCities();
                context.Cities.AddRange(cities);
                context.SaveChanges();

                foreach (City c in cities)
                {
                    Responder[] responders = null;
                    var heatMap = Seed.GetHeatMap(c);
                    context.HeatMapPoints.AddRange(heatMap);

                    if (c.Id == 35) // For Seattle use predefined points.
                    {
                        var seattleRoutes = SeattleSeed.GetResponderRoutes();
                        context.ResponderRoutes.AddRange(seattleRoutes);
                        context.SaveChanges();
                        responders = SeattleSeed.GetResponders(c, seattleRoutes[0].Id, seattleRoutes[1].Id);
                    }
                    else
                    {
                        responders = Seed.GetResponders(c);
                    }

                    context.Responders.AddRange(responders);
                }

                context.SaveChanges();
            }
        }
    }
}
