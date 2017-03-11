# Pillar

[![Build Status](https://ci.appveyor.com/api/projects/status/2qrtolh41cn80ssi/branch/develop?svg=true)](https://ci.appveyor.com/project/asimmon/pillar/branch/develop)
[![NuGet version](https://badge.fury.io/nu/Pillar.svg)](https://badge.fury.io/nu/Pillar)

Pillar is a standalone MVVM framework for [Xamarin.Forms](https://xamarin.com/forms) 2.2+. With this framework, you won't have to deal with page navigation or messed up code-behind anymore. Now, it's all about **view models**, and **navigation between view models**. It uses a modified version of the [ASP.NET Core Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) for dependency injection. You can easily use your own IoC container by implementing an adapter class.

## Features

* [ViewModel navigation](#navigation), you won't need to manipulate pages in your view models
* Design your apps with unit testing in mind with dependency injection
* Flexible, you can use differents patterns: ViewModel first, Messaging, ViewModelLocator (without static classes, wiki page coming soon)
* [EventToCommand](#eventtocommandbehavior) behavior and useful converters included
* Useful views: [ItemsView repeater](#itemsview-with-templateselector-by-type), with optional DataTemplate selector by item type
* Not intrusive, you can reuse your view models in other projects (WPF for example) with very few modifications 

## Get started

[Install the nuget package](https://www.nuget.org/packages/Pillar/):

    Install-Package Pillar

Extend the `PillarBootstrapper` class to configure your view models and views. Then, in your Application class, instantiate it and call the `Run` method.

Here is an example:

```C#
public class MyAppBootstrapper : PillarBootstrapper
{
    // 1. Keep a reference of the Application instance to set the main page later.
    // You will start the app with new MyAppBootstrapper(this).Run();
    private readonly Application _app;

    public MyAppBootstrapper(Application app)
    {
        _app = app; 
    }

    // 2. Register your dependencies or Autofac modules here.
    // Don't forget to call the base implementation to register Pillar dependencies.
    protected override void ConfigureContainer(ContainerBuilder builder)
    {
		base.ConfigureContainer(builder);
        
        builder.RegisterType<LoginViewModel>();
        builder.RegisterType<LoginView>();
    }

    // 3. Map your view models to your views with the Pillar view factory.
    protected override void RegisterViews(IViewFactory viewFactory)
    {
        viewFactory.Register<LoginViewModel, LoginView>();
    }

    // 4. Grab your first view model and its corresponding page,
    // and set it as your application main page. Your app is now started!
    protected override void ConfigureApplication(IContainer container)
    {
        var viewFactory = container.Resolve<IViewFactory>();
        var page = viewFactory.Resolve<LoginViewModel>();

        _app.MainPage = new NavigationPage(page);
    }
}

public class App : Application
{
    public App()
    {
        new MyAppBootstrapper(this).Run();
    }
}
```

The view models that will be associated to pages need to extend the `PillarViewModelBase`, or you will get a compilation error. It is a child class of the ViewModelBase from MvvmLight library. This class provides useful observable properties for mobile applications:

* **Title** (*string*): You can bound it to the associated page Title
* **NoHistory** (*boolean*): If true, it will remove the previous page from the navigation stack
* **IsBusy** (*boolean*): Can be used to show or hide a loading spinner using the included BooleanConverter with the Visibility property

## Navigation

With Pillar, view models and pages are fully decoupled. To navigate between view models, you need to inject the `INavigator` Pillar service into your view models (do not confuse `INavigator` with `INavigation` from Xamarin.Forms). This service provides the following methods:

```C#
// Navigate to a view model (the lifecycle is managed by Autofac)
await _navigator.PushAsync<HomeViewModel>();
await _navigator.PushModalAsync<HomeViewModel>();

// Navigate to a specific view model instance
var home = new HomeViewModel();
await _navigator.PushAsync(home);
await _navigator.PushModalAsync(home);

// Navigate to a view model and apply changes on it
await _navigator.PushAsync<HomeViewModel>(vm => { vm.Title = "Home page"; });
await _navigator.PushModalAsync<HomeViewModel>(vm => { vm.Title = "Home page"; });

// Navigate back
await _navigator.PopAsync();
await _navigator.PopModalAsync();
await _navigator.PopToRootAsync();
```

You may notice that some of these methods can take an action as parameter. This is the preferred way to pass data from one view model to another. All of these methods - except `PopToRootAsync` - return the resolved instance of the destination view model type.

How to use it:

```C#
public class FirstViewModel : PillarViewModelBase
{
    private readonly INavigator _navigator;

    public FirstViewModel(INavigator navigator)
    {
        _navigator = navigator;

        // Go to the second view after 5 seconds
        GoToSecondViewModel();
    }

    public async void GoToSecondViewModel()
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        await _navigator.PushAsync<SecondViewModel>();
    }
}
```

## EventToCommandBehavior

Pillars provides a behavior that allows you to bind any view event to a command. This is an example that bind the ItemTapped event of a ListView to a command:

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" xmlns:b="clr-namespace:HelloEventToCommand.Behaviors;assembly=HelloEventToCommand" xmlns:c="clr-namespace:HelloEventToCommand.Converters;assembly=HelloEventToCommand" x:Class="HelloEventToCommand.Views.HomeView">
  <ContentPage.Resources>
    <ResourceDictionary>
      <c:ItemTappedEventArgsConverter x:Key="ItemTappedConverter" />
    </ResourceDictionary>
  </ContentPage.Resources>

  <ListView ItemsSource="{Binding People}">
    <ListView.Behaviors>
      <b:EventToCommandBehavior EventName="ItemTapped" Command="{Binding SayHelloCommand}" EventArgsConverter="{StaticResource ItemTappedConverter}" />
    </ListView.Behaviors>
    <ListView.ItemTemplate>
      <DataTemplate>
        <TextCell Text="{Binding Name}"/>
      </DataTemplate>
    </ListView.ItemTemplate>
  </ListView>
</ContentPage>
```

In this example, we use a converter to extract the tapped item's BindingContext and pass it to our command. You can see the full example [here, on my blog](http://anthonysimmon.com/eventtocommand-in-xamarin-forms-apps/).

### Properties

The EventToCommandBehavior class has the following properties:

* **EventName** (*string*): Name of the event to bind.
* **Command** (*ICommand*): Command that will be fired when the event will be raised
* **CommandParameter** (*object*): Optional parameter to pass to the command
* **EventArgsConverter** (*IValueConverter*): Optional converter that will convert an EventArgs to something that will be passed as command parameter. Overrides any user defined command parameter with the CommandParameter property.
* **EventArgsConverterParameter** (*object*): Optional parameter that will be passed to the EventArgsConverter.

## ItemsView with TemplateSelector by type

The ItemsView is a  way for displaying a list of items. When used with the TemplateSelector, you can define a template for each different type of items contained in the items source. Quick example:

```XML
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:v="clr-namespace:Pillar.Views;assembly=Pillar"
             xmlns:vm="clr-namespace:HelloXam.ViewModels;assembly=HelloXam"
             x:Class="HelloXam.Views.FirstView" Title="First">
  <ContentPage.Resources>
    <ResourceDictionary>
      <v:TemplateSelector x:Key="TemplateSelector">
        <v:DataTemplateWrapper x:TypeArguments="vm:Foo" IsDefault="True">
          <DataTemplate>
            <Label Text="Template for type Foo"   />
          </DataTemplate>
        </v:DataTemplateWrapper>
        <v:DataTemplateWrapper x:TypeArguments="vm:Bar">
          <DataTemplate>
            <Label Text="Template for type Bar" />
          </DataTemplate>
        </v:DataTemplateWrapper>
        <v:DataTemplateWrapper x:TypeArguments="vm:Qux">
          <DataTemplate>
            <Label Text="Template for type Qux" />
          </DataTemplate>
        </v:DataTemplateWrapper>
      </v:TemplateSelector>
    </ResourceDictionary>
  </ContentPage.Resources>

  <StackLayout>
    <v:ItemsView ItemsSource="{Binding Things}" TemplateSelector="{StaticResource TemplateSelector}" Orientation="Vertical" />
  </StackLayout>
</ContentPage>
```

The items source is:

```C#
Things = new ObservableCollection<Thing>
{
    new Bar(),
    new Foo(),
    new Qux(),
    new Qux(),
    new Foo()
};
```

The result:

![](http://anthonysimmon.com/wp-content/uploads/pillar/pillar-itemsview-result.png)

The ItemsView class provides an Orientation property, just like the StackLayout class.
If a template is not defined for a specific type, the TemplateSelector will look for the first template with the IsDefault property set to true.

The rest of the documentation will be available soon in the wiki section. Checkout the demo app to see examples of each features of Pillar.

Thank you for your interest in this framework.

I would like to thanks Jonathan Yates (https://web.archive.org/web/20161015115825/https://adventuresinxamarinforms.com/) for his tutorials. It has been a great source of inspiration and he is doing a very good job.
