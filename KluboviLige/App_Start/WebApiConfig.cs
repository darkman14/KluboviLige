using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using KluboviLige.Interfaces;
using KluboviLige.Models;
using KluboviLige.Repository;
using KluboviLige.Resolver;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Unity;
using Unity.Lifetime;

namespace KluboviLige
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //AutoMapper
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Club, ClubDTO>()
                .ForMember(dest => dest.LeagueAbb, opt => opt.MapFrom(src => src.League.AbbName));
            });


            // Unity
            var container = new UnityContainer();
            container.RegisterType<IClubRepository, ClubRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeagueRepository, LeagueRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

        }
    }
}
