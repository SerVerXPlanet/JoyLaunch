## JoyLaunch

### _Starter for games (programs) [at Win tray]_

&nbsp;

#### Изменения:

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

- git clone https://github.com/SerVerXPlanet/JoyLaunch.git
- cd JoyLaunch
- %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe JoyLaunch.sln /property:Configuration=Release
- cd JoyLaunch\bin\Release

&nbsp;

### License
[MIT](https://github.com/git/git-scm.com/blob/main/MIT-LICENSE.txt)

&nbsp;

**Free Software, enjoy!**
