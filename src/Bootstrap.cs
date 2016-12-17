using Autofac;
using OrangeOxygen.FileCopying;
using OrangeOxygen.FileTypeCompares;

namespace OrangeOxygen
{
    static public class Bootstrap
    {
        static public IContainer InitializeContainer(string baseDirectory)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ResourceManager>()
                  .As<IResourceManager>()
                  .WithParameter("baseDirectory", baseDirectory)
                  .SingleInstance();

            //File Copying
            builder.RegisterType<DateFileCopy>()
                  .As<IFileCopy>().SingleInstance();

            //File Type Comparers.
            builder.RegisterType<ImageFileTypeCompare>()
                 .As<IFileTypeCompare>().SingleInstance();

            return builder.Build();
        }
    }
}
