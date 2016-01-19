using System;

namespace VendingMachine.Domain.Erros
{
    /// <summary>
    /// Ошибки торгового автомата
    /// </summary>
    public class VMException : Exception
    {
        #region ctor

        public VMException(String message) 
            : base(message)
        {
        }

        #endregion

        /// <summary>
        /// Товар отсутствует
        /// </summary>
        public static readonly VMException ProductNotFound = new VMException("Товар отсутствует");

        /// <summary>
        /// Недостаточно средств
        /// </summary>
        public static readonly VMException InsufficientFunds = new VMException("Недостаточно средств");

        /// <summary>
        /// Нет сдачи
        /// </summary>
        public static readonly VMException NoChange = new VMException("Нет сдачи");
    }
}
