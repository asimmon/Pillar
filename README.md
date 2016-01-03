# Pillar

[![Build Status](https://travis-ci.org/asimmon/Pillar.svg?branch=master)](https://travis-ci.org/asimmon/Pillar)

Pillar is a MVVM framework for [Xamarin.Forms](https://xamarin.com/forms) 1.x and 2.x. With this framework, you won't have to deal with page navigation or messed up code-behind anymore. Now, it's all about **view models**, and **navigation between view models**. It rely on [Autofac](http://autofac.org/) for dependency injection and [MvvmLight](https://mvvmlight.codeplex.com/) for base MVVM and helper classes.

## Features

* ViewModel navigation, you won't need to manipulate pages in your view models
* Design your apps with unit testing in mind with dependency injection
* Flexible, you can use differents patterns: ViewModel first, Messaging, ViewModelLocator (without static classes, wiki page coming soon)
* EventToCommand behavior and useful converters included
* Useful views: ItemsView repeater, with optional DataTemplate selector by item type
* Not intrusive, you can reuse your view models in other projects (WPF for example) with very few modifications 

## Get started

Install the nuget package and its dependencies:

    Install-Package Askaiser.Mobile.Pillar -Pre

Extend the class `PillarBootstrapper` to configure your view models and views. Then, in your Application class, instantiate it and call the `Run` method.

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
```

The view models that will be associated to pages need to extend the `PillarViewModelBase`, or you will get a compilation error. It is a child class of the ViewModelBase from MvvmLight library. This class provides useful observable properties for mobile applications:

* `Title` (string): You can bound it to the associated page Title
* `NoHistory` (boolean): If true, the associated page will be removed from the navigation stack
* `IsBusy` (boolean): Can be used to show or hide a loading spinner using the included BooleanConverter with the Visibility property

## Navigation

In Pillar, view models and pages are fully decoupled. To navigate between view models, you need to inject the `INavigator` Pillar service into your view models (do not confuse `INavigator` with `INavigation` from Xamarin.Forms). This service provides the following methods:

```C#
// Navigate to a view model (the lifecycle is managed by Autofac)
await _navigator.PushAsync<HomeViewModel>();
await _navigator.PushModalAsync<HomeViewModel>();

// Navigate to a specific view model instance
var home = new HomeViewModel();
await _navigator.PushAsync(home);
await _navigator.PushModalAsync(home);

// Navigate to a view model and apply changes ont it
await _navigator.PushAsync<HomeViewModel>(vm => { vm.Title = "Home page"; });
await _navigator.PushModalAsync<HomeViewModel>(vm => { vm.Title = "Home page"; });

// Navigate back
await _navigator.PopAsync();
await _navigator.PopModalAsync();
await _navigator.PopToRootAsync();
```

You may notice that some of these methods can take an action as parameter. This is the preferred way to pass data from one view model to another. All of these methods - except `PopToRootAsync` - return the resolved instance of the destination view model type.

The rest of the documentation will be available soon in the wiki section. Thank you for your interest in this library.
