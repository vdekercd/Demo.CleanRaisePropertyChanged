using Unity;
using Unity.Interception;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

namespace Demo.CleanRaisePropertyChanged.ViewModel
{
    public class ViewModelLocator
    {
        private IUnityContainer _container;

        public ViewModelLocator()
        {
            _container = new UnityContainer();

            _container.AddNewExtension<Interception>();
            _container.RegisterType<MainViewModel>().
                Configure<Interception>().
                SetInterceptorFor<MainViewModel>(new VirtualMethodInterceptor());
        }

        public MainViewModel Main => _container.Resolve<MainViewModel>();

        public static void Cleanup()
        { }
    }
}