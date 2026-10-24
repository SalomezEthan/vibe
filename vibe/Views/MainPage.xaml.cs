using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vibe.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vibe.Factories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vibe.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
    private readonly MainViewModel viewModel;

    public MainPage()
    {
        InitializeComponent();
        
        this.viewModel = new MainViewModel(
            this.RootFrame,
            App.Provider.GetRequiredService<MusicPlayerViewModel>(),
            App.Provider.GetRequiredService<ViewModelFactory>()
            );

        this.DataContext = this.viewModel;

        this.Loaded += async (s, e) =>
        {
            await this.viewModel.LoadFolderTreeAsync();
        };
    }

    private void AppTitleBar_BackRequested(TitleBar sender, object args)
    {
        // TODO
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        this.viewModel.Player.TogglePlaybackState();
    }

    private async void Previous_Click(object sender, RoutedEventArgs e)
    {
        await this.viewModel.Player.PreviousAsync();
    }

    private void Shuffle_Click(object sender, RoutedEventArgs e)
    {
        this.viewModel.Player.ToggleOrderingState();
    }

    private async void Next_Click(object sender, RoutedEventArgs e)
    {
        await this.viewModel.Player.NextAsync();
    }
}
