﻿using WorkyOne.Contracts.Enums.Result;

namespace WorkyOne.Contracts.Repositories.Result
{
    public class RepositoryResult
    {
        public List<string> SucceedItems { get; set; } = new List<string>();
        public List<string> ErrorItems { get; set; } = new List<string>();
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public RepositoryResult(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        public void AddError(ResultType type, string? entityId, string? entityName)
        {
            ErrorItems.Add(GenerateMessage(type, entityId, entityName));
        }

        public void AddSucceed(ResultType type, string? entityId, string? entityName)
        {
            SucceedItems.Add(GenerateMessage(type, entityId, entityName));
        }

        private string GenerateMessage(ResultType type, string? entityId, string? entityName)
        {
            entityId = entityId ?? "Unknown";
            entityName = entityName ?? "Unknown";

            string message = type switch
            {
                ResultType.Created => "Entity succesfully created",
                ResultType.Updated => "Entity succesfully updated",
                ResultType.Deleted => "Entity succesfully deleted",
                ResultType.NotFound => "Entity not found",
                ResultType.AlreadyExisted => "Entity already existed",
                _ => "Unknown error",
            };

            return $"Entity [{entityName}] (ID: {entityId}) - {message}";
        }

        public static RepositoryResult Ok(string message = "Успех!") =>
            new RepositoryResult(true, message);

        public static RepositoryResult Ok(ResultType type, string? entityId, string? entityName)
        {
            var result = Ok();
            result.AddSucceed(type, entityId, entityName);
            return result;
        }

        public static RepositoryResult Error(string message = "Ошибка") =>
            new RepositoryResult(false, message);

        public static RepositoryResult Error(ResultType type, string? entityId, string? entityName)
        {
            var result = Error();
            result.AddError(type, entityId, entityName);
            return result;
        }

        public static RepositoryResult CancellationRequested() =>
            new RepositoryResult(false, "Запрошено завершение задачи");
    }
}
