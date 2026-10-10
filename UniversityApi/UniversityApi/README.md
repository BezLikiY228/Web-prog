UniversityApi — это REST API для управления учебным процессом университета. Проект позволяет управлять информацией о студентах, преподавателях, учебных курсах и записях студентов на курсы.
Система поддерживает создание, просмотр, изменение и удаление данных, а также фильтрацию курсов и проверку повторной записи студентов на один курс.
Проект предназначен для упрощения управления учебными данными и демонстрирует основные принципы разработки веб-API.
API поддерживает CRUD-операции для студентов, преподавателей, курсов и записей на курсы. Для курсов доступна фильтрация по преподавателю и количеству кредитов. Реализованы проверка входных данных, обработка ошибок и единый формат ответа.
Контроллер	Endpoint	Назначение
Students	GET /api/Students	Получить всех студентов
Students	GET /api/Students/{id}	Получить студента по ID
Students	POST /api/Students	Создать студента
Students	PUT /api/Students/{id}	Обновить студента
Students	DELETE /api/Students/{id}	Удалить студента
Teachers	GET /api/Teachers	Получить преподавателей
Teachers	POST /api/Teachers	Создать преподавателя
Teachers	PUT /api/Teachers/{id}	Обновить преподавателя
Teachers	DELETE /api/Teachers/{id}	Удалить преподавателя
Courses	GET /api/Courses	Получить курсы
Courses	POST /api/Courses	Создать курс
Courses	PUT /api/Courses/{id}	Обновить курс
Courses	DELETE /api/Courses/{id}	Удалить курс
Enrollments	GET /api/Enrollments	Получить записи на курсы
Enrollments	GET /api/Enrollments/{id}	Получить запись по ID
Enrollments	POST /api/Enrollments	Записать студента на курс
Enrollments	PUT /api/Enrollments/{id}	Обновить оценку
Enrollments	DELETE /api/Enrollments/{id}	Удалить запись



Client (Swagger / HTTP)
        |
        v
    Controller
        |
        v
     Service
        |
        v
   Repository
        |
        v
Entity Framework Core
        |
        v
 SQL Server Database