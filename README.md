<br/><br/><br/>
<p align="center">
    <img src="https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Branding/lilwidgets_logo_noborder.png" alt="Lil Widgets Icon" width="50%"/>
</p>
<br/><br/><br/>

[![nuget](https://www.nuget.org/packages/LilWidgets.ChaseRoth/1.0.0-alpha)](https://raw.githubusercontent.com/pixel-cookers/built-with-badges/master/nuget/nuget-long.png)

# LilWidgets
LilWidgets is an early staged Xamarin.Forms library that provides customizable controls (widgets) that are not otherwise available by default.

## The Plan
The plan for LilWidgets is to empower Xamarin.Forms developers by providing unique custom controls that otherwise would take away from the core app development. This repository will always be open source and under the MIT license. Therefore, future releases and updates will be fully available.

### Available

- Progress Widget

### Coming Soon:

- Loading Widget
- Pie Chart Widget

## Most Recent Addition - Progress Widget

The *Progress Widget* is designed to show a percentage of a goal or objective. This can be anything from a quiz score where the user attained a 90%, or even a loading bar where the loading process’s progress can be given.


Progress Widget Example Page | Progress Widget Test Page
-------------------------|-------------------------
![Progress Widget Example Page](https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Sceenshots/ProgressWidget/screenshot_example1.png)  |  ![Progress Widget Testing Page](https://raw.githubusercontent.com/ChaseRoth/LilWidgets/main/Resources/Sceenshots/ProgressWidget/screenshot_test1.png)

## How to Implement

A simple example of implementing the Progress Widget into your xaml is as follows:
```xaml
<lilWidget:ProgressWidget PercentProgressValue="0.65"                                                    
                          TextColor="#323031"
                          BackArcColor="#FFC857"
                          ProgressArcColor="#DB3A34"/>
```

Here we are providing a percent value to be displayed while also overriding the default colors for the text and arcs.

A more customized implementation would look as follows:
```xaml
<lilWidget:ProgressWidget HorizontalOptions="FillAndExpand"
                          VerticalOptions="FillAndExpand"
                          PercentProgressValue="0.65"
                          StrokeWidth="20"
                          TextMargin="70"             
                          Margin="40"                                                                    
                          TextColor="#323031"
                          BackArcColor="#FFC857"
                          ProgressArcColor="#DB3A34"/>
```

Find out more on the <a href="https://github.com/ChaseRoth/LilWidgets/wiki/Progress-Widget">*Progress Widget*</a> wiki page.
