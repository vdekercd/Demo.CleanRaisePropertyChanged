using GalaSoft.MvvmLight;
using System.Threading.Tasks;
using Demo.CleanRaisePropertyChanged.Interceptors;

namespace Demo.CleanRaisePropertyChanged.ViewModel
{
public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        StartIncrementIndex();
    }

    [RaisePropertyChanged]
    public virtual int Index { get; set; }

    private async Task StartIncrementIndex()
    {
        for (var i = 0; i < 1000; i++)
        {
            await Wait();
            Index += 1;
        }
    }

    private async Task Wait()
    {
        await Task.Delay(1000);
    }
}
}