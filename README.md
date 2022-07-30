# DPSystem

DPSystem - data processing system - система обработки файлов заданной структуры.

## Назначение системы

Назначение системы - обмен информацией между автосалонами, имеющими разное программное обеспецение для инвентаризации текущих запасов автомобилей, поэтому данные по продаваемым авто могут храниться в файлах различных форматов (json, xml, yaml, csv).

Примеры файлов:

- [json](https://github.com/v-blinov/DPSystem/blob/master/DPSystem.Tests/TestFiles/Correct/DefaultJson.json)
- [xml](https://github.com/v-blinov/DPSystem/blob/master/DPSystem.Tests/TestFiles/Correct/DefaultXml.xml)
- [yaml](https://github.com/v-blinov/DPSystem/blob/master/DPSystem.Tests/TestFiles/Correct/DefaultYaml.yaml)
- [csv](https://github.com/v-blinov/DPSystem/blob/master/DPSystem.Tests/TestFiles/Correct/DefaultCsv_v1.csv)


## Требования и ограничения проекта

1. формат данных для автосалона заводится один раз, но в будущем возможно добавление / удаление некоторых полей, поэтому должна поддерживаться версионность файлов;
2. разворачивание проекта происходит через docker-compose;
3. при регистрации пользователя должно происходить подтверждение email-адреса отправкой не него уникальной ссылки для перехода;
4. альтернативным вариантом аутентификации является вход с использованием учетной записи google;
5. сервис по отправке сообщений реализован с помощью брокера сообщений RabbitMQ, на текущий момент сообщения отправляются по email, в будущем должны использоваться и другие каналы связи;
6. список форматов обрабатываемых файлов: json, xml, yaml, csv.

## Ключевые слова

- .Net 5
- asp.net core web api
- ef core
- docker
- docker-compose
- rabbitmq
- xUnit
- publisher/subscriber pattern
- google oauth 2

## Порядок развертывания

1. Склонировать репозиторий:

    ```bash
    git clone https://github.com/v-blinov/DPSystem.git
    ```

2. Перейти в папку с решением
3. В проекте web api в файле appsetting.json указать данные для подключения к БД, ClientID и ClientSecret для google oauth, данные почты для сервиса рассылки сообщений
4. Поднять проект в docker-compose

    ```bash
    # Находимся в папке решения

    # собрать проект из нескольких контейнеров
    docker-compose build

    # запустить проект
    docker-compose up -d
    ```

5. Открыть swagger в браузере по адресу [http://localhost:90/](http://localhost:90/)
