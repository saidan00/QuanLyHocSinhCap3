using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IResultRepository : IRepository<Result>
    {
        ResultType GetResultType(int resultTypeId);
        ResultDetail GetResultDetail(int resultId, int resultTypeId, int month);
        void AddDetail(ResultDetail detail);
    }
}