# Pillar

[![Build Status](https://travis-ci.org/asimmon/Pillar.svg?branch=master)](https://travis-ci.org/asimmon/Pillar)

Pillar is a MVVM framework for [Xamarin.Forms](https://xamarin.com/forms) 1.x and 2.x. With this framework, you won't have to deal with page navigation anymore or messed up code-behind. Now, it's all about **view models**, and navigation between view models. It rely on [Autofac](http://autofac.org/) for dependency injection and [MvvmLight](https://mvvmlight.codeplex.com/) for base MVVM and helper classes.

# Get started

First, install the nuget package ant its dependencies:

    Install-Package Askaiser.Mobile.Pillar -Pre

Now, let's say that you already have a view model HomeViewModel with a string property Title and a XAML Page, HomeView, with a binding on the Title property. Note that your HomeViewModel extends the class `PillerViewModelBase`.

 1. Create a class that extends `PillarBootstrapper`. In its constructor, pass an Xamarin.Forms Application to define later the MainPage.

 2. Override the `ConfigureContainer` method (call the base method). In this method, you can register your view model and view as Autofac dependencies.

 3. Map the HomeViewModel type to the HomeView type in the `RegisterViews` method. The purpose of this method is to create a map between view models and views so that the view model navigation system will know which page type to navigate to. 

 4. In the `ConfigureApplication` method, you can retrieve the page that you want to show as MainPage. Here, we use a NavigationPage as wrapper but you can use 

 5. Finally, in your Application class, instanciate your bootstrap class and call the `Run` method.

Here is the code that we just explained:

```C#
public class MyAppBootstrapper : PillarBootstrapper
{
    private readonly Application _app;

    public MyAppBootstrapper(Application app)
    {
        _app = app;
    }

    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        base.ConfigureContainer(builder);

        builder.RegisterType<HomeViewModel>();
        builder.RegisterType<HomeView>();
    }

    protected override void RegisterViews(IViewFactory viewFactory)
    {
        viewFactory.Register<HomeViewModel, HomeView>();
    }

    protected override void ConfigureApplication(IContainer container)
    {
        var viewFactory = container.Resolve<IViewFactory>();
        var mainPage = viewFactory.Resolve<HomeViewModel>();

        _app.MainPage = new NavigationPage(mainPage);
    }
}
```

To be continued.
