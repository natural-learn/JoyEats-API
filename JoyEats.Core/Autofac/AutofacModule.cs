using Autofac;
using JoyEats.Core.Autofac.DependencyInjection;
using System.Reflection;

namespace JoyEats.Core.Autofac
{
    public class AutofacModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var abs = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x))).ToArray();

            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency(); //瞬态
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(IScopeDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope(); //范围
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance(); //单例
        }
    }
}
