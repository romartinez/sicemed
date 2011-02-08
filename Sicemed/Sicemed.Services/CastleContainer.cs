using System;
using Agatha.Common.InversionOfControl;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Sicemed.Services {
    public class CastleContainer : IContainer {
        private readonly IWindsorContainer _windsorContainer;

        public CastleContainer(IWindsorContainer windsorContainer) {
            this._windsorContainer = windsorContainer;
        }

        public void Register(Type componentType, Type implementationType, Lifestyle lifeStyle) {
            var registration = Component.For(componentType).ImplementedBy(implementationType);
            _windsorContainer.Register(AddLifeStyleToRegistration(lifeStyle, registration));
        }

        public void Register<TComponent, TImplementation>(Lifestyle lifestyle) where TImplementation : TComponent {
            Register(typeof(TComponent), typeof(TImplementation), lifestyle);
        }

        public void RegisterInstance(Type componentType, object instance) {
            _windsorContainer.Register(Component.For(componentType).Instance(instance));
        }

        public void RegisterInstance<TComponent>(TComponent instance) {
            RegisterInstance(typeof(TComponent), instance);
        }

        public TComponent Resolve<TComponent>() {
            return _windsorContainer.Resolve<TComponent>();
        }

        public object Resolve(Type componentType) {
            return _windsorContainer.Resolve(componentType);
        }

        public void Release(object component) {
            _windsorContainer.Release(component);
        }

        private static ComponentRegistration<TInterface> AddLifeStyleToRegistration<TInterface>(Lifestyle lifestyle, ComponentRegistration<TInterface> registration) {
            if (lifestyle == Lifestyle.Singleton) {
                registration = registration.LifeStyle.Singleton;
            } else if (lifestyle == Lifestyle.Transient) {
                registration = registration.LifeStyle.Transient;
            } else {
                throw new ArgumentOutOfRangeException("lifestyle", "Only Transient and Singleton is supported");
            }

            return registration;
        }
    }
}
