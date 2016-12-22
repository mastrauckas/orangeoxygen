using Autofac;
using OrangeOxygen.FileMoving;

namespace OrangeOxygen
{
    static public class Bootstrap
    {
        static public IContainer InitializeContainer(string digitalFilesBaseDirectory, string digitalFilesMoveDirectory, string digitalFilesToeMoveBadDateDirectory)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ResourceManager>()
                .As<IResourceManager>()
                .WithParameter("digitalFilesBaseDirectory", digitalFilesBaseDirectory)
                .SingleInstance();

            builder.RegisterType<FileDateMove>()
                .WithParameter("digitalFilesMoveDirectory", digitalFilesMoveDirectory)
                .As<IFileMove>()
                .SingleInstance();

            builder.RegisterType<BadDateFileMove>()
                .WithParameter("digitalFilesMoveDirectory", digitalFilesToeMoveBadDateDirectory)
                .As<IFileMove>()
                .SingleInstance();

            return builder.Build();
        }
    }
}
