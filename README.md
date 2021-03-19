<br/><br/><br/>
<p align="center">
    <img src="https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Branding/lilwidgets_logo_noborder.png" alt="Lil Widgets Icon" width="50%"/>
</p>
<br/><br/><br/>

# LilWidgets [![nuget](https://img.shields.io/nuget/v/LilWidgets.Xamarin.Forms)](https://www.nuget.org/packages/LilWidgets.Xamarin.Forms/)

LilWidgets is an early staged Xamarin.Forms library that provides customizable controls (widgets).

## The Plan
The plan for LilWidgets is to empower Xamarin.Forms developers by providing unique custom controls that otherwise would take away from the core app development. This repository will always be open source and under the MIT license. Therefore, future releases and updates will be fully available.

### Available Widgets

- <a href="https://github.com/ChaseRoth/LilWidgets/wiki/Progress-Widget">*Progress Widget*</a>
- <a href="https://github.com/ChaseRoth/LilWidgets/wiki/Loading-Widget">*Loading Widget*</a>

### Coming Soon:

- Pie Chart Widget

## Supported Platforms

- Android (>= 8.0 - API 26)
- iOS (>= 13.0)

## Dependencies

- <a href="https://www.nuget.org/packages/SkiaSharp.Views.Forms/2.80.2">SkiaSharp.Views.Forms</a> (>= 2.80.2)
- <a href="https://www.nuget.org/packages/Xamarin.Forms/%204.6.0.1141">Xamarin.Forms</a> (>= 4.6.0.1141)

## Most Recent Addition - Loading Widget

The *Loading Widget* is designed to show an infinite loading cycle. This can be used when your app is making a web request and its unknown how long it will take for the targeted resources to return and be populated into the user interface.


Loading Widget Example Page | Loading Widget Test Page
-------------------------|-------------------------
![Progress Widget Example Page](https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Sceenshots/LoadingWidget/screenshot_example1.png)  |  ![Progress Widget Testing Page](https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Sceenshots/LoadingWidget/screenshot_test1.png)

## How to Implement

A simple example of implementing the Loading Widget into your xaml is as follows:
```xaml
<lilWidget:LoadingWidget VerticalOptions="CenterAndExpand"                        
                         ArcColor="#DB3A34"/>
```

Here we are providing a *ArcLength* value of 180° which will make our arc's length be half of a circle.

A more customized implementation would look as follows:
```xaml
 <lilWidget:LoadingWidget VerticalOptions="CenterAndExpand"
                          WidthRequest="100"
                          HeightRequest="100"
                          ArcLength="180"
                          ArcColor="#DB3A34"/>
```

Find out more on the <a href="https://github.com/ChaseRoth/LilWidgets/wiki/Loading-Widget">*Loading Widget*</a> wiki page.
