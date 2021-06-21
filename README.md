## JoyLaunch

### _Starter for games/programs with customizable menu (at Win tray)_

&nbsp;

#### Screenshots:

_LMB menu_

![LMB](/screenshots/Screenshot_LMB.png)

_RMB menu_

![RMB](/screenshots/Screenshot_RMB.png)

_Settings_

![Settings](/screenshots/Screenshot_Settings.png)

&nbsp;

#### Changes:

_v.1.0.3.0_

- улучшено качество иконок в ЛКМ меню
- при добавлении программы с помощью drag'n'drop устанавливается фокус и выделение на последний элемент
- добавлена возможность выбора exe-файла для иконки по нажатию ПКМ
- расширен список исполняемых файлов по нажатию ПКМ
- рефакторинг кода

_v.1.0.2.2_

- исправлена ошибка удаления последнего элемента

_v.1.0.2.1_

- исправлено изменение путей по нажатию правой кнопки мыши

_v.1.0.2.0_

- добавлены иконки в ЛКМ меню, которые могут рендериться из указанных изображений или браться из EXE файлов
- добавлена возможность добавления программ в настройках по нажатию INSERT
- добавлена возможность удаления программ в настройках по нажатию DELETE
- добавлена возможность вставки разделителя меню в настройках по нажатию CTRL + INSERT
- добавлена возможность изменения порядка программ в настройках по нажатию CTRL + UP|DOWD
- добавлена возможность изменения пути логотипа и пути программы по нажатию ПКМ

_v.1.0.1.1_

- исправлена обработка путей файлов

_v.1.0.1.0_

- добавлен движок kaitai для обработки LNK файлов
- переделан механизм получения пути и аргументов оригинального приложения, а также пути иконки из ярлыка
- добавлена возможность работы с аргументами приложений для запуска

_v.1.0.0.2_

- добавлена иконка приложения

_v.1.0.0.1_

- обновлена иконка в трее
- добавлены иконки для меню, вызываемого ПКМ

_v.1.0.0.0_

- отображение программы только в трее
- запрет запуска нескольких копий программы
- по нажатию ЛКМ выводится список приложений для запуска
- по нажатию ПКМ - меню настроек и выход
- при выборе настроек открывается список, в который можно добавить приложение с помощью drag'n'drop (работает даже с ярлыками - из них извлекается оригинальный путь)
- по двойному клику можно редактировать соответствующее значение записи настроек
- при закрытии окна настроек перестраивается меню программ, а настройки записываются в профиль пользователя
- при старте программы настройки считываются из профиля пользователя

&nbsp;

### How to compile

#### in Microsoft Visual Studio Community 2019:

open project and build

#### or

#### without IDE:

##### required tools:
- [Git](http://git-scm.com/download/win)
- [Build Tools for Visual Studio 2019](https://visualstudio.microsoft.com/ru/thank-you-downloading-visual-studio/?sku=BuildTools&rel=16)
- [NuGet](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
- [.NET Framework 4.5.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net452-developer-pack-offline-installer)

```cmd
:: Install build tools
vs_buildtools__XXXXX.exe --add Microsoft.VisualStudio.Workload.MSBuildTools --layout c:\offlineBuildTool
cd c:\offlineBuildTool
vs_setup.exe

:: Install .Net SDK if required
NDP452-KB2901951-x86-x64-DevPack.exe

:: Go to any directory
cd YOUR_DIR_WITH_PROJECTS

:: Clone repo
git clone https://github.com/SerVerXPlanet/JoyLaunch.git

:: Go to project
cd JoyLaunch

:: Create directory for outside packages
mkdir packages

:: Get necessary packages
YOUR_DIR_WITH_NUGET\nuget.exe install JoyLaunch\packages.config -OutputDirectory packages

:: Build binaries
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" JoyLaunch.sln /property:Configuration=Release

:: Go to our program
cd JoyLaunch\bin\Release
```

&nbsp;

**Free Software, enjoy!**
