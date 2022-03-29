# ITTV


## Содержание

1. [Описание](#описание)
2. [Конфигурация](#конфигурация)
   1. [Settings](#settings)
   2. [Cache](#cache)
   4. [Games](#games)
   5. [TimeTables](#timetables)
   6. [Videos](#videos)
      + Videos\Background
   7. [Logs](#logs)
3. [Кража фокуса экрана другими приложениями](#кража-фокуса-экрана-другими-приложениями)
4. [Deploy](#deploy)
5. [License](#license)
   

## Описание

ПРИЛОЖЕНИЕ ЗАКРЫВАЕТСЯ ТОЛЬКО ЧЕРЕЗ ДИСПЕТЧЕР ЗАДАЧ !!!

Альтернативные методы (Alt+F4 и тп.) работают только в случае, если в настройках стоит режим Администратора.

## Конфигурация

### Settings

В главной директории располагается файл настроек `settings.json`, который имеет следующую структуру:

``` jsonc
{
  "Settings": {
    "isAdminMode": true, // включения режима админского управления, включающего в себя: возможность выключения через alt+F4 и работу мышки
    "needCheckTime": true, // включение режима ночной работы приложения (ночью работает герб МИРЭА, вместо видео)
    "startWorkTime": "08:00:00", // время начала работы сервиса
    "endWorkTime": "22:00:00", // время завершения работы сервиса, переход в спящий режим
    "inactiveModeTime" : "00:01:00", // допустимая длительность бездействия
    "cacheUpdateInterval": "00:20:00", // установка частоты обновления кэша.
    "videoVolume": 1, // установка уровня громкости всех видеозаписей в приложении. Значение: от 0 до 1
    "backgroundVideoOrder": [], // установка плейлиста, в последовательности которого будут воспроизводиться ролики
    "eggVideoCommands": []
  }
}
{
  "needCheckTime": false, 
  "sleepHour": 22, 
  "isAdmin": true, 
  "videoVolume": 0, 
  "minForUpdate": 5, 
  "backgroundVideoOrder": [] /
 }
```

### Cache
В директории `Cache`, где лежат два `.json` файла: `news.json` и `groups.json`. Файл `news.json` нужен для кэширования новостей с сайта вуза. Файл `groups.json` служит для кэширования списка групп института.

### Games

В директории `Games` находятся игры которые можно добавлять. Единственное требование - создать внутри директории Games директорию, название которой будет являться названием панели в самом приложении. Внутри директории с названием игры должен лежать собранный проект с исполняемым файлом игры (.exe).

### TimeTables

В директории `TimeTables` необходимо разместить расписание в формате png(для получение из excel файла png можно воспользоваться любым онлайн конвертером). 

Приняты следующие обозначения для файлов расписания:

- 1.png - расписание первого курса бакалавриата
- 2.png - расписание второго курса бакалавриата
- 3.png - расписание третьего курса бакалавриата
- 4.png - расписание четвертого курса бакалавриата
- 5.png - расписание первого курса магистратуры
- 6.png - расписание второго курса магистратуры

### Videos

Так же для работы видеозаписей используется директория `Videos`. В ней хранятся все видеозаписи телевизора, где название видеофайла является названием панельки видео в программе.

#### Videos\Background

Чтобы добавить видео для фонового проигрывания (режим бездействия), нужно поместить видео ролики в директорию `Videos\Background`.


Поддерживаемые форматы видеофайлов: `ogg`, `mov`, `mp4`.

### Logs

Логи приложения пишутся в файл `logs.txt`, располагаемый в корневом каталоге.

## Кража фокуса экрана другими приложениями

Для модуля KinectControls необходимо наличие фокуса на экране. При отсутствии фокуса на нем вы не сможете взаимодействовать с решением и оно просто не будет реагировать на ввод.

В системе Windows очень часть сторонние приложения запускаемые по таймеру или любому другому тригеру производят эффект, называемый "Stilling Focus". Как описано выше при открытии любого окна поверх приложения оно будет не активно

Для решения данной проблемы мы рекомендуем использовать решение [Turbotop](https://www.savardsoftware.com/turbotop/). Оно достаточно легковесное и позволит вам выбрать в нем окно приложения как активное, что защитит его от кражи фокуса и оно всегда будет воспроизводиться OnTop.

## Deploy

Для развёртывания приложения необходимо предустановить следующие компоненты:

- [Microsoft Net Framework 4.8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [Kinect for Windows SDK 2.0](https://www.microsoft.com/en-us/download/details.aspx?id=44561)
- [Kinect for Windows Runtime 2.2](https://www.microsoft.com/en-us/download/details.aspx?id=100067)


Детальную информацию можно найти в [последнем релизе](https://github.com/RTUITLab/KinectV2TVInteraction/releases/latest)

## License
- [Apache-2.0 License](https://github.com/RTUITLab/ITTV/blob/master/LICENSE)
