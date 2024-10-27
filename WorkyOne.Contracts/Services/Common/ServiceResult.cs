using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Repositories.Common;

namespace WorkyOne.Contracts.Services.Common
{
    /// <summary>
    /// Результат выполнения действий сервисом
    /// </summary>
    public sealed class ServiceResult
    {
        /// <summary>
        /// Ошибки, возникшие в процессе выполнения действия сервисом
        /// </summary>
        private List<string> _errors = new List<string>();

        /// <summary>
        /// Сообщения об успешном выполнении действия сервисом
        /// </summary>
        public string? SucceedMessage { get; set; }

        /// <summary>
        /// Индикатор успешности выполнения действия сервисом
        /// </summary>
        public bool IsSucceed => _errors.Count == 0;

        public ServiceResult() { }

        public ServiceResult(string succeedMessage)
        {
            SucceedMessage = succeedMessage;
        }

        /// <summary>
        /// Добавляет ошибку с список ошибок
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        public void AddError(string message) => _errors.Add($"Ошибка: {message}");

        /// <summary>
        /// Возвращает список ошибок
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<string> GetErrors() => new ReadOnlyCollection<string>(_errors);

        public static ServiceResult Ok(string? message) => new ServiceResult(message ?? "Успех!");

        public static ServiceResult Error(string message)
        {
            var result = new ServiceResult();
            result.AddError(message);
            return result;
        }

        public static ServiceResult AccessDenied()
        {
            var result = new ServiceResult();
            result.AddError("Доступ запрещён");
            return result;
        }

        public static ServiceResult CancellationRequested() => Error("Задача отменена");

        public static ServiceResult FromRepositoryResult(RepositoryResult repoResult)
        {
            var result = new ServiceResult();

            foreach (var error in repoResult.Errors)
            {
                result.AddError(error.ErrorMessage());
            }

            return result;
        }
    }
}
