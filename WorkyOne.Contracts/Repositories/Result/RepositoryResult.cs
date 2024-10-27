using WorkyOne.Contracts.Enums.Result;

namespace WorkyOne.Contracts.Repositories.Result
{
    public class RepositoryResult
    {
        public List<string> Succeed { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public RepositoryResult(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        public void AddError(ResultType type, string? entityId, string? entityName)
        {
            Errors.Add(GenerateMessage(type, entityId, entityName));
        }

        public void AddSucceed(ResultType type, string? entityId, string? entityName)
        {
            Succeed.Add(GenerateMessage(type, entityId, entityName));
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

        public static RepositoryResult Error(string message = "Ошибка") =>
            new RepositoryResult(false, message);

        public static RepositoryResult CancellationRequested() =>
            new RepositoryResult(false, "Запрошено завершение задачи");
    }
}
