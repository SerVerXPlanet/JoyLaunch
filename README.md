## JoyLaunch

### _Starter for games (programs) [at Win tray]_

&nbsp;

#### Изменения:

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

#### required tools:
- [Git](http://git-scm.com/download/win)
- [Build Tools for Visual Studio 2019](https://visualstudio.microsoft.com/ru/thank-you-downloading-visual-studio/?sku=BuildTools&rel=16)
- [NuGet](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)

```sh
vs_buildtools__XXXXX.exe --add Microsoft.VisualStudio.Workload.MSBuildTools --layout c:\offlineBuildTool
cd c:\offlineBuildTool
vs_setup.exe
cd YOUR_DIR_WITH_PROJECTS
git clone https://github.com/SerVerXPlanet/JoyLaunch.git
cd JoyLaunch
mkdir packages
nuget.exe install JoyLaunch\packages.config
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" JoyLaunch.sln /property:Configuration=Release
cd JoyLaunch\bin\Release
```

&nbsp;

### License
[MIT](https://github.com/git/git-scm.com/blob/main/MIT-LICENSE.txt)

&nbsp;

**Free Software, enjoy!**
