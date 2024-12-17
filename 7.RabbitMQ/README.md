# Otus.Teaching.PromoCodeFactory

Проект для домашних заданий и демо по курсу `C# ASP.NET Core Разработчик` от `Отус`.
Cистема `Promocode Factory` для выдачи промокодов партнеров для клиентов по группам предпочтений.

Подробное описание проекта и описание домашних заданий можно найти в [Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Home)

Данный проект является стартовой точкой для Homework №7

Описание домашнего задания в [Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Homework-7)

# Подключить RabbitMQ к своему проекту

1. Добавляем RabbitMq в docker-compose файл.
2. Подключаем RabbitMq в микросервисы Выдача промокода клиенту (Otus.Teaching.Pcf.GivingToCustomer), Администрирование (Otus.Teaching.Pcf.Administration) и по желанию в Получение промокода от партнера (Otus.Teaching.Pcf.ReceivingFromPartner). Можно использовать драйвер RabbitMq для .NET, тогда вероятно нужно будет использовать Hosted Service для прослушивания очереди, также можно использовать MassTransit или NServiceBus.
3. При получении промокода от партнера в микросервисе Otus.Teaching.Pcf.ReceivingFromPartner в методе ReceivePromoCodeFromPartnerWithPreferenceAsync контроллера PartnersController в конце выполнения сейчас происходят вызовы _givingPromoCodeToCustomerGateway и _administrationGateway, где через HTTPClient изменяются данных в других микросервисах, такой способ является синхронным и при росте нагрузке или отказе одного из сервисов приведет к отказу всей операции.
4. Для повышения надежности вместо синхронных вызовов нужно отправлять одно событие в RabbitMq, тогда в микросервисах Otus.Teaching.Pcf.Administration и по желанию в Otus.Teaching.Pcf.GivingToCustomer нужно сделать подписку на данное событие и реализовать аналог текущих синхронных операций, вызываемых через API.
5. Для того, чтобы ту же логику можно было вызвать из консюмера очереди ее надо перенести из контроллера в класс-сервис, который нужно разместить в проекте Core, в контроллере и консюмере очереди надо вызывать метод этого сервиса.

<b>Инструкция по запуску:</b><br/>
Проект состоит из трех микросервисов и их баз данных, настройка Posgress баз для них приведена в docker-compose файле в корне репозитория, чтобы запустить только базы данных выполняем команду:
docker-compose up promocode-factory-administration-db promocode-factory-receiving-from-partner-db promocode-factory-giving-to-customer-db
Сами сервисы доступны в общем solution: Otus.Teaching.Pcf.sln
Если базы данных запущены, то в Visual Studio или Rider настраиваем запуск нескольких проектов сразу и работаем с API через Swagger, для API в Swagger добавлены примеры данных для вызова и тестирования.


<b>Критерии оценки:</b><br/>
Подключен RabbitMq, синхронный вызов по HTTP в _administrationGateway заменен передачей события в RabbitMq, в Otus.Teaching.Pcf.Administration реализовано обновление количества промокодов у сотрудника через класс-сервис: 7 баллов;

Подключен RabbitMq, синхронные вызовы по HTTP в _administrationGateway и _givingPromoCodeToCustomerGateway заменены передачей события в RabbitMq (тут можно просто отправить одно событие и подписаться на него в двух сервисах), в Otus.Teaching.Pcf.Administration реализовано обновление количества промокодов у сотрудника через класс-сервис, а в Otus.Teaching.Pcf.GivingToCustomer реализована выдача промокода клиентам через подписку на событие в Rabbit, код перенесен в класс-сервис: 10 баллов.

Минимальный проходной балл: 7 баллов.