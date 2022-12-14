using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Helpers;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;
using RMDesktopUI.ViewModels;
using RMDesktopUI.ViewModels.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();



        public Bootstrapper()
        {
            Initialize();

            // The following snippet is taken from the following link: https://stackoverflow.com/questions/30631522/caliburn-micro-support-for-passwordbox
            // To support the PasswordBox
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged"
            );
        }

        protected override void Configure()
        {
            IMapper mapper = ConfigureAutoMapper();

            _container.Instance(mapper);

            _container.Instance(_container);

            // Here we're using Singleton to ensure that only a single instance of the following classes is instantiated during the lifetime of the application
            // It's generally recommended to dump and re-instantiate class instances with every request, but in this case, we need a single instance of both
            // the WindowManager and EventAggregator to avoid unexpected behaviours when communicating.
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IApiHelper, ApiHelper>();

            _container
                .PerRequest<IDisplayBox, DisplayBox>()
                .PerRequest<IProductEndpoint, ProductEndpoint>()
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>();

            // Here we're using Reflection in order to tie the ShellViewModel to the ShellView
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));

        }

        private static IMapper ConfigureAutoMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });

            var mapper = mapperConfig.CreateMapper();
            return mapper;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}
