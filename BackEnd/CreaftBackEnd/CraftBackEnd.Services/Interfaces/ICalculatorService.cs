using CraftBackEnd.Common.Models.IO;

namespace CraftBackEnd.Services.Interfaces
{
    public interface ICalculatorService
    {
        NetworthResult CalculateNetworth(NetworthRequest networthRequest, int fieldLengthLimit);
    }
}
