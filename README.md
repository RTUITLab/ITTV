# ITTV


## Содержание

1. [Описание](#описание)
   1. [Режим администратора](#режим-администратора)
2. [Стек](#стек)
3. [Конфигурация](#конфигурация)
   1. [Configuration](#configuration)
   2. [Cache](#cache)
   4. [Games](#games)
   5. [ScheduleImages](#scheduleimages)
   6. [Videos](#videos)
      + Videos\Background
   7. [Logs](#logs)
4. [Deploy](#deploy)
5. [License](#license)
   

## Описание

Данное приложение позволяет просматривать расписание Института ИТ РТУ МИРЭА, запускать различные поддерживаемые игры, просматривать видеоролики и новости с [оффициального сайта](https://www.mirea.ru/news/) РТУ МИРЭА. 

Также дает возможность взаимодействовать с приложением путем использования внешнего оборудования - [Kinect'a](https://ru.wikipedia.org/wiki/Kinect). 

Адаптировано только под Kinect v2.0 Microsoft Xbox.

### Режим Администратора

Существуют следующие особенности данного режима:
   - В случае, если режим Администратора не активен, приложение невозможно закрыть стандартными методами, в том числе Alt+F4, так как оно будет бесконечно перезапускаться
   - В режиме Администратора в верхнем правом углу будут отображаться уведомления об ошибках в работе приложения
   - В режиме Администратора отображается курсор мыши

Для включения данного режима необходимо в [файле конфигурации](#configuration) указать полю `isAdminMode` значение `true`.

## Стек

Здесь используются следующие библиотеки и фреймворки:
-  [WPF .NET Framework 4.8](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netframeworkdesktop-4.8)
-  [Serilog](https://github.com/serilog/serilog) - для структурного логирования
-  [Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
-  [AngleSharp](https://github.com/AngleSharp/AngleSharp) - для парсинга данных с интернет-ресурсов
-  XUnit - для написания Unit тестов
-  [Microsoft.Kinect.WPF.Controls](https://www.nuget.org/packages/Microsoft.Kinect.Wpf.Controls/) - для работы с Kinect-ом

Также активно используется паттерн разработки [MVVM](https://ru.wikipedia.org/wiki/Model-View-ViewModel).

## Конфигурация

### Configuration

В главной директории располагаются файл конфигурации `configuration.json`, который имеет следующую структуру:

``` jsonc
{
  "Settings": {
    "isAdminMode": true, // включения режима админского управления, включающего в себя: возможность выключения через alt+F4 и работу мыши
    "needCheckTime": true, // включение режима ночной работы приложения (ночью работает герб РТУ МИРЭА, вместо фоновых видеороликов)
    "startWorkTime": "08:00:00", // время начала работы сервиса, HH:mm:ss
    "endWorkTime": "22:00:00", // время завершения работы сервиса, переход в спящий режим, HH:mm:ss
    "inactiveModeTime" : "00:01:00", // допустимая длительность бездействия, HH:mm:ss
    "cacheUpdateInterval": "00:20:00", // частота обновления кэшируемых данных, HH:mm:ss
    "videoVolume": 1, // уровень громкости всех видеороликов в приложении. Значение: от 0 до 1
    "backgroundVideoOrder": [], // плейлист фоновых видеороликов
    "eggVideoCommands": []
  }
}
```

и файл конфигурации логирования `configuration.logging.json`:

```jsonc
  "ApiSeqLoggerSettings": {
    "Uri": "", // uri к серверу Seq
    "ApiKey": "" // apiKey для записи логов на сервер Seq
  }
```

### Cache

При взаимодействии с сервером все данные (расписание групп, новости) кэшируются на период, заданный в конфигурации в поле `cacheUpdateInterval`.

Пример конфигурации при периоде кэширования 30 минут:

``` jsonc
{
  "Settings": {
    "isAdminMode": false, 
    "needCheckTime": false, 
    "startWorkTime": "00:00:00", 
    "endWorkTime": "00:00:00", 
    "inactiveModeTime" : "00:00:00", 
    "cacheUpdateInterval": "00:30:00", 
    "videoVolume": 0, 
    "backgroundVideoOrder": [], 
    "eggVideoCommands": []
  }
}
```

Проверка актуальности кэша осуществляется во время обращении к нему, за исключением кэша новостей. Последний обновляется постоянно, с частотой, указаной в поле `cacheUpdateInterval`.

### Games

Для размещения игр необходимо добавить директорию с исполняемым файлом игры в директорию `Games`. 
Название директории будет являться названием игры в разделе `Игры`.

### ScheduleImages

В директории `Images\ScheduleImages` необходимо разместить расписание курса в поддерживаемом формате (для получение из excel файла изображения можно воспользоваться любым онлайн конвертером). 

Поддерживаемые форматы изображения : `.png`, `.jpg`, `.jpeg`.

Расписание для курсов бакалавриата размещается в директории `Images\ScheduleImages\Bachelor`:

   - 1.* - расписание первого курса
   - 2.* - расписание второго курса
   - 3.* - расписание третьего курса
   - 4.* - расписание четвертого курса 

Расписание для курсов бакалавриата размещается в директории `Images\ScheduleImages\Master`:

   - 1.* - расписание первого курса
   - 2.* - расписание второго курса

Пример полного названия файла - `1.png`.

### Videos

Поддерживаемые форматы видеофайлов: `ogg`, `mov`, `mp4`.

Для добавления видеоролика в раздел `Видео` необходимо разместить видео в поддерживаемом формате в директории `Videos`.

#### Videos\Background

Чтобы добавить видео для фонового проигрывания (режим бездействия), нужно поместить видеоролики в директорию `Videos\Background` и прописать название видеоролика в файле `configuration.json`.

Пример конфигурации при названии ролика `1.mp4`:
``` jsonc
{
  "Settings": {
    "isAdminMode": false, 
    "needCheckTime": false, 
    "startWorkTime": "00:00:00", 
    "endWorkTime": "00:00:00", 
    "inactiveModeTime" : "00:00:00", 
    "cacheUpdateInterval": "00:00:00", 
    "videoVolume": 0, 
    "backgroundVideoOrder": [
       "1.mp4"
    ], 
    "eggVideoCommands": []
  }
}
```

### Logs

Логи приложения пишутся в файл `logs.txt`, который располагается в корневом каталоге.

Также при задании конфигурации в секции `ApiSeqLoggerSettings` в файле `configuration.logging.json` возможна отправка логов на удаленный сервер. 

При пустых или не указанных значениях данной секции отправка логов на удаленный сервер отключена.

Пример конфигурации

``` jsonc
{
    "ApiSeqLoggerSettings": {
    "Uri": "http://localhost:5431",
    "ApiKey": "my_secret_api_key_for_write_logs"
  }
}
```

## Deploy

Для развертывания приложения необходимо предустановить следующие компоненты:

- [Microsoft Net Framework 4.8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [Kinect for Windows SDK 2.0](https://www.microsoft.com/en-us/download/details.aspx?id=44561)
- [Kinect for Windows Runtime 2.2](https://www.microsoft.com/en-us/download/details.aspx?id=100067)


Детальную информацию можно найти в [последнем релизе](https://github.com/RTUITLab/KinectV2TVInteraction/releases/latest)

## License
- [Apache-2.0 License](https://github.com/RTUITLab/ITTV/blob/master/LICENSE)
