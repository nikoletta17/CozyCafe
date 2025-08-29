using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Exceptions
{
    // abstract class for all exceptions

    /// <summary>
    /// (UA) Абстрактний базовий клас для всіх спеціалізованих винятків у додатку CozyCafe. 
    /// Дозволяє зберігати додаткові дані, пов’язані з помилкою.
    /// 
    /// (EN) Abstract base class for all custom exceptions in the CozyCafe application. 
    /// Allows storing additional data related to the error.
    /// </summary>
    public abstract class AppException : Exception
    {
        public object AdditionalData { get; }

        protected AppException(string message) : base(message) { }

        protected AppException(string message, object additionalData)
            : base(message)
        {
            AdditionalData = additionalData;
        }
    }

    #region General exceptions 

    /// <summary>
    /// (UA) Виняток, який виникає, коли сутність не знайдено у базі даних. 
    /// Може включати назву сутності та її ключ.
    /// 
    /// (EN) Exception thrown when an entity is not found in the database. 
    /// Can include the entity name and its key.
    /// </summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key '{key}' was not found.", new { Entity = entityName, Key = key }) { }

        public NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// (UA) Виняток, який сигналізує про помилки валідації введених даних. 
    /// Може містити додаткові відомості про некоректні поля.
    /// 
    /// (EN) Exception indicating validation errors in input data. 
    /// May contain additional details about invalid fields.
    /// </summary>
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, object errors) : base(message, errors) { }
    }

    /// <summary>
    /// (UA) Виняток, який сигналізує про відсутність прав доступу для виконання дії.
    /// 
    /// (EN) Exception thrown when the user is not authorized to perform an action.
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "You are not authorized to perform this action.")
            : base(message) { }
    }

    /// <summary>
    /// (UA) Виняток, що виникає при конфліктних операціях (наприклад, дублювання даних). 
    /// 
    /// (EN) Exception that occurs during conflicting operations (e.g., duplicate data).
    /// </summary>
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message) { }
    }
    #endregion General exceptions 

    #region Specific exceptions

    /// <summary>
    /// (UA) Спеціалізований виняток для випадків, коли елемент меню не знайдено.
    /// 
    /// (EN) Specialized exception for cases when a menu item is not found.
    /// </summary>
    public class MenuItemNotFoundException : NotFoundException
    {
        public MenuItemNotFoundException(int id)
            : base("Menu item", id) { }
    }

    /// <summary>
    /// (UA) Виняток, який виникає при спробі створити замовлення з порожнім кошиком.
    /// 
    /// (EN) Exception thrown when attempting to create an order with an empty cart.
    /// </summary>
    public class CartEmptyException : AppException
    {
        public CartEmptyException()
            : base("Cannot create an order with an empty cart.") { }
    }

    /// <summary>
    /// (UA) Виняток для некоректної зміни статусу замовлення 
    /// (наприклад, із "Completed" назад у "Pending").
    /// 
    /// (EN) Exception for invalid order status transitions 
    /// (e.g., from "Completed" back to "Pending").
    /// </summary>
    public class InvalidOrderStatusException : AppException
    {
        public InvalidOrderStatusException(string currentStatus, string attemptedStatus)
            : base($"Cannot change order status from '{currentStatus}' to '{attemptedStatus}'.",
                   new { Current = currentStatus, Attempted = attemptedStatus })
        { }
    }

    /// <summary>
    /// (UA) Спеціалізований виняток для випадків, коли товар у замовленні не знайдено.
    /// 
    /// (EN) Specialized exception for cases when an order item is not found.
    /// </summary>
    public class OrderItemNotFoundException : NotFoundException
    {
        public OrderItemNotFoundException(int id)
            : base("Order item", id) { }
    }

    #endregion Specific exceptions

}
