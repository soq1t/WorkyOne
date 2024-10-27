using System.Text;
using WorkyOne.Contracts.Enums.Result;

namespace WorkyOne.Contracts.Repositories.Result
{
    public sealed class ResultItem
    {
        public ResultItem(string? entityId, string? entityname, ResultType? type)
        {
            EntityId = entityId;
            Entityname = entityname;
            Type = type;
        }

        public string? EntityId { get; private set; }

        public string? Entityname { get; private set; }

        public ResultType? Type { get; private set; }

        public string Message
        {
            get
            {
                var id = EntityId ?? "Unknown";
                var name = Entityname ?? "Unknown";
                var message = "{1} [ID: {2}] - {3}";

                switch (Type)
                {
                    case ResultType.Created:
                        return string.Format(message, name, id, "Entity succesfully created");
                    case ResultType.Updated:
                        return string.Format(message, name, id, "Entity succesfully updated");
                    case ResultType.Deleted:
                        return string.Format(message, name, id, "Entity succesfully deleted");
                    case ResultType.NotFound:
                        return string.Format(message, name, id, "Entity not found");
                    case ResultType.AlreadyExisted:
                        return string.Format(message, name, id, "Entity already existed");
                    default:
                        return string.Format(message, name, id, "Unknown error");
                }
            }
        }
    }
}
