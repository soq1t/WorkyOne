using System.Text;
using WorkyOne.Contracts.Enums.Result;

namespace WorkyOne.Contracts.Repositories.Result
{
    public sealed class ResultItem
    {
        public string? EntityId { get; private set; }

        public ResultType? Type { get; private set; }

        public string Message
        {
            get
            {
                var id = EntityId ?? "Unknown";

                switch (Type)
                {
                    case ResultType.Created:
                        return string.Format("ID: [{1}] - {2}", id, "Entity succesfully created");
                    case ResultType.Updated:
                        return string.Format("ID: [{1}] - {2}", id, "Entity succesfully updated");
                    case ResultType.Deleted:
                        return string.Format("ID: [{1}] - {2}", id, "Entity succesfully deleted");
                    case ResultType.NotFound:
                        return string.Format("ID: [{1}] - {2}", id, "Entity not found");
                    case ResultType.AlreadyExisted:
                        return string.Format("ID: [{1}] - {2}", id, "Entity already existed");
                    default:
                        return string.Format("ID: [{1}] - {2}", id, "Unknown error");
                }
            }
        }
    }
}
