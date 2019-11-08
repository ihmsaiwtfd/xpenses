using Autofac;

namespace Core
{
    public static class IocProvider // todo: this is an ad-hoc for DI wire-up and should be removed in the future
    {
        public static IContainer Container { get; set; }
    }
}
