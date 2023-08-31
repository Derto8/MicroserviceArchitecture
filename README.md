# DrinksBuyApplication
### Стек технологий проекта:
<img src="https://img.shields.io/badge/ASP.NET WEB API-black?style=for-the-badge&logo=.NET&logoColor=512BD4"/> <img src="https://img.shields.io/badge/ASP.NET MVC-black?style=for-the-badge&logo=.NET&logoColor=512BD4"/> <img src="https://img.shields.io/badge/JavaScript-black?style=for-the-badge&logo=javascript&logoColor=F7DF1E"/> <img src="https://img.shields.io/badge/ORM EntityFramework-black?style=for-the-badge&logo=.NET&logoColor=512BD4"/> <img src="https://img.shields.io/badge/MSSQL Server-black?style=for-the-badge&logo=microsoftsqlserver&logoColor=CC2927"/> <img src="https://img.shields.io/badge/AJAX-black?style=for-the-badge&logo=javascript&logoColor=3A76F0"/> <img src="https://img.shields.io/badge/JsonWebToken-black?style=for-the-badge&logo=jsonwebtokens&logoColor=white"/> <img src="https://img.shields.io/badge/JQuery-black?style=for-the-badge&logo=jquery&logoColor=0769AD"/>

# Описание:

Проект для покупки напитков, с возможностью изменения данных в бд через клиент (браузер).

СУБД MSSQL Server. Содержит 3 таблицы: 

Users атрибуты: Id, Login, Password, Role (enum).

Drinks атрибуты: Id, Name, Price, Amount, Img.

Coins, атрибуты: Id, Amount, IsBlocked, Denomination (enum).



Проект имеет авторизацию через JWT с асимметричным шифрованием ключей, разделением ролей на администратора и пользователя.
Пользователь может покупать напитки за монеты, тем самым увеличивая в таблице бд Coins количество (amount) какой-либо монеты определённого номинала (Denomination), а так же в таблице бд Drinks, уменьшая количество (amount) на 1 единицу.
Если у пользователя не достаточно монет для покупки, или напитки закончились (amount = 0), то это показывается на клиенте.
В проекте так же реализован функционал глобальной обработки ошибок.

Администратор может добавлять/редактировать/удалять таблицу Drink, редактировать некоторые данные атрибутов (Amount и IsBlocked) таблицы Coins.

# Установка:
**!!!Важно!!!** - после клонирования репозитория к себе на ПК, запускать основной MVC приложение **IntraVisionTestTask** через **http**, на порту **5000**, после запустить приложение **AuthMicroservice** тоже через **http** на порту **5001**,
я запускал **AuthMicroservice** через командную строку, пример: **cd <Путь до проекта>\\AuthMicroservice --> dotnet run**, если запускать не по этой инструкции, то авторизация юзера не будет работать.

Конфигурация проекта **IntraVisionTestTask** находится в файле **appsettings.json**.
Конфигурация проекта **AuthMicroservice** находится в файле **appsettings.json**.

При запуске проекта **IntraVisionTestTask** автоматически создаётся локальная бд и таблицы заполняются данными.
