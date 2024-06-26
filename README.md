# UsersStore API

UsersStore API - это простой веб-API для управления пользователями. Он включает функции для создания, обновления, удаления и получения профилей пользователей. Проект построен с использованием ASP.NET Core и Entity Framework Core.

## Функции
- Аутентификация и авторизация пользователей
- CRUD операции для профилей пользователей
- Функциональность мягкого удаления
- Эндпоинты, доступные только администраторам
- Инициализация пользователя-администратора при старте

## Необходимые компоненты
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/ru-ru/sql-server/sql-server-downloads)


## Начало работы
#### Клонирование репозитория
```sh
git clone https://github.com/qweabuzer/UserStore_WEB_API.git
cd UserStore_WEB_API
```

## Применение миграций
#### Перейдите в проект UsersStore.DataAccess и примените миграции:
```sh
cd UsersStore.DataAccess
dotnet ef --startup-project ../UsersStore.Api migrations add InitialCreate
dotnet ef --startup-project ../UsersStore.Api database update
```
## Сборка и запуск приложения
#### Перейдите обратно в корневую директорию и запустите приложение:
```sh
cd ..
dotnet build
dotnet run --project UsersStore.Api
```

## Эндпоинты API
##### GET /users/AllActive: Получить список всех активных пользователей (Только для администратора)
##### GET /users/ByLogin: Получить пользователя по логину (Только для администратора)
##### GET /users/Profile: Получить профиль пользователя по логину и паролю (Администратор или сам пользователь, если активен)
##### GET /users/ByAge: Получить список пользователей старше определенного возраста (Только для администратора)
##### POST /users/Create: Создать нового пользователя (Только для администратора)
##### PUT /users/UpdateInfo: Обновить информацию пользователя (Администратор или сам пользователь, если активен)
##### DELETE /users/Delete: Удалить пользователя (Только для администратора)
##### PATCH /users/Revoke: Мягко удалить пользователя (Только для администратора)
##### PATCH /users/Unban: Восстановить удаленного пользователя (Только для администратора)

## Инициализация пользователя-администратора
#### При старте приложение проверяет наличие пользователя-администратора. Если его нет, создается администратор с следующими данными:
##### Логин: admin
##### Пароль: admin
