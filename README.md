# MicroserviceArchitecture

# Описание:
API для просмотра и редактирования данных в таблицах БД.

Изначально данный проект был написан на монолитной архитектуре, для тестового задания другой компании, позже был переписан на микросервисную.
В проекте 2 микросервиса: мироксервис авторизации, микросервис для взаимодействия с таблицами БД.

Основное приложение **IntraVisionTestTask** посылает http-запросы к микросервисам, которые могут взаимодействовать друг с другом.
Например, чтобы в микросервисе **DrinksCoinsMicroservice** обратиться к методу **Add**, контроллера **Drinks** необходимо иметь JWT-токен (тк метод с атрибутом Authorize),
поэтому сначала необходимо обратиться к микросервису **AuthMicroservice**, к методу **AuthorizationMethod** в контроллере **Authorization** и получить JWT-токен, только после
присваивать его в хедер во время запроса к методу **Add**. В проекте **IntraVisionTestTask** при авторизации JWT-токен присваивается сессии на сервере.

СУБД MSSQL Server. Содержит 3 таблицы: 
Users атрибуты: Id, Login, Password, Role (enum).
Drinks атрибуты: Id, Name, Price, Amount, Img.
Coins, атрибуты: Id, Amount, IsBlocked, Denomination (enum).

Проект имеет авторизацию через JWT с асимметричным шифрованием ключей, разделением ролей на администратора и пользователя.
Администратор может добавлять/редактировать/удалять таблицу Drink, редактировать некоторые данные атрибутов (Amount и IsBlocked) таблицы Coins.
Пользователь не наделён никаким правами, так как изначально проект задумывался как MVC (для другого тестового задания), с возможностью покупкать товары, что я и реализовал в ветке **MVC**, здесь же
писать отдельный микросервис для возможности покупать товары - не хватает времени.
Главная цель данного проекта, продемонстрировать вам мои возможности, что проект отлично делает и без микросервиса покупок товаров.

В проекте так же реализован функционал глобальной обработки ошибок.

При запуске проекта **IntraVisionTestTask** автоматически создаётся локальная бд и таблицы заполняются данными.

Чтобы авторизироваться под админа: логин: adm, пароль: adm.

# Установка:
**!!!Важно!!!** - после клонирования репозитория к себе на ПК, запускать основное приложение **IntraVisionTestTask** через **http**, на порту **5000**, после запустить приложения **AuthMicroservice** через **http** на порту **5001**
и **DrinksCoinsMicroservice** тоже через **http** на порту **5002**, я запускал **AuthMicroservice** и **DrinksCoinsMicroservice** через командную строку, пример: **cd <Путь до проекта>\\AuthMicroservice --> dotnet run**, 
если запускать не по этой инструкции, то микросервисы просто не будут работать.

Конфигурация проекта **IntraVisionTestTask** находится в файле **appsettings.json**.
Конфигурация проекта **AuthMicroservice** находится в файле **appsettings.json**.
Конфигурация проекта **DrinksCoinsMicroservice** находится в файле **appsettings.json**.
