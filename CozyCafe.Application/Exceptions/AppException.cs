using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Exceptions
{
    // abstract class for all exceptions
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
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key '{key}' was not found.", new { Entity = entityName, Key = key }) { }

        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, object errors) : base(message, errors) { }
    }

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "You are not authorized to perform this action.")
            : base(message) { }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message) { }
    }
    #endregion General exceptions 

    #region Specific exceptions

    public class MenuItemNotFoundException : NotFoundException
    {
        public MenuItemNotFoundException(Guid id)
            : base("Menu item", id) { }
    }

    public class CartEmptyException : AppException
    {
        public CartEmptyException()
            : base("Cannot create an order with an empty cart.") { }
    }

    public class InvalidOrderStatusException : AppException
    {
        public InvalidOrderStatusException(string currentStatus, string attemptedStatus)
            : base($"Cannot change order status from '{currentStatus}' to '{attemptedStatus}'.",
                   new { Current = currentStatus, Attempted = attemptedStatus })
        { }
    }

    public class OrderItemNotFoundException : NotFoundException
    {
        public OrderItemNotFoundException(Guid id)
            : base("Order item", id) { }
    }

    #endregion Specific exceptions

}
